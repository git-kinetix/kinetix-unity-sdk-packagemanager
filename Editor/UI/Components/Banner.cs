using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kinetix.Editor
{
    public class Banner: IEditorWindowComponent
    {
        private const int BANNER_WIDTH  = 460;
        private const int BANNER_HEIGHT = 113;

        private const string BANNER_SEARCH_FILTER = "t:Texture KinetixBanner";

        private readonly Texture2D banner;
        
        public Banner()
        {
            string assetGuid = AssetDatabase.FindAssets(BANNER_SEARCH_FILTER).FirstOrDefault();
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

            if (assetPath == null)
                return;
            
            banner = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        }

        public void Draw(Rect position)
        {
            var rect = new Rect((position.size.x - BANNER_WIDTH) / 2, 0, BANNER_WIDTH, BANNER_HEIGHT);
            GUI.DrawTexture(rect, banner);
            GUILayout.Space(128);
        }
    }
}
