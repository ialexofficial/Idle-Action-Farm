using System.Collections;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class Slicable : MonoBehaviour
    {
        [SerializeField] private float reloadTime = 10f;
        [SerializeField] private GameObject slicablePart;
        [SerializeField] private Vector3 collectablePlaceOffset = new Vector3(0, 1f, 0);

        private Rigidbody _rigidbody;
        
        public bool IsReady { get; private set; }

        public void Slice(Collectable collectable)
        {
            IsReady = false;
            slicablePart.SetActive(false);
            
            collectable.transform.position = transform.position + collectablePlaceOffset;
            collectable.transform.rotation = transform.rotation;
            collectable.SetJointParent(_rigidbody);

            StartCoroutine(Reload());
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            IsReady = true;
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(reloadTime);

            slicablePart.SetActive(true);
            IsReady = true;
        }
    }
}