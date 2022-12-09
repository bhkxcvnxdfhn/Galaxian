using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineExtension
{
    public static void Stop(this Coroutine coroutine, MonoBehaviour mono)
    {
        mono.StopCoroutine(coroutine);
    }
}