using Components;
using Models;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers
{
    public class BarnController : MonoBehaviour
    {
        public UnityEvent<int> OnMoneyChange = new UnityEvent<int>();

        private BarnModel _model;

        public void BuyBlock(Collectable block)
        {
            _model.Buy(block);
        }

        private void Awake()
        {
            _model = new BarnModel();

            _model.OnBuy += Bought;
        }

        private void Bought(Collectable block)
        {
            block.ReturnToPull();
            OnMoneyChange.Invoke(_model.Money);
        }

        private void OnDestroy()
        {
            _model.OnBuy -= Bought;
        }
    }
}