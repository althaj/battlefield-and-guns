using System.Collections;
using System.Collections.Generic;
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
    }
}
