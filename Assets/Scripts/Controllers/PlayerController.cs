using System;
using UnityEngine;
using Models;
using UnityEngine.Events;

namespace Controllers
{
    [RequireComponent(typeof(Animator), typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public UnityEvent<float> OnSlash = new UnityEvent<float>();
        
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float slashCooldown = 1f;

        private PlayerModel _model;
        private Animator _animator;
        private CharacterController _characterController;
        private bool _running;

        public void Slash()
        {
            _model.Slash(slashCooldown);
        }

        public void Move(float horizontal, float vertical)
        {
            if(horizontal == 0 && vertical == 0)
                _running = false;
            
            _model.Move(vertical, horizontal);
        }

        private void Awake()
        {
            _model = new PlayerModel();

            _model.OnMove += Moved;
            _model.OnSlash += Slashed;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
        }
        
# if UNITY_EDITOR
        private void FixedUpdate()
        {
            _model.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            
            if(Input.GetAxisRaw("Fire1") == 1)
                _model.Slash(slashCooldown);
        }
# endif

        private void LateUpdate()
        {
            _animator.SetBool("Running", _running);
        }

        private void Moved(Vector3 translation)
        {
            Vector3 previousPosition = transform.position;
            
            _characterController.Move(translation * moveSpeed * Time.fixedDeltaTime);
            _running = true;
            
            Vector3 delta = transform.position - previousPosition;
            
            if (delta.magnitude > 0.01)
            {
                Quaternion newRotation = Quaternion.LookRotation(delta.normalized);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.fixedDeltaTime * rotationSpeed);
            }
        }

        private void Slashed()
        {
            OnSlash.Invoke(slashCooldown);
            _animator.SetTrigger("Slash");
        }

        private void OnDestroy()
        {
            _model.OnMove -= Moved;
            _model.OnSlash -= Slashed;
        }
    }
}
