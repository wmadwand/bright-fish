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

namespace Terminus.Extensions
{
    public static class ActionExtensions
    {
        /// <summary>
        ///     Осуществляет вызов делегата <paramref name="action" />, если передаваемый делегат не является null
        /// </summary>
        /// <param name="action">Делегат, для которого необходимо осуществить вызов</param>
        public static void Call(this Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        /// <summary>
        ///     Осуществляет вызов делегата <paramref name="action" /> с агрументом <paramref name="arg" />, если передаваемый
        ///     делегат не является null
        /// </summary>
        /// <param name="action">Делегат, для которого необходимо осуществить вызов</param>
        /// <param name="arg">Аргумент для вызова <paramref name="action" /></param>
        public static void Call<T>(this Action<T> action, T arg)
        {
            if (action != null)
            {
                action(arg);
            }
        }

        /// <summary>
        ///     Осуществляет вызов делегата <paramref name="action" /> с агрументами <paramref name="arg1" /> и
        ///     <paramref name="arg2" />, если передаваемый делегат не является null
        /// </summary>
        /// <param name="action">Делегат, для которого необходимо осуществить вызов</param>
        /// <param name="arg1">Аргумент для вызова <paramref name="action" /></param>
        /// <param name="arg2">Аргумент для вызова <paramref name="action" /></param>
        public static void Call<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (action != null)
            {
                action(arg1, arg2);
            }
        }

        /// <summary>
        ///     Осуществляет вызов делегата <paramref name="action" /> с агрументами <paramref name="arg1" />,
        ///     <paramref name="arg2" /> и <paramref name="arg3" />, если передаваемый делегат не является null
        /// </summary>
        /// <param name="action">Делегат, для которого необходимо осуществить вызов</param>
        public static void Call<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (action != null)
            {
                action(arg1, arg2, arg3);
            }
        }

        /// <summary>
        ///     Осуществляет вызов делегата <paramref name="action" /> с агрументами <paramref name="arg1" />,
        ///     <paramref name="arg2" />, <paramref name="arg3" /> и <paramref name="arg4" />, если передаваемый делегат не
        ///     является null
        /// </summary>
        /// <param name="action">Делегат, для которого необходимо осуществить вызов</param>
        public static void Call<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (action != null)
            {
                action(arg1, arg2, arg3, arg4);
            }
        }

        public static T Call<T>(this Func<T> func, T @default = default(T))
        {
            return func != null ? func() : @default;
        }

        public static T Call<T1, T>(this Func<T1, T> func, T1 arg1, T @default = default(T))
        {
            return func != null ? func(arg1) : @default;
        }

        public static T Call<T1, T2, T>(this Func<T1, T2, T> func, T1 arg1, T2 arg2, T @default = default(T))
        {
            return func != null ? func(arg1, arg2) : @default;
        }

        public static T Call<T1, T2, T3, T>(this Func<T1, T2, T3, T> func, T1 arg1, T2 arg2, T3 arg3,
            T @default = default(T))
        {
            return func != null ? func(arg1, arg2, arg3) : @default;
        }

        public static T Call<T1, T2, T3, T4, T>(this Func<T1, T2, T3, T4, T> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4,
            T @default = default(T))
        {
            return func != null ? func(arg1, arg2, arg3, arg4) : @default;
        }
    }
}