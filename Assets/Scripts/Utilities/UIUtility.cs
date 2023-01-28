using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Utility
{
    public static class UIUtility
    {
        /// <summary>
        /// Gets a position within screen size from world position.
        /// </summary>
        /// <param name="camera">Camera to use.</param>
        /// <param name="worldPosition">World position to transform into screen position.</param>
        /// <param name="panelSize">Size of the panel to display.</param>
        /// <returns>Screen position in pixels that is on a world position.
        /// If the resulting position is not within screen bounds it will "point" towards the world position.</returns>
        public static Vector3 GetPanelPositionFromWorldPosition(Camera camera, Vector3 worldPosition, Vector2 panelSize)
        {
            panelSize = panelSize * 0.55f; // Half size + bounds.
            Vector3 result = camera.WorldToScreenPoint(worldPosition) + Vector3.up * panelSize.y;
            Vector2 safeSize = new Vector2(Screen.width - panelSize.x, Screen.height - panelSize.y);

            if (result.y > safeSize.y) result.y = safeSize.y;
            if (result.y < panelSize.y) result.y = panelSize.y;
            if (result.x > safeSize.x) result.x = safeSize.x;
            if (result.x < panelSize.x) result.x = panelSize.x;

            return result;
        }

        /// <summary>
        /// Returns the size of a UI panel.
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        public static Vector2 GetPanelSize(RectTransform panel)
        {
            return new Vector2(panel.rect.width, panel.rect.height);
        }

        /// <summary>
        /// Returns the Description of an object using the Description attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetDescription<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }
}
