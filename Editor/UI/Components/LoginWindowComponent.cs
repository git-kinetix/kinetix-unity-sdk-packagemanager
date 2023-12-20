#if UNITY_VSA
using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.VSAttribution.Kinetix;

namespace Kinetix.Editor
{
    public class LoginWindowComponent : IEditorWindowComponent
    {
        private readonly GUIStyle webButtonStyle;
        
        private string mail;
        private string password;
        private bool   isLogging;

        public LoginWindowComponent()
        {
            webButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize    = 12,
                fixedWidth  = 149,
                fixedHeight = 30,
                padding     = new RectOffset(5, 5, 5, 5)
            };
        }
        
        public bool HasLoggedIn()
        {
            return EditorPrefs.HasKey(VSOperation.LOGIN_PREF_KEY);
        }
        
        public void Draw(Rect position)
        {
            // Mail
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Mail ");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            mail     = EditorGUILayout.TextField(mail);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            
            
            GUILayout.Space(15);
            
            // Password
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Password ");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            password = EditorGUILayout.PasswordField(password);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();

            GUILayout.Space(15);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Login", webButtonStyle))
            {
                if (!isLogging)
                    ProcessLogin();
            }
            
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(15);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create Account", webButtonStyle))
            {
                Application.OpenURL("https://portal.kinetix.tech/signup");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(15);
        }

        private async void ProcessLogin()
        {
            isLogging = true;
            try
            {
                await VSOperation.SendRequest(mail, password);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Connection failed with error : " + e.Message); 
            }

            isLogging = false;
        }
    }
}
#endif
