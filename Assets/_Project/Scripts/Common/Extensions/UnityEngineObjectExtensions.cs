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

using System.Collections.Generic;

using UnityEngine;

namespace Terminus.Extensions
{
    public static class UnityEngineObjectExtensions
    {
        /// <summary>
        ///     Осуществляет корректное уничтожение объекта в зависимости от символа условной компиляции UNITY_EDITOR
        ///     Если UNITY_EDITOR присутствует, то выполняется <see cref="Object.DestroyImmediate(Object)" />.
        ///     Иначе выполняется <see cref="Object.Destroy(Object)" />
        /// </summary>
        /// <param name="obj">Объект, который необходимо уничтожить</param>
        public static void Destroy(this Object obj, bool isImmediateDestructionAllowed = true)
        {
            if (Application.isEditor && isImmediateDestructionAllowed)
            {
                Object.DestroyImmediate(obj);
            }
            else
            {
                Object.Destroy(obj);
            }
        }

        /// <summary>
        ///     Destroys all objects in <paramref name="objects" />.
        ///     Objects destroyed with <see cref="Object.DestroyImmediate(Object)" /> if
        ///     <paramref name="isImmediateDestructionAllowed" /> equals <code>true</code> and <code>UNITY_EDITOR</code> is
        ///     defined.
        ///     Otherwise <see cref="Object.Destroy(UnityEngine.Object)" /> used
        /// </summary>
        public static void DestroyAll<T>(this IEnumerable<T> objects, bool isImmediateDestructionAllowed = true)
            where T : Object
        {
            foreach (var obj in objects)
            {
                obj.Destroy(isImmediateDestructionAllowed);
            }
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Destroys asset <paramref name="obj" /> with <see cref="Object.DestroyImmediate(Object,bool)" />
        /// </summary>
        /// <param name="obj">Asset to destroy</param>
        public static void DestroyAsset(this Object obj)
        {
            Object.DestroyImmediate(obj, true);
        }

        /// <summary>
        ///     Destroys assets <paramref name="objects" /> with <see cref="Object.DestroyImmediate(Object,bool)" />
        /// </summary>
        /// <param name="objects">Assets to destroy</param>
        public static void DestroyAssets<T>(this IEnumerable<T> objects)
            where T : Object
        {
            foreach (var obj in objects)
            {
                DestroyAsset(obj);
            }
        }
#endif
    }
}