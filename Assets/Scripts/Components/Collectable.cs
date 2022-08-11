using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(ConfigurableJoint))]
    public class Collectable : Movable
    {
        [SerializeField] private int cost;

        private ConfigurableJoint _joint;
        private Rigidbody _rigidbody;
        private bool _active;
        private Rigidbody _parent;
        private Vector3 _defaultConnectedAnchor;

        public int Cost => cost;
        public CollectablePuller CollectablePuller { get; set; }

        public void SetActive(bool isActive)
        {
            _active = isActive;
            
            if (_rigidbody && _joint)
            {
                gameObject.SetActive(_active);
                
                if(_active) {
                    _rigidbody.WakeUp();
                }
            }
        }

        public void SetJointParent(Collectable parent, Vector3? connectedAnchor = null)
        {
            SetJointParent(parent._rigidbody, connectedAnchor);
        }

        public void SetJointParent(Rigidbody parent, Vector3? connectedAnchor = null)
        {
            _parent = parent;

            if (_joint)
            {
                _joint.connectedAnchor = _defaultConnectedAnchor + (connectedAnchor ?? Vector3.zero);

                _joint.connectedBody = _parent;
            }
        }

        public void ReturnToPull()
        {
            CollectablePuller.ReturnCollectableToPull(this);
        }

        private void Start()
        {
            _joint = GetComponent<ConfigurableJoint>();
            _rigidbody = GetComponent<Rigidbody>();
            _defaultConnectedAnchor = _joint.connectedAnchor;

            gameObject.SetActive(_active);
            
            if(_parent)
                SetJointParent(_parent);
        }
    }
}