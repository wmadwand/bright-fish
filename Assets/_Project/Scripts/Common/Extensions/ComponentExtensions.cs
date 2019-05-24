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
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

namespace Terminus.Extensions
{
    public static class ComponentExtensions
    {
        public static void ReverseSiblingsOrder(this Transform transform)
        {
            var children = transform.GetChildTransforms().ToList();
            children.Reverse();

            for (var i = 0; i < children.Count; i++)
            {
                children[i].SetSiblingIndex(i);
            }
        }

        /// <summary>
        ///     Осуществляет получение компонента типа <typeparamref name="T" />, расположенного на объекте
        ///     <paramref name="gameObject" />.
        ///     Если компонент отсутствует на <paramref name="gameObject" />, он будет добавлен.
        /// </summary>
        /// <typeparam name="T">Тип получаемого компонента</typeparam>
        /// <param name="gameObject">Объект, на котором располагается компонент</param>
        /// <returns>Экземпляр типа <typeparamref name="T" />, расположенный на объекте <paramref name="gameObject" /> </returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            Assert.IsNotNull(gameObject);
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        /// <summary>
        ///     Осуществляет получение компонента типа <typeparamref name="T" />, расположенного на том же объекте, что и
        ///     <paramref name="component" />.
        /// </summary>
        /// <typeparam name="T">Тип получаемого компонента</typeparam>
        /// <param name="component">Объект, из размещения которого будет получен компонент</param>
        /// <returns>
        ///     Экземпляр типа <typeparamref name="T" />, расположенный на том же объекте, что и <paramref name="component" />
        /// </returns>
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            Assert.IsNotNull(component);
            return component.gameObject.GetOrAddComponent<T>();
        }

        /// <summary>
        ///     Осуществляет добавление компонента типа <typeparamref name="T" /> к объекту, на котором расположен компонент
        ///     <paramref name="component" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public static T AddComponent<T>(this Component component) where T : Component
        {
            Assert.IsNotNull(component);
            return component.gameObject.AddComponent<T>();
        }

        /// <summary>
        ///     Возвращает коллекцию <see cref="Transform" /> объектов потомков <see cref="Transform" />
        ///     <paramref name="component" /> без рекурсии.
        /// </summary>
        /// <param name="component">Компонент, <see cref="Transform" /> которого будет использован для поиска потомков</param>
        /// <returns>Коллекция потомков <see cref="Transform" /> <paramref name="component" /> без рекурсии</returns>
        public static IEnumerable<Transform> GetChildTransforms(this Component component)
        {
            Assert.IsNotNull(component);
            var transform = component.transform;
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                yield return transform.GetChild(i);
            }
        }

        public static T FindComponent<T>(this Component component)
        where T : class
        {
            return component.GetComponents<Component>().OfType<T>().FirstOrDefault();
        }

        /// <summary>
        ///     Осуществляет проверку наличия компонента типа <typeparamref name="T" /> на объекте, на котором расположен компонент
        ///     <paramref name="component" />
        /// </summary>
        /// <returns>
        ///     true если на объекте, на котором расположен компонент <paramref name="component" />, имеется компонент типа
        ///     <typeparamref name="T" />, иначе false
        /// </returns>
        public static bool HasComponent<T>(this Component component) where T : Component
        {
            Assert.IsNotNull(component);
            return component.GetComponent<T>() != null;
        }

        /// <summary>
        ///     Возвращает значение, указывающее содержит ли <paramref name="gameObject" /> единственный компонент
        ///     <paramref name="component" /> типа <typeparamref name="T" />.
        ///     Если <paramref name="component" /> не равен <code>null</code>, так же проверяет, что <paramref name="component" />
        ///     расположен на <paramref name="gameObject" />
        /// </summary>
        public static bool HasSingle<T>(this GameObject gameObject, T component = null) where T : Component
        {
            var components = gameObject.GetComponents<T>();
            if (components.Length == 1)
            {
                return component == null || component == components[0];
            }

            return false;
        }

        /// <summary>
        ///     Возвращает значение, указывающее является ли компонент <paramref name="component" /> единственным компонентом типа
        ///     <typeparamref name="T" /> на своем <see cref="GameObject" />
        /// </summary>
        public static bool IsSingle<T>(this T component) where T : Component
        {
            return component.gameObject.HasSingle(component);
        }

        /// <summary>
        ///     Осуществляет получение первого найденного компонента типа <typeparamref name="T" /> на объекте
        ///     <paramref name="component" /> или его родительских объектов.
        /// </summary>
        /// <typeparam name="T">Тип искомого компонента</typeparam>
        /// <param name="component">Компонент, с которого начнется поиск</param>
        /// <param name="includeInactive">Возвращать ли деактивированные компоненты</param>
        /// <returns></returns>
        public static T GetComponentInParent<T>(this Component component, bool includeInactive)
            where T : Component
        {
            return component.GetComponentsInParent<T>(includeInactive).FirstOrDefault();
        }

        /// <summary>
        ///     Возвращает все компоненты типа <typeparamref name="T" /> найденные на <paramref name="component" /> и его
        ///     родителях, включая неактивные компоненты.
        /// </summary>
        public static IEnumerable<T> GetAllComponentsInParent<T>(this Component component)
            where T : Component
        {
            return component.GetComponentsInParent(typeof(T), true).Cast<T>();
        }

        /// <summary>
        ///     Возвращает все компоненты типа <typeparamref name="T" /> найденные на <paramref name="gameObject" /> и его
        ///     потомках, включая неактивные компоненты.
        /// </summary>
        public static IEnumerable<T> GetAllComponentsInChildren<T>(this GameObject gameObject)
            where T : Component
        {
            return gameObject.GetComponentsInChildren(typeof(T), true).Cast<T>();
        }

        /// <summary>
        ///     Возвращает все компоненты типа <typeparamref name="T" /> найденные на <paramref name="component" /> и его потомках,
        ///     включая неактивные компоненты.
        /// </summary>
        public static IEnumerable<T> GetAllComponentsInChildren<T>(this Component component)
            where T : Component
        {
            return component.GetComponentsInChildren(typeof(T), true).Cast<T>();
        }

        public static void DestroyAllSiblingsExcept(this Transform parent, params Transform[] ignoredTransforms)
        {
            var i = 0;
            while (parent.childCount > ignoredTransforms.Length)
            {
                var child = parent.GetChild(i);
                if (ignoredTransforms.Any(t => t == child))
                {
                    i++;
                    continue;
                }

                child.SetParent(null);
                child.gameObject.Destroy();
            }
        }
    }
}