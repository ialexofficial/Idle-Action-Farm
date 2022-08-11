using System;
using UnityEngine;

namespace Models
{
    public class PlayerModel
    {
        private float _previousSlashTime;
        
        public event Action<Vector3> OnMove;
        public event Action OnSlash;
        

        public void Move(float vertical, float horizontal)
        {
            if (vertical == 0 && horizontal == 0)
                return;
            
            Vector3 translation = new Vector3(horizontal, 0, vertical);

            OnMove?.Invoke(translation.normalized);
        }

        public void Slash(float slashCooldown)
        {
            float currentTime = Time.time;
            
            if (_previousSlashTime == 0 || currentTime - _previousSlashTime >= slashCooldown)
            {
                _previousSlashTime = currentTime;
                OnSlash?.Invoke();
            }
        }
    }
}