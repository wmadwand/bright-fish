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

using Terminus.Extensions;

using UnityEngine;

namespace Terminus
{
    [ExecuteInEditMode]
    public sealed class ExecuteHostBehaviour : MonoBehaviour
    {
        private static ExecuteHostBehaviour _instance;
        private static bool IsDestroying;

        public static MonoBehaviour Instance
        {
            get
            {
                if (_instance == null)
                {
                    var gameObject = GameObject.Find("CoroutinesHost");
                    if (gameObject != null)
                    {
                        _instance = gameObject.GetOrAddComponent<ExecuteHostBehaviour>();
                    }

                    if (_instance == null)
                    {
                        _instance = new GameObject("CoroutinesHost").AddComponent<ExecuteHostBehaviour>();
                    }

                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(_instance.gameObject);
                    }
#if UNITY_EDITOR
                    _instance.gameObject.hideFlags = HideFlags.HideAndDontSave;
#endif
                }

                return _instance;
            }
        }

        private void OnDestroy()
        {
            IsDestroying = true;
        }
#if UNITY_EDITOR
        private void Update()
        {
            gameObject.hideFlags = HideFlags.HideAndDontSave;
        }
#endif
    }
}