using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ZTestClass))]
public class TestSceneView : Editor
{
    public GUIContent New;
    public GUIContent Save;
    //public GUIContent InspectInfo;
    //public GUIContent ToolSetting;
    //public GUIContent DisplayWindow;

    public class DataSub
    {
        public ZTestClass.Data src;
        public SerializedProperty prop;
        public bool folded;

        public DataSub(ZTestClass.Data src)
        {
            this.src = src;
        }
    }

    static string LastWindowPosXKey = "TFA_LastWindowPosX";
    static string LastWindowPosYKey = "TFA_LastWindowPosY";

    static Vector2 FixedSize = new Vector2(300, 350);

    ZTestClass Target;

    Rect rtWindow;

    public List<DataSub> allData = new List<DataSub>();

    Texture2D captureTex;
    GUIStyle windowStyle;
    bool doCapture = true;

    Editor defaultEditor;

    SerializedObject myObj;
    SerializedProperty prop;

    bool isDirty = false;

    Vector2 scrollPos;

    void Init()
    {
        New = EditorGUIUtility.TrTextContentWithIcon("NEW", "Grid 새로 생성", "Grid Icon");
        Save = EditorGUIUtility.TrTextContentWithIcon("저장", "원하는 형태로 저장가능", "SaveAs");

        myObj = new SerializedObject(target);
        prop = myObj.FindProperty($"{nameof(ZTestClass.allData)}");
    }

    private void OnEnable()
    {
        Init();

        Target = base.target as ZTestClass;

        if (EditorPrefs.HasKey(LastWindowPosXKey))
        {
            rtWindow.x = EditorPrefs.GetFloat(LastWindowPosXKey);
            rtWindow.y = EditorPrefs.GetFloat(LastWindowPosYKey);
            rtWindow.width = FixedSize.x;
            rtWindow.height = FixedSize.y;
        }
        else
        {
            rtWindow = new Rect(20, 20, FixedSize.x, FixedSize.y);
        }

        ResetData();

        doCapture = true;
    }

    void ResetData()
    {
        allData.Clear();

        allData.Capacity = Target.allData.Count;
        for (int i = 0; i < Target.allData.Count; i++)
        {
            allData.Add(new DataSub(Target.allData[i]));
        }
    }

    private void OnDisable()
    {
        EditorPrefs.SetFloat(LastWindowPosXKey, rtWindow.x);
        EditorPrefs.SetFloat(LastWindowPosYKey, rtWindow.y);
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        {
            base.OnInspectorGUI();
        }
        if (EditorGUI.EndChangeCheck())
        {
            ResetData();
        }
    }

    private void OnSceneGUI()
    {
        if (PrefabStageUtility.GetCurrentPrefabStage() == null)
        {
            return;
        }

        if (doCapture)
        {
            doCapture = false;
            windowStyle = new GUIStyle(GUI.skin.window);
            windowStyle.normal.textColor = Color.white;
            windowStyle.onNormal = new GUIStyleState() { background = GUI.skin.window.normal.background, textColor = Color.white };
            windowStyle.fontSize = 15;
            windowStyle.fontStyle = FontStyle.Bold;
        }

        if (Target == null)
        {
            return;
        }

        Handles.BeginGUI();
        {
            if (Event.current.type != EventType.Repaint)
            {
                int controlId = GUIUtility.GetControlID(FocusType.Passive);
                rtWindow = GUILayout.Window(controlId, rtWindow, OnDraw, "Fx 정보", windowStyle, GUILayout.Width(FixedSize.x), GUILayout.Height(FixedSize.y));
            }
        }
        Handles.EndGUI();
    }

    private void OnDraw(int windowId)
    {
        GUILayout.Space(5);
        DrawToolBar();
        GUILayout.Space(5);

        //GUILayout.BeginHorizontal();
        //GUILayout.FlexibleSpace();
        //GUILayout.EndHorizontal();

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        {
            for (int i = 0; i < allData.Count; i++)
            {
                DrawElement(allData[i], i);
            }
        }
        GUILayout.EndScrollView();

        var sceneView = SceneView.lastActiveSceneView;
        GUI.DragWindow(new Rect(0, 0, sceneView.position.width, sceneView.position.height));
    }

    void DrawElement(DataSub data, int idx)
    {
        GUILayout.BeginVertical();
        {
            data.folded = EditorGUILayout.Foldout(data.folded, new GUIContent(data.src.name), true);

            if (data.folded)
            {
                EditorGUI.indentLevel++;
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        EditorGUILayout.PropertyField(this.prop.GetArrayElementAtIndex(idx));
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        isDirty = true;
                    }

                    if (GUILayout.Button(""))
                    {
                    }
                }
                EditorGUI.indentLevel--;
            }

            EditorHelper.Draw_Separator();
        }
        GUILayout.EndVertical();
    }

    Rect mSaveMenuRect;
    Rect mViewStateMenuRect;
    void DrawToolBar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);// EditorStyles.toolbarTextField);
        {
            if (GUILayout.Button(New, EditorStyles.toolbarButton, GUILayout.Width(100)))
            {
            }

            GUILayout.FlexibleSpace();

            mSaveMenuRect = GUILayoutUtility.GetRect(Save, EditorStyles.toolbarButton, GUILayout.Width(100f));
            GUI.enabled = isDirty;
            if (GUI.Button(mSaveMenuRect, Save, EditorStyles.toolbarButton))
            {

            }
            GUI.enabled = true;

            //mViewStateMenuRect = GUILayoutUtility.GetRect(DisplayWindow, EditorStyles.toolbarButton, GUILayout.Width(100f));
            //if (GUI.Button(mViewStateMenuRect, DisplayWindow, EditorStyles.toolbarButton))
            //{

            //}

            //GUILayout.FlexibleSpace();

            //bool oldEnabled = GUI.enabled;
            //GUI.enabled = false;
            //if (GUILayout.Button(InspectInfo, EditorStyles.toolbarButton))
            //{
            //}
            //GUI.enabled = oldEnabled;

            //if (GUILayout.Button(ToolSetting, EditorStyles.toolbarButton))
            //{
            //}
        }
        EditorGUILayout.EndHorizontal();
    }
}
