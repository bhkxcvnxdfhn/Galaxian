using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CoroutineExtension
{
    public static void Stop(this Coroutine coroutine, MonoBehaviour mono, Action OnComplete = null)
    {
        mono.StopCoroutine(coroutine);
        OnComplete?.Invoke();
    }
}
