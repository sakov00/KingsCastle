using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts._GlobalLogic;

namespace _Project.Scripts.DraggableObjects
{
    public class CustomScrollRect : ScrollRect, IPointerUpHandler, IPointerClickHandler
    {
        private bool _lockScroll;
        private bool _directionChosen;
        private GameObject _currentChildObject;
        
        
        public void OnPointerClick(PointerEventData eventData)
        {
            ExecuteEvents.ExecuteHierarchy(
                eventData.pointerPress,
                eventData,
                ExecuteEvents.pointerClickHandler
            );
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            _currentChildObject = null;
            _lockScroll = false;
            _directionChosen = false;

            if (!_directionChosen)
            {
                Vector2 delta = eventData.delta;

                // если движения мало — игнорируем
                if (delta.magnitude > 2f)
                {
                    // угол движения в градусах
                    float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

                    // проверяем: движение вверх под ±45° (45° < angle < 135°)
                    if (angle > 45f && angle < 135f)
                    {
                        _lockScroll = true; // будем тянуть объект
                        PassThroughClick(eventData);
                        DragNewObject(eventData);
                    }
                }

                _directionChosen = true;
            }

            if (!_lockScroll)
            {
                base.OnBeginDrag(eventData);
            }
        }
        
        public override void OnDrag(PointerEventData eventData)
        {
            if (!_lockScroll)
            {
                base.OnDrag(eventData);
            }
            else
            {
                ExecuteEvents.Execute(_currentChildObject, eventData, ExecuteEvents.dragHandler);
            }
        }
        
        public override void OnEndDrag(PointerEventData eventData)
        {
            _lockScroll = false;
            _directionChosen = false;
            ExecuteEvents.Execute(_currentChildObject, eventData, ExecuteEvents.endDragHandler);
            base.OnEndDrag(eventData);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            ExecuteEvents.Execute(_currentChildObject, eventData, ExecuteEvents.pointerUpHandler);
        }
        
        private void PassThroughClick(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            GlobalObjects.EventSystem.RaycastAll(eventData, results);
            
            if (results.Count < 2) 
                return;
            
            _currentChildObject = results[1].gameObject;
        }

        private void DragNewObject(PointerEventData eventData)
        {
            if (_currentChildObject == null) return;
            _currentChildObject.GetComponent<BowlItemSpawner>();
            
            ExecuteEvents.Execute(_currentChildObject, eventData, ExecuteEvents.pointerDownHandler);
        }
        
        public void SetDraggedObject(GameObject obj)
        {
            _currentChildObject = obj;
            _lockScroll = true;
        }
    }
}