using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class CollectablePuller : MonoBehaviour
    {
        [SerializeField] private int startCollectableCount = 40;
        [SerializeField] private Collectable collectablePrefab;

        private readonly Queue<Collectable> _collectablePull = new Queue<Collectable>();

        protected int CollectableCount => _collectablePull.Count;

        public void ReturnCollectableToPull(Collectable collectable)
        {
            collectable.SetActive(false);
            _collectablePull.Enqueue(collectable);
        }
        
        private void Start()
        {
            for (int i = 0; i < startCollectableCount; i++)
            {
                AddBlockToQueue();
            }
        }
        
        protected void AddBlockToQueue()
        {
            Collectable collectable = Instantiate(collectablePrefab.gameObject).GetComponent<Collectable>();
            collectable.SetActive(false);
            collectable.CollectablePuller = this;
            
            _collectablePull.Enqueue(collectable);
        }

        protected Collectable DequeueCollectable() =>
            _collectablePull.Dequeue();
    }
}