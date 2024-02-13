using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MBExtensions
{
    public static class MBExtensions
    {
        public static IEnumerator DelayedExecution(this UnityEngine.MonoBehaviour @this, Action Action, float waitForSecs)
        {
            yield return new WaitForSeconds(waitForSecs);
            Action();
        }
        public static IEnumerator DelayedExecution(this UnityEngine.MonoBehaviour @this, Action Action, YieldInstruction wait)
        {
            yield return wait;
            Action();
        }

        /*
        public static List<Coroutine> delayRoutines = new();
        public class CoroutineContainer
        {
            public Coroutine coroutine;
        }
        
        public static void DelEx(this UnityEngine.MonoBehaviour @this, in Action Action, float delay)
        {
            CoroutineContainer container = new();
            IEnumerator enumerator = DelayedExecutionRoutine(@this, Action, delay, container);
            container.coroutine = @this.StartCoroutine(enumerator);
            //coroutines.Add(@this.StartCoroutine(DelayedExecution(@this, Action, delay)));
        }
        
        private static IEnumerator DelayedExecutionRoutine(this UnityEngine.MonoBehaviour @this, Action Action, float delay, CoroutineContainer container)
        {
            //yield return new WaitForEndOfFrame();
            //Debug.Log(Time.timeAsDouble);
            //delayRoutines.Add(container.coroutine);

            yield return new WaitForSeconds(delay);
            //Debug.Log(Time.timeAsDouble);
            Action();
            //delayRoutines.Remove(container.coroutine);
        }
        */

    }
}
