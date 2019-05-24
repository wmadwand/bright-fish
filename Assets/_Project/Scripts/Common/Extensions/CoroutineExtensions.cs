// Mirosoft LLC CONFIDENTIAL
// Copyright (C) 2018 Mirosoft LLC
// All Rights Reserved.
//  
// NOTICE:  All information contained herein is, and remains the property
// of Mirosoft LLC and its suppliers, if any. The intellectual and 
// technical concepts contained herein are proprietary to Mirosoft LLC 
// and its suppliers and may be covered by U.S. and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from Mirosoft LLC.

using System;
using System.Collections;

using Terminus.Core.Helper;

using UnityEngine;

namespace Terminus.Extensions
{
    public static class CoroutineExtensions
    {
        public static Coroutine AfterSeconds(this MonoBehaviour monoBehaviour, float seconds, Action action)
        {
            return monoBehaviour.StartCoroutine(AfterSecondsCoroutine(seconds, action));
        }

        public static Coroutine AfterFrames(this MonoBehaviour monoBehaviour, int frames, Action action)
        {
            return monoBehaviour.StartCoroutine(AfterFramesCoroutine(frames, action));
        }

        public static Coroutine OnNextFrame(this MonoBehaviour monoBehaviour, Action action)
        {
            return AfterFrames(monoBehaviour, 1, action);
        }

        private static IEnumerator AfterSecondsCoroutine(float seconds, Action action)
        {
            if (action == null)
            {
                yield break;
            }

            yield return new WaitForSeconds(seconds);
            action();
        }

        private static IEnumerator AfterFramesCoroutine(int frames, Action action)
        {
            if (action == null)
            {
                yield break;
            }

            for (var i = 0; i < frames; i++)
            {
                yield return null;
            }

            action();
        }

        public static IEnumerator Than(this IEnumerator enumerator, Action onFinished)
        {
            yield return enumerator;
            onFinished();
        }

        public static IEnumerator Than(this IEnumerator enumerator, params IEnumerator[] nextEnumerators)
        {
            yield return enumerator;

            foreach (var nextEnumerator in nextEnumerators)
            {
                yield return nextEnumerator;
            }
        }

        public static IEnumerator AwaitCoroutine(this Message message)
        {
            var received = false;
            Action w = () => received = true;

            message.Receive += w;

            while (!received)
            {
                yield return null;
            }

            message.Receive -= w;
        } 
        
        public static IEnumerator AwaitCoroutine<T>(this Message<T> message, Action<T> onReceived = null)
        {
            var received = false;
            var t = default(T);
            Action<T> w = a =>
            {
                received = true;
                t = a;
            };

            message.Receive += w;

            while (!received)
            {
                yield return null;
            }

            message.Receive -= w;
            
            onReceived.Call(t);
        }

        public static IEnumerator AwaitCoroutine(this IMessagePromise promise)
        {
            while (!promise.IsReceived)
            {
                yield return null;
            }
        }

        public static IEnumerator ToCoroutine(this AsyncOperation asyncOperation)
        {
            yield return asyncOperation;
        }

        public static IEnumerator Await(Action<Action> actionWithCallback, Action onFinished)
        {
            var finished = false;

            actionWithCallback(() => finished = true);

            while (!finished)
            {
                yield return null;
            }

            onFinished.Call();
        }

        public static void ExeuteImmediate(this IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                
            }
        }
    }
}