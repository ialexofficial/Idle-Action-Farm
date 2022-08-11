using System;
using Components;

namespace Models
{
    public class BarnModel
    {
        private int _money;

        public int Money => _money;

        public event Action<Collectable> OnBuy;

        public void Buy(Collectable block)
        {
            _money += block.Cost;

            OnBuy?.Invoke(block);
        }
    }
}