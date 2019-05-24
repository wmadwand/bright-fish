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
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Terminus.Extensions;

using UnityEditor;

using UnityEngine;

namespace Terminus
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class Execute
    {
        private static MonoBehaviour Host
        {
            get { return ExecuteHostBehaviour.Instance; }
        }

        public static Coroutine AfterFrames(int frames, Action action)
        {
            return Host.StartCoroutine(AfterFramesCoroutine(frames, action));
        }

        public static Coroutine NextFrame(Action action)
        {
            return AfterFrames(1, action);
        }

        public static Coroutine AfterSeconds(float seconds, Action action)
        {
            return Host.ExecuteAfterSeconds(seconds, action);
        }

        public static Coroutine OnFixedUpdate(Action action)
        {
            return Host.StartCoroutine(OnFixedUpdateCoroutine(action));
        }

        public static Coroutine OnEndOfFrame(Action action)
        {
            return Host.ExecuteOnEndOfFrame(action);
        }

        public static Coroutine Coroutine(IEnumerator coroutine)
        {
            return Host.StartCoroutine(coroutine);
        }

        public static Coroutine One(IEnumerator coroutine)
        {
            return Host.StartCoroutine(coroutine);
        }

        public static IEnumerator Many(params AsyncOperation[] asyncOperations)
        {
            while (!asyncOperations.All(o => o.isDone))
            {
                yield return null;
            }
        }

        public static IEnumerator Many(IEnumerable<IEnumerator> coroutines)
        {
            return Many(coroutines.ToArray());
        }
        
        public static IEnumerator Many(params IEnumerator[] coroutines)
        {
            var count = 0;
            for (var i = 0; i < coroutines.Length; i++)
            {
                var coroutine = coroutines[i];
                if (coroutine == null)
                {
                    continue;
                }

                count++;
                Coroutine(coroutine.Than(() => count--));
            }

            while (count > 0)
            {
                yield return null;
            }
        }
        
        public static void StopCoroutine(Coroutine coroutine)
        {
            Host.StopCoroutine(coroutine);
        }

        public static Coroutine Coroutine(IEnumerable enumerable)
        {
            return Coroutine(enumerable.GetEnumerator());
        }

        public static Coroutine AsyncOperation(AsyncOperation operation, Action onFinished)
        {
            return ExecuteAsyncOperation(Host, operation, onFinished);
        }

        public static Coroutine AsyncOperation<T>(T asyncOperation, Action<T> onFinished)
            where T : AsyncOperation
        {
            return ExecuteAsyncOperation(Host, asyncOperation, () => onFinished.Call(asyncOperation));
        }

        public static Coroutine ExecuteOnEndOfFrame(this MonoBehaviour host, Action action)
        {
            return host.StartCoroutine(OnEndOfFrameCoroutine(action));
        }

        public static Coroutine ExecuteAfterSeconds(this MonoBehaviour host, float seconds, Action action)
        {
            return host.StartCoroutine(AfterSecondsCoroutine(seconds, action));
        }

        public static Coroutine ExecuteInSeparateThread(this MonoBehaviour host, Action action)
        {
            return host.StartCoroutine(OnSeparateThreadCoroutine(action));
        }

        public static Coroutine ExecuteAsyncOperation(this MonoBehaviour host, AsyncOperation asyncOperation,
            Action onFinished)
        {
            return host.StartCoroutine(ExecuteAsyncOperationCoroutine(asyncOperation, onFinished));
        }

        private static IEnumerator ExecuteAsyncOperationCoroutine(AsyncOperation asyncOperation, Action onFinished)
        {
            yield return asyncOperation;
            onFinished.Call();
        }

        private static IEnumerator AfterFramesCoroutine(int frames, Action action)
        {
            if (frames < 0 || action == null)
            {
                yield break;
            }

            for (var i = 0; i < frames; i++)
            {
                yield return null;
            }

            action();
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

        private static IEnumerator OnFixedUpdateCoroutine(Action action)
        {
            if (action == null)
            {
                yield break;
            }

            yield return new WaitForFixedUpdate();

            action();
        }

        private static IEnumerator OnEndOfFrameCoroutine(Action action)
        {
            if (action == null)
            {
                yield break;
            }

            yield return new WaitForEndOfFrame();

            action();
        }

        public static IEnumerator OnSeparatePooledThreadCoroutine(Action action)
        {
            var isFinished = 0;

            if (!ThreadPool.QueueUserWorkItem(stateInfo =>
            {
                try
                {
                    action.Call();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                Interlocked.Increment(ref isFinished);
            }))
            {
                isFinished = 1;
            }

            while (isFinished == 0)
            {
                yield return null;
            }
        }

        public static IEnumerator OnSeparateThreadCoroutine(Action action)
        {
            var isFinished = 0;

            var thread = new Thread(() =>
            {
                try
                {
                    action.Call();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                Interlocked.Increment(ref isFinished);
            });
            thread.Start();

            while (isFinished == 0)
            {
                yield return null;
            }
        }

        public static Coroutine Begin(this IEnumerator enumerator)
        {
            return Coroutine(enumerator);
        }

#if UNITY_EDITOR
        public static void OnEditorUpdate(Action action)
        {
            NextEditorUpdate += action;
        }

        static Execute()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private static Action NextEditorUpdate;

        private static void OnEditorUpdate()
        {
            var update = NextEditorUpdate;
            if (update == null)
            {
                return;
            }

            NextEditorUpdate = null;
            update();
        }
#endif
    }
}