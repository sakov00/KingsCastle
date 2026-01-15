using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.WindowElements
{
    public class LevelScrollRect : ScrollRect
    {
        private int _currentLevel;
        private List<LevelPanel> _levelPanels;
        private int _levelWidth;
        private int _spaceBetweenLevels;
        private int _totalLevels;       
        private float _minX;
        private float _maxX;
        
        private float _lastContentX;
        
        public void Initialize(int currentLevel, List<LevelPanel> levelPanels, int levelWidth, int spaceBetweenLevels, int totalLevels)
        {
            _currentLevel = currentLevel;
            _levelPanels = levelPanels;
            _spaceBetweenLevels = spaceBetweenLevels;
            _levelWidth = levelWidth;
            _totalLevels = totalLevels;
            CalculateLimits();
        }

        private void CalculateLimits()
        {
            float contentWidth = _totalLevels * _levelWidth + (_totalLevels - 1) * _spaceBetweenLevels;
            float viewportWidth = viewport.rect.width;

            _minX = 0f;
            _maxX = Mathf.Max(contentWidth - viewportWidth, 0f);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            ClampContentPosition();
        }

        public override void OnScroll(PointerEventData data)
        {
            base.OnScroll(data);
            ClampContentPosition();
        }
        
        protected override void LateUpdate()
        {
            base.LateUpdate();
            ClampContentPosition();

            float delta = content.anchoredPosition.x - _lastContentX;
            float step = _levelWidth + _spaceBetweenLevels;

            while (Mathf.Abs(delta) >= step)
            {
                if (delta > 0)
                {
                    MoveLeftToRight();
                    _lastContentX += step;
                }
                else
                {
                    MoveRightToLeft();
                    _lastContentX -= step;
                }

                delta = content.anchoredPosition.x - _lastContentX;
            }
        }

        
        private void MoveLeftToRight()
        {
            if(_levelPanels == null || _levelPanels.Count == 0) return;
            
            var last = _levelPanels[^1];
            _levelPanels.RemoveAt(_levelPanels.Count - 1);
            _levelPanels.Insert(0, last);

            var first = _levelPanels[1];
            last.transform.SetAsFirstSibling();
            last.SetPosition(first.GetPosition() - new Vector2(_levelWidth + _spaceBetweenLevels, 0));

        }

        private void MoveRightToLeft()
        {
            if(_levelPanels == null || _levelPanels.Count == 0) return;
            
            var first = _levelPanels[0];
            _levelPanels.RemoveAt(0);
            _levelPanels.Add(first);

            var last = _levelPanels[^2];
            first.transform.SetAsLastSibling();
            first.SetPosition(last.GetPosition() + new Vector2(_levelWidth + _spaceBetweenLevels, 0));
        }

        private void ClampContentPosition()
        {
            if (horizontal && content != null)
            {
                float x = Mathf.Clamp(content.anchoredPosition.x, -_maxX, -_minX);
                content.anchoredPosition = new Vector2(x, content.anchoredPosition.y);
            }
        }
    }
}