using System;
using System.Collections;
using UnityEngine;

namespace Components
{
    public class Movable : MonoBehaviour
    {
        private Coroutine _movingCoroutine;

        public void Move(Vector3 targetPosition, float time, Action callback, AnimationCurve animationCurve = null)
        {
            if(_movingCoroutine != null)
                StopCoroutine(_movingCoroutine);

            _movingCoroutine = StartCoroutine(StartMoving(
                targetPosition,
                time,
                callback,
                animationCurve ?? AnimationCurve.Linear(0, 0, 1, 1)
            ));
        }

        private IEnumerator StartMoving(Vector3 targetPosition, float time, Action callback, AnimationCurve animationCurve)
        {
            Vector3 startPosition = transform.position;
            Vector3 delta = targetPosition - startPosition;
            float passedTime = 0;

            while (passedTime < time)
            {
                passedTime += Time.deltaTime;
                transform.position = startPosition + delta * passedTime / time;
                
                yield return null;
            }

            callback.Invoke();
        }
    }
}