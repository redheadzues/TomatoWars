using System;
using System.Collections;
using UnityEngine;

namespace Source.Code.IdleNumbers
{
    public static class IdleNumberTween
    {
        public static Coroutine IdleTweenTo(this MonoBehaviour mono, IdleNumber from, IdleNumber to, float duration, Action<IdleNumber> onUpdate)
        {
            return mono.StartCoroutine(TweenRoutine(from, to, duration, onUpdate));
        }

        private static IEnumerator TweenRoutine(IdleNumber from, IdleNumber to, float duration, Action<IdleNumber> onUpdate)
        {
            float time = 0f;

            while (time < duration)
            {
                float t = time / duration;

                var interpolated = Lerp(from, to, t);
                onUpdate(interpolated);

                time += Time.deltaTime;
                yield return null;
            }

            onUpdate(to); 
        }

        private static IdleNumber Lerp(IdleNumber a, IdleNumber b, float t)
        {
            int targetDegree = Math.Max(a.Degree, b.Degree);

            double aNormalizedValue = a.Value * Math.Pow(10, a.Degree - targetDegree);
            double bNormalizedValue = b.Value * Math.Pow(10, b.Degree - targetDegree);

            double lerpedValue = Mathf.Lerp((float)aNormalizedValue, (float)bNormalizedValue, t);

            IdleNumber result = new IdleNumber((float)lerpedValue);

            result = new IdleNumber(result.Value * Math.Pow(10, targetDegree));

            return result;
        }
    }
}