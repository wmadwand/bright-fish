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

using Terminus.Core.Helper;

using Random = UnityEngine.Random;

namespace Terminus.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        ///     Осуществляет проверку на нахождение индекса <paramref name="index" /> внутри границ списка <paramref name="list" />
        /// </summary>
        /// <param name="list">Список, для которого осуществляет проверка границ</param>
        /// <param name="index">Индекс, для которого осуществляется проверка границ</param>
        /// <returns>
        ///     Возвращает true если индекс <paramref name="index" /> находится внутри списка <paramref name="list" />, иначе
        ///     false
        /// </returns>
        public static bool IsValidIndex(this IList list, int index)
        {
            var count = list.Count;
            if (count == 0)
            {
                return false;
            }

            return index < count && index >= 0;
        }

        /// <summary>
        ///     Осуществляет проверку на нахождение индекса <paramref name="index" /> внутри границ списка <paramref name="list" />
        /// </summary>
        /// <param name="list">Список, для которого осуществляет проверка границ</param>
        /// <param name="index">Индекс, для которого осуществляется проверка границ</param>
        /// <returns>
        ///     Возвращает true если индекс <paramref name="index" /> находится внутри списка <paramref name="list" />, иначе
        ///     false
        /// </returns>
        public static bool IsValidIndex<T>(this IList<T> list, int index)
        {
            var count = list.Count;
            if (count == 0)
            {
                return false;
            }

            return index < count && index >= 0;
        }

        /// <summary>
        ///     Осуществляет проверку на нахождение индекса <paramref name="index" /> внутри границ списка <paramref name="list" />
        /// </summary>
        /// <param name="list">Список, для которого осуществляет проверка границ</param>
        /// <param name="index">Индекс, для которого осуществляется проверка границ</param>
        /// <returns>
        ///     Возвращает true если индекс <paramref name="index" /> находится внутри списка <paramref name="list" />, иначе
        ///     false
        /// </returns>
        public static bool IsValidIndex<T>(this List<T> list, int index)
        {
            return IsValidIndex((IList<T>) list, index);
        }

        public static int ClampIndex<T>(this IList<T> list, int index)
        {
            if (index < 0)
            {
                return 0;
            }

            if (index >= list.Count)
            {
                return list.Count - 1;
            }

            return index;
        }

        public static bool IsValidIndex<T>(this T[] array, int index)
        {
            return ((IList<T>) array).IsValidIndex(index);
        }

        public static bool ContainsIndex<T>(this T[] array, int index)
        {
            return ((IList<T>) array).IsValidIndex(index);
        }

        public static bool ContainsIndex<T>(this IList<T> array, int index)
        {
            return array.IsValidIndex(index);
        }

        /// <summary>
        ///     Находит следующий элемент в массиве, удовлетворяющий условиям. Проход идет по "кругу" с индекса следующего за
        ///     текущим элементом.
        /// </summary>
        public static TSource NextOrDefault<TSource>(this IList<TSource> list, TSource current,
            Func<TSource, bool> predicate) where TSource : class
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (current == null)
            {
                throw new ArgumentNullException("current");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var index = list.IndexOf(current);
            if (index >= 0)
            {
                var tempIndex = index < list.Count - 1 ? index + 1 : 0;
                while (tempIndex != index)
                {
                    if (list[tempIndex] != null && predicate(list[tempIndex]))
                    {
                        return list[tempIndex];
                    }

                    tempIndex = tempIndex < list.Count - 1 ? tempIndex + 1 : 0;
                }
            }

            return default(TSource);
        }

        public static T GetRandom<T>(this IList<T> list)
        {
            return list[GetRandomIndex(list)];
        }

        public static int GetRandomIndex<T>(this IList<T> list)
        {
            return Random.Range(0, list.Count);
        }

        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, Range<int>? range = null)
        {
            var r = range ?? new Range<int>(0, 1000);
            return enumerable.OrderBy(i => UnityEngine.Random.Range(r.Min, r.Max));
        }
    }
}