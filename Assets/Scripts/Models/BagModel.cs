using System;
using Components;
using UnityEngine;

namespace Models
{
    public class BagModel
    {
        private int _currentBlockCount;
        private float _previousSaleTime;

        public event Action<Collectable> OnCollect;
        public event Action OnSell;

        public int CurrentBlockCount => _currentBlockCount;

        public void Collect(int maxBlockCount, Collectable block)
        {
            if (_currentBlockCount >= maxBlockCount)
                return;

            _currentBlockCount++;
            OnCollect?.Invoke(block);
        }

        public void Sell(float saleCooldown)
        {
            if (_currentBlockCount == 0)
                return;

            float time = Time.time;

            if (_previousSaleTime != 0 && time - _previousSaleTime < saleCooldown)
                return;

            _previousSaleTime = time;
            _currentBlockCount--;
            OnSell?.Invoke();
        }
    }
}