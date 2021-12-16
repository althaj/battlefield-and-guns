using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Utility
{
    public static class Utility
    {

        /// <summary>
        /// Get a component in the gameObject, or recursively in the parents.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public static T GetComponentInParents<T>(this MonoBehaviour behavior)
        {
            Transform currentTransform = behavior.transform;
            T result = behavior.GetComponent<T>();

            // We loop while the result is not found, or until we don't have any parents anymore.
            while (EqualityComparer<T>.Default.Equals(result, default(T)) && currentTransform.parent != null)
            {
                result = behavior.GetComponentInParent<T>();

                currentTransform = currentTransform.parent;
            }

            return result;
        }

        /// <summary>
        /// Get scaled number by level.
        /// </summary>
        /// <param name="initial">Initial number to scale.</param>
        /// <param name="level">Level of the number.</param>
        /// <param name="scaling">Scaling of the number.</param>
        /// <returns></returns>
        public static int ScaleNumber(this int initial, int level, float scaling)
        {
            return initial * (int)Mathf.Pow(scaling, level - 1);
        }

        /// <summary>
        /// Get scaled number by level.
        /// </summary>
        /// <param name="initial">Initial number to scale.</param>
        /// <param name="level">Level of the number.</param>
        /// <param name="scaling">Scaling of the number.</param>
        /// <returns></returns>
        public static float ScaleNumber(this float initial, int level, float scaling)
        {
            return initial * Mathf.Pow(scaling, level - 1);
        }

        /// <summary>
        /// Remove all children from a transform.
        /// </summary>
        /// <param name="transform">Parent to remove all children from.</param>
        public static void DestroyAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

    }
}
