using System.Collections;
using UnityEngine;

namespace Source.Code
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);

    }
}