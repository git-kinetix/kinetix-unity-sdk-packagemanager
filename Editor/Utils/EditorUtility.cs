using System;
using UnityEditor;

namespace Kinetix.Editor
{
    [InitializeOnLoad]
    public static class EditorUtility
    {
        public static Action OnEditorOpened;
        
        private const string CHECK_OPEN_EDITOR = "KINETIX_CHECK_OPEN_EDITOR";
        
        static EditorUtility()
        {
            // Delay one frame to let register
            EditorApplication.update += EditorUpdate;
        }
        
        private static void EditorUpdate()
        {
            if (DidOpenThisSession())
                return;
            
            OnEditorOpened?.Invoke();
            StoreOpenThisSession();
            EditorApplication.update -= EditorUpdate;
        }
        
        private static bool DidOpenThisSession()
        {
            return SessionState.GetBool(CHECK_OPEN_EDITOR, false);
        }

        private static void StoreOpenThisSession(bool _Opened = true)
        {
            SessionState.SetBool(CHECK_OPEN_EDITOR, _Opened);
        }
    }
}

