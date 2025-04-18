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
            double aValue = a.Value * Math.Pow(10, a.Degree);
            double bValue = b.Value * Math.Pow(10, b.Degree);
            double lerped = Mathf.Lerp((float)aValue, (float)bValue, t);

            return new IdleNumber(lerped);
        }
    }
}