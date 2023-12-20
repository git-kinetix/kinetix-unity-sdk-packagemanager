using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kinetix.Editor
{
    public class EditorWindowBase : EditorWindow
    {
        protected GUIStyle HeadingStyle;
        protected GUIStyle ButtonStyle;
        protected GUIStyle Box;

        private readonly GUILayoutOption windowWidth = GUILayout.Width(452);

        private   Banner banner;
        private   Footer footer;

        private void LoadAssets()
        {
            banner ??= new Banner();
            
            footer ??= new Footer();

            HeadingStyle ??= new GUIStyle
            {
                fontSize = 14,
                richText = true,
                fontStyle = FontStyle.Bold,
                margin = new RectOffset(5, 0, 0, 8),
                normal =
                {
                    textColor = Color.white
                }
            };

            ButtonStyle ??= new GUIStyle(GUI.skin.button)
            {
                fontSize = 12,
                fixedWidth = 149,
                fixedHeight = 30,
                padding = new RectOffset(10, 10, 10, 10)
            };

            Box ??= new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(0, 0, 5, 5)
            };
        }

        protected void DrawContent(Action content, bool useFooter = true)
        {
            LoadAssets();

            Horizontal(() =>
            {
                GUILayout.FlexibleSpace();
                Vertical(() =>
                {
                    banner.Draw(position);
                    content?.Invoke();
                    
                    if (useFooter)
                    {
                        Vertical(() =>
                        {
                            GUILayout.Label("Support", HeadingStyle);
                            footer.Draw(position);
                        }, true);
                    }
                }, windowWidth);
                GUILayout.FlexibleSpace();
            });

            SetWindowSize();
        }
        
        private void SetWindowSize()
        {
            float height = GUILayoutUtility.GetLastRect().height;
            if (height > 1)
            {
                minSize = maxSize = new Vector2(460, height);
            }
        }
        
        protected void Vertical(Action content, bool isBox = false)
        {
            EditorGUILayout.BeginVertical(isBox ? Box : GUIStyle.none);
            content?.Invoke();
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();

        }

        protected void Vertical(Action content, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(options);
            content?.Invoke();
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();

        }

        protected void Horizontal(Action content, bool isBox = false)
        {
            EditorGUILayout.BeginHorizontal(isBox ? Box : GUIStyle.none);
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        protected void Horizontal(Action content, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }
    }
}
    