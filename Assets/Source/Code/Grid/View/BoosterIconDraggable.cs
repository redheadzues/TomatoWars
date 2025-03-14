using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Code.Grid.View
{
    public class BoosterIconDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _selfRect;
        [SerializeField] private Collider2D _selfCollider;
        
        
        private Transform _parentTransform;
        private CellView _parentView;

        public event Action<BoosterIconDraggable, CellView> DragStarted;
        public event Action<BoosterIconDraggable> DragEnded;

        public event Action<Collider2D> TriggerEnter;
        public event Action<Collider2D> TriggerExit;

        public Collider2D Collider => _selfCollider;

        public void Init(CellView cellView)
        {
            _parentTransform = cellView.transform;
            _parentView = cellView;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(_parentTransform.root); 
            DragStarted?.Invoke(this, _parentView);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _selfRect.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(_parentTransform);
            transform.localPosition = Vector2.zero;
            DragEnded?.Invoke(this);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform == _parentTransform.transform)
                return;
            
            TriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.transform == _parentTransform.transform)
                return;
            
            TriggerExit?.Invoke(other);
            
        }
    }
}