using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Extensions
{
    public static class TimeExtensions
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitForSeconds = new();

        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (WaitForSeconds.TryGetValue(seconds, out var forSeconds)) return forSeconds;

            var waitForSeconds = new WaitForSeconds(seconds);
            WaitForSeconds.Add(seconds, waitForSeconds);
            return WaitForSeconds[seconds];
        }

        public static void Invoke(this MonoBehaviour mb, Action delayedAction, float delay)
        {
            mb.StartCoroutine(invokeRoutine(delayedAction, delay));

            static IEnumerator invokeRoutine(Action delayedAction, float delay)
            {
                yield return GetWaitForSeconds(delay);
                delayedAction();
            }
        }
    }
}