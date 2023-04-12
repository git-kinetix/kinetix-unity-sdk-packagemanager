using Kinetix.PackageManager.Editor;
using UnityEditor;
using UnityEngine;

namespace Kinetix.Editor
{
    public class KinetixPackageManagerEditorWindow : EditorWindowBase
    {
        private          GUIStyle        titleStyle;
        private          GUIStyle        descriptionStyle;
        private          GUIStyle        buttonStyle;

        /// <summary>
        /// Called each editor render frame
        /// </summary>
        private void OnGUI()
        {
            LoadStyles();
            DrawContent(DrawContent, true);
        }

        /// <summary>
        /// Display the window editor
        /// </summary>
        [MenuItem("Kinetix/Package Manager", priority = 0)]
        public static void ShowWindowMenu()
        {
            KinetixPackageManagerEditorWindow window = (KinetixPackageManagerEditorWindow)GetWindow(typeof(KinetixPackageManagerEditorWindow));
            window.titleContent = new GUIContent("Kinetix Package Manager");
            window.Show();
        }

        /// <summary>
        /// Load the different styles of the Editor UI
        /// </summary>
        private void LoadStyles()
        {
            titleStyle ??= new GUIStyle(GUI.skin.label)
            {
                fontSize   = 14,
                margin     = new RectOffset(5, 0, 0, 0),
                fontStyle  = FontStyle.Bold,
                richText   = true,
                wordWrap   = true,
                alignment  = TextAnchor.MiddleCenter,
                fixedWidth = 452,
                normal =
                {
                    textColor = new Color(0.85f, 0.85f, 0.85f, 1.0f)
                }
            };

            descriptionStyle ??= new GUIStyle(GUI.skin.label)
            {
                fontSize   = 12,
                margin     = new RectOffset(5, 0, 0, 0),
                richText   = true,
                wordWrap   = true,
                alignment  = TextAnchor.MiddleCenter,
                fixedWidth = 452,
                normal =
                {
                    textColor = new Color(0.7f, 0.7f, 0.7f, 1.0f)
                }
            };

            buttonStyle ??= new GUIStyle(GUI.skin.button)
            {
                fontStyle   = FontStyle.Bold,
                fontSize    = 12,
                padding     = new RectOffset(5, 5, 5, 5),
                fixedHeight = 30,
                fixedWidth  = 225
            };
        }

        /// <summary>
        /// Draw different content elements of this window
        /// </summary>
        private void DrawContent()
        {
            DrawPackageManagerUpdates();
            DrawCoreBundle(KinetixModules.CoreBundleWeb3, KinetixModules.CoreBundleWeb2);
            DrawOrCondition();
            DrawCoreBundle(KinetixModules.CoreBundleWeb2, KinetixModules.CoreBundleWeb3);
        }

        /// <summary>
        /// Draw the Package Manager Updates Part
        /// </summary>
        private void DrawPackageManagerUpdates()
        {
            Vertical(() =>
            {
                GUILayout.Label("Package Manager", HeadingStyle);
                
                bool hasNewVersionPackageManager = KinetixPackageManagerUpdater.HasNewVersionPackageManager != null && (bool)KinetixPackageManagerUpdater.HasNewVersionPackageManager;

                Horizontal(() =>
                {
                    if (hasNewVersionPackageManager)
                    {
                        Vertical(() =>
                        {
                            GUILayout.Label("New update(s) available!", titleStyle);
                            GUILayout.Label("Update the Pack Manager to discover the new modules updates available.", descriptionStyle);
                        });
                    }
                    else
                    {
                        GUILayout.Label("Check if there is new updates of the Kinetix Package Manager", descriptionStyle);
                    }
                });
               
                GUILayout.Space(5);
                
                Horizontal(() =>
                {
                    GUILayout.FlexibleSpace();

                    if (hasNewVersionPackageManager)
                    {
                        GUILayout.FlexibleSpace();
                        
                        if (GUILayout.Button("Update Package Manager", buttonStyle))
                        {
                            KinetixPackageManagerUpdater.UpdatePackageManager();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    else
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Check For Updates", buttonStyle))
                        {
                            KinetixPackageManagerUpdater.CheckNewPackageManager();
                        }
                        GUILayout.FlexibleSpace();
                    }

                    GUILayout.FlexibleSpace();
                });
            }, true);
        }

        private void DrawCoreBundle(KinetixBundleInfo _KinetixBundleInfo, KinetixBundleInfo _KinetixBundleInfoToRemove)
        {
            Vertical(() =>
            {
                GUILayout.Label(_KinetixBundleInfo.Name, HeadingStyle);

                if (!KinetixModulesUpdater.IsBundleRegistered(_KinetixBundleInfo))
                {
                    Vertical(() =>
                    {
                        Horizontal(() =>
                        {
                            GUILayout.FlexibleSpace();
                            DrawInstallCoreBundle(_KinetixBundleInfo, _KinetixBundleInfoToRemove);
                            GUILayout.FlexibleSpace();
                        });
                    });
                    return;
                }
                
                if (KinetixModulesUpdater.IsBundleUpToDate(_KinetixBundleInfo))
                {
                    Vertical(() =>
                    {
                        GUILayout.Label("Up To Date", titleStyle);
                        Horizontal(() =>
                        {
                            GUILayout.FlexibleSpace();
                            DrawRemoveCoreBundle(_KinetixBundleInfo);
                            GUILayout.FlexibleSpace();
                        });
                    });
                }
                else
                {
                    Vertical(() =>
                    {
                        GUILayout.Label("New Update Available!", titleStyle);
                        Horizontal(() =>
                        {
                            GUILayout.FlexibleSpace();
                            DrawUpdateCoreBundle(_KinetixBundleInfo);
                            GUILayout.FlexibleSpace();
                        });
                        
                        Horizontal(() =>
                        {
                            GUILayout.FlexibleSpace();
                            DrawRemoveCoreBundle(_KinetixBundleInfo);
                            GUILayout.FlexibleSpace();
                        });
                    });
                }
            }, true);
            
        }

        private void DrawInstallCoreBundle(KinetixBundleInfo _KinetixBundleInfo, KinetixBundleInfo _KinetixBundleInfoToRemove)
        {
            if (GUILayout.Button("Install Core Bundle", buttonStyle))
            {
                if (KinetixModulesUpdater.IsBundleRegistered(_KinetixBundleInfoToRemove))
                {
                    KinetixModulesUpdater.RemoveModules(_KinetixBundleInfoToRemove.Modules, () =>
                    {
                        KinetixModulesUpdater.UpdateBundle(_KinetixBundleInfo, () =>
                        {
                            DrawContent(DrawContent, true);
                        });
                    });
                }
                else
                {
                    KinetixModulesUpdater.UpdateBundle(_KinetixBundleInfo, () =>
                    {
                        DrawContent(DrawContent, true);
                    });
                }
                               
            }
        }
        
        private void DrawRemoveCoreBundle(KinetixBundleInfo _KinetixBundleInfo)
        {
            if (GUILayout.Button("Remove Core Bundle", buttonStyle))
            {
                KinetixModulesUpdater.RemoveModules(_KinetixBundleInfo.Modules, () =>
                {
                    DrawContent(DrawContent, true);
                });
            }
        }

        private void DrawUpdateCoreBundle(KinetixBundleInfo _KinetixBundleInfo)
        {
            if (GUILayout.Button("Update Core Bundle", buttonStyle))
            {
                KinetixModulesUpdater.UpdateBundle(_KinetixBundleInfo, () =>
                {
                    DrawContent(DrawContent, true);
                });
            }
        }

        private void DrawOrCondition()
        {
            Horizontal(() =>
            {
                GUILayout.Label("OR", descriptionStyle);
            });
        }
    }
}
