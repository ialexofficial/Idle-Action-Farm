using System;
using Components;
using UnityEngine;

namespace Models
{
    public class SickleModel
    {
        private float _slashStartTime;
        private float _slashTime;

        public event Action<Slicable> OnSlice;

        public void StartSlashing(float slashTime)
        {
            _slashStartTime = Time.time;
            _slashTime = slashTime;
        }

        public void Slice(Slicable slicable, AnimationCurve slicingTime)
        {
            float passedTime = Time.time - _slashStartTime;
            
            if (
                passedTime > _slashTime ||
                !slicable.IsReady ||
                slicingTime.Evaluate(passedTime / _slashTime) < 1
            )
                return;
            
            OnSlice?.Invoke(slicable);
        }
    }
}