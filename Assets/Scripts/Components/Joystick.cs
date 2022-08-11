using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Components
{
    public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public UnityEvent<float, float> OnMove = new UnityEvent<float, float>();
        
        [SerializeField] private GameObject joystick;
        [SerializeField] private RectTransform joystickMovablePart;
        [SerializeField] private RectTransform joystickSpawnPlace;
        [SerializeField] private Vector2 joystickRange = new Vector2(1, 1);

        private Vector2 _dragStartPosition;
        private Vector2 _delta;
        private bool _active;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragStartPosition = eventData.pointerCurrentRaycast.screenPosition;
            
            Vector2 spawnPlace = _dragStartPosition - (Vector2) joystickSpawnPlace.position;
            if (
                Mathf.Abs(spawnPlace.x) < joystickSpawnPlace.rect.width / 2 &&
                Mathf.Abs(spawnPlace.y) < joystickSpawnPlace.rect.height / 2
            )
            {
                _active = true;
            }

            if (!_active)
                return;
            
            joystickMovablePart.position = Vector3.zero;
            joystick.transform.position = _dragStartPosition;
            joystick.SetActive(_active);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_active)
                return;
            
            _delta = eventData.pointerCurrentRaycast.screenPosition - _dragStartPosition;

            if (Mathf.Abs(_delta.x) > joystickRange.x)
                _delta.x = Mathf.Sign(_delta.x) * joystickRange.x;
            
            if (Mathf.Abs(_delta.y) > joystickRange.y)
                _delta.y = Mathf.Sign(_delta.y) * joystickRange.y;

            joystickMovablePart.anchoredPosition = _delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _active = false;
            
            _delta = Vector2.zero;
            joystick.SetActive(_active);
        }

        private void Start()
        {
            joystick.SetActive(_active);
        }

        private void FixedUpdate()
        {
            OnMove.Invoke(_delta.x / joystickRange.x, _delta.y / joystickRange.y);
        }
    }
}
