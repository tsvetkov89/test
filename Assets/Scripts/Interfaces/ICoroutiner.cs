using System.Collections;
using UnityEngine;

namespace Test.Interfaces
{
    public interface ICoroutiner
    {
        Coroutine StartCoroutine(IEnumerator routine);
        void StopCoroutine(IEnumerator routine);
        void StopCoroutine(string routine);
    }
}