using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Kinetix.Editor
{
    public class Footer: IEditorWindowComponent
    {
        private const string DOCS_URL      = "https://docs.kinetix.tech/";
        private const string CHANGELOG_URL = "https://docs.kinetix.tech/changelog";
        private const string DISCORD_URL   = "https://discord.gg/rGa6z4pGTW";

        private readonly GUIStyle webButtonStyle;
        
        public string EditorWindowName { get; set; }
        
        public Footer()
        {
            webButtonStyle             = new GUIStyle(GUI.skin.button)
            {
                fontSize    = 12,
                fixedWidth  = 149,
                fixedHeight = 30,
                padding     = new RectOffset(5, 5, 5, 5)
            };
        }

        public void Draw(Rect position)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(3);
            
            if (GUILayout.Button("Documentation", webButtonStyle))
            {
                Application.OpenURL(DOCS_URL);
            }
            
            if (GUILayout.Button("Changelog", webButtonStyle))
            {
                Application.OpenURL(CHANGELOG_URL);
            }
            
            if (GUILayout.Button("Discord", webButtonStyle))
            {
                Application.OpenURL(DISCORD_URL);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

}
