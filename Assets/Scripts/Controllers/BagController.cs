using System.Collections.Generic;
using Components;
using Models;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class BagController : MonoBehaviour
    {
        public UnityEvent<int> OnBlockCountChange = new UnityEvent<int>();
        
        [SerializeField] private int maxBlockCount = 40;
        [SerializeField] private int collectingLayer = 8;
        [SerializeField] private BarnController barnController;

        [Header("Blocks properties")]
        [SerializeField] private Vector3 bagOffset = new Vector3(0, 0, -0.2f);
        [SerializeField] private Vector3 blockOffset = new Vector3(0, 0.3f, 0);
        [SerializeField] private Vector3 blockRotation = new Vector3(0, 90f, 0);
        
        [Header("Sale properties")]
        [SerializeField] private int sellingLayer = 7;
        [SerializeField] private float saleCooldown = 1f;
        [SerializeField] private Vector3 salePosition = Vector3.zero;
        [SerializeField] private AnimationCurve saleAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private BagModel _model;
        private Rigidbody _rigidbody;
        private readonly Stack<Collectable> _blocks = new Stack<Collectable>();

        private void Awake()
        {
            _model = new BagModel();

            _model.OnCollect += Collected;
            _model.OnSell += Sold;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == collectingLayer)
            {
                _model.Collect(maxBlockCount, other.GetComponent<Collectable>());
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == sellingLayer)
            {
                _model.Sell(saleCooldown);
            }
        }

        private void Collected(Collectable block)
        {
            Collectable parent = _blocks.Count > 0 ? _blocks.Peek() : null;
            _blocks.Push(block);
            
            if (parent)
            {
                block.transform.position = parent.transform.position + blockOffset;
                block.transform.rotation = parent.transform.rotation;
                block.SetJointParent(parent);
            }
            else
            {
                block.transform.position = transform.TransformPoint(transform.localPosition + bagOffset);
                block.transform.rotation = transform.rotation * Quaternion.Euler(blockRotation);
                block.SetJointParent(_rigidbody, bagOffset);
            }
            
            OnBlockCountChange.Invoke(_model.CurrentBlockCount);
        }

        private void Sold()
        {
            Collectable block = _blocks.Pop();
            block.Move(
                salePosition,
                saleCooldown,
                () =>
                {
                    barnController.BuyBlock(block);
                },
                saleAnimationCurve
            );
            OnBlockCountChange.Invoke(_model.CurrentBlockCount);
        }

        private void OnDestroy()
        {
            _model.OnCollect -= Collected;
            _model.OnSell -= Sold;
        }
    }
}