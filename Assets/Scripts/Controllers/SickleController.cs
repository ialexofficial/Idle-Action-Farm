using Components;
using Models;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Collider))]
    public class SickleController : CollectablePuller
    {
        [SerializeField] private int slicingLayer = 6;
        [SerializeField] private AnimationCurve slicingTime;
        
        private SickleModel _model;

        public void Slashed(float slashTime)
        {
            _model.StartSlashing(slashTime);
        }

        private void Awake()
        {
            _model = new SickleModel();

            _model.OnSlice += Sliced;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == slicingLayer)
            {
                _model.Slice(other.GetComponent<Slicable>(), slicingTime);
            }
        }

        private void Sliced(Slicable slicable)
        {
            if(CollectableCount == 0)
                AddBlockToQueue();
                
            Collectable block = DequeueCollectable();
            block.SetActive(true);
            slicable.Slice(block);
        }

        private void OnDestroy()
        {
            _model.OnSlice -= Sliced;
        }
    }
}
