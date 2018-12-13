using UnityEngine;
using System.Collections;
using System;


public class StaticCoroutine : MonoBehaviour
{
    public void Wait(float seconds, Action action)
    {
        StartCoroutine(_wait(seconds, action));
    }
    IEnumerator _wait(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    IEnumerator _WaitThenEnableCo(float seconds, MonoBehaviour mono)
    {
        yield return new WaitForSeconds(seconds);
        mono.enabled = false;
    }

    public void WaitThenEnableCo(float seconds, MonoBehaviour mono)
    {
        _WaitThenEnableCo(seconds, mono);
    }
}

public static class Utils
{
    public static int SignOr0(float a)
    {
        return ((a > 0) ? 1 : (a == 0) ? 0 : -1);
    }


    public static void WaitThenEnable(float seconds, MonoBehaviour mono)
    {
        StaticCoroutine c = new StaticCoroutine();
        c.WaitThenEnableCo(seconds, mono);
    }
}
