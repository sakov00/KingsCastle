using System;
using _Project.Scripts._GlobalLogic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.SpecialItems.Drag
{
    public enum DragAxis
    {
        Both,
        OnlyX,
        OnlyY
    }

    [RequireComponent(typeof(RectTransform))]
    public class DraggableOffset : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private bool _isDraggable = true;

        [Header("Movement Axis")]
        [SerializeField] private DragAxis _dragAxis = DragAxis.Both;

        [Header("Manual Limits (anchoredPosition limits)")]
        [SerializeField] private bool _useLimitsX = false;
        [SerializeField] private bool _useLimitsY = false;
        [SerializeField] private Vector2 _maxLimit;
        [SerializeField] private Vector2 _minLimit;

        public bool IsDraggable
        {
            get => _isDraggable;
            set
            {
                _isDraggable = value;
                if (!_isDraggable) _isDragging = false;
            }
        }

        public Transform StartParent { get; private set; }
        public int SiblingNumber { get; private set; }
        public Vector2 StartAnchoredPosition { get; private set; }

        private Vector3 _offset;   // <-- смещение в локальных координатах
        private bool _isDragging;

        public DraggableOffsetEvent OnPointerDowned;
        public DraggableOffsetEvent OnBeginedDrag;
        public DraggableOffsetEvent OnDragging;
        public DraggableOffsetEvent OnEndedDrag;
        public DraggableOffsetEvent OnPointerUpped;

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
        }

        private void Awake()
        {
            StartParent = transform.parent;
            SiblingNumber = transform.GetSiblingIndex();
            StartAnchoredPosition = _rectTransform.anchoredPosition;
        }

        // ----------------------------------------------------------

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsDraggable) return;

            _rectTransform.DOKill();
            OnPointerDowned?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!IsDraggable) return;

            _rectTransform.DOKill();
            _isDragging = true;

            // --- рассчитываем offset в локальных координатах ---
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)_rectTransform.parent,
                    eventData.position,
                    GlobalObjects.Camera,
                    out var localPoint))
            {
                _offset = _rectTransform.anchoredPosition - localPoint;
            }

            OnBeginedDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging || !IsDraggable) return;
            UpdatePosition(eventData);
        }

        // ----------------------------------------------------------

        private void UpdatePosition(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)_rectTransform.parent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out var localPoint))
                return;

            Vector2 newPos = localPoint + (Vector2)_offset;

            // Ограничение по осям
            switch (_dragAxis)
            {
                case DragAxis.OnlyX:
                    newPos.y = _rectTransform.anchoredPosition.y;
                    break;
                case DragAxis.OnlyY:
                    newPos.x = _rectTransform.anchoredPosition.x;
                    break;
            }

            // Ручные лимиты
            if (_useLimitsX)
                newPos.x = Mathf.Clamp(newPos.x, _minLimit.x, _maxLimit.x);

            if (_useLimitsY)
                newPos.y = Mathf.Clamp(newPos.y, _minLimit.y, _maxLimit.y);

            // Лимиты по размеру родителя
            var parent = (RectTransform)_rectTransform.parent;
            var parentRect = parent.rect;
            var rect = _rectTransform.rect;

            float halfW = rect.width * 0.5f;
            float halfH = rect.height * 0.5f;

            newPos.x = Mathf.Clamp(
                newPos.x,
                parentRect.xMin + halfW,
                parentRect.xMax - halfW
            );

            newPos.y = Mathf.Clamp(
                newPos.y,
                parentRect.yMin + halfH,
                parentRect.yMax - halfH
            );

            _rectTransform.anchoredPosition = newPos;

            OnDragging?.Invoke(this);
        }

        // ----------------------------------------------------------

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragging || !IsDraggable) return;

            _isDragging = false;
            OnEndedDrag?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpped?.Invoke(this);
        }

        public void CancelDrag()
        {
            IsDraggable = false;
            _rectTransform.DOAnchorPos(StartAnchoredPosition, 0f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _rectTransform.SetParent(StartParent);
                    _rectTransform.SetSiblingIndex(SiblingNumber);
                    IsDraggable = true;
                })
                .Play();
        }
        

        private void OnDestroy()
        {
            OnBeginedDrag = null;
            OnEndedDrag = null;
        }
    }

    [Serializable]
    public class DraggableOffsetEvent : UnityEvent<DraggableOffset> { }
}
