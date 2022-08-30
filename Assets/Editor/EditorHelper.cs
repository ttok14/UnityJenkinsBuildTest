#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorHelper
{
    public static GUIStyle FontBoldRed = new GUIStyle(EditorStyles.helpBox)
    {
        normal = { textColor = new Color(1f, .15f, .15f) },
        fontStyle = FontStyle.Bold,
        fontSize = 12
    };

    private static GUIStyle mWarningLabel;
    public static GUIStyle WarningLabel
    {
        get
        {
            if (null == mWarningLabel)
            {
                mWarningLabel = new GUIStyle(EditorStyles.whiteLargeLabel);
                mWarningLabel.normal.textColor = Color.red;
                mWarningLabel.alignment = TextAnchor.MiddleLeft;
            }
            return mWarningLabel;
        }
    }

    private static GUIStyle mGreenLabel;
    public static GUIStyle GreenLabel
    {
        get
        {
            if (null == mGreenLabel)
            {
                mGreenLabel = new GUIStyle(EditorStyles.whiteLargeLabel);
                mGreenLabel.normal.textColor = Color.green;
                mGreenLabel.alignment = TextAnchor.MiddleLeft;
            }
            return mGreenLabel;
        }
    }

    private static GUIStyle mBox1;
    public static GUIStyle BoxWhite
    {
        get
        {
            if (null == mBox1)
            {
                mBox1 = new GUIStyle(GUI.skin.box);
                mBox1.fontStyle = FontStyle.Bold;
                mBox1.normal.textColor = Color.red;
                mBox1.normal.background = Texture2D.whiteTexture;
                mBox1.onNormal.textColor = Color.red;
                mBox1.onNormal.background = Texture2D.whiteTexture;
            }
            return mBox1;
        }
    }

    private static GUIStyle mBoldLabelButton;
    public static GUIStyle BoldLabelButton
    {
        get
        {
            if (null == mBoldLabelButton)
            {
                mBoldLabelButton = new GUIStyle(GUI.skin.button);
                mBoldLabelButton.fontSize = 14;
                mBoldLabelButton.fontStyle = FontStyle.Bold;
            }
            return mBoldLabelButton;
        }
    }

    private static GUIStyle mBlueLabelButton;
    public static GUIStyle BlueLabelButton
    {
        get
        {
            if (null == mBlueLabelButton)
            {
                mBlueLabelButton = new GUIStyle(GUI.skin.button);
                mBlueLabelButton.fontSize = 14;
                mBlueLabelButton.fontStyle = FontStyle.Bold;
                mBlueLabelButton.normal.textColor = Color.blue;
                mBlueLabelButton.hover.textColor = Color.blue;
                mBlueLabelButton.richText = true;
            }
            return mBlueLabelButton;
        }
    }

    private static GUIStyle mFlatWindow;
    public static GUIStyle FlatWindow
    {
        get
        {
            if (null == mFlatWindow)
            {
                mFlatWindow = new GUIStyle(GUI.skin.window);
                mFlatWindow.margin = new RectOffset(5, 5, 0, 0);
                mFlatWindow.padding = new RectOffset(5, 5, 5, 5);
            }
            return mFlatWindow;
        }
    }

    public static void DrawOutline(Rect r, string t, int strength, GUIStyle style)
    {
        GUI.color = new Color(0, 0, 0, 1);
        int i;
        for (i = -strength; i <= strength; i++)
        {
            GUI.color = new Color(0, 0, 0, 1);
            GUI.Label(new Rect(r.x - strength, r.y + i, r.width, r.height), t, style);
            GUI.color = new Color(0.9f, 0.9f, 0.9f, 1);
            GUI.Label(new Rect(r.x + strength, r.y + i, r.width, r.height), t, style);
        }

        for (i = -strength + 1; i <= strength - 1; i++)
        {
            GUI.color = new Color(0, 0, 0, 1);
            GUI.Label(new Rect(r.x + i, r.y - strength, r.width, r.height), t, style);
            GUI.color = new Color(0.9f, 0.9f, 0.9f, 1);
            GUI.Label(new Rect(r.x + i, r.y + strength, r.width, r.height), t, style);
        }
    }

    public static Texture TexWarning = EditorGUIUtility.Load("ZEditor/icon-warning.png") as Texture;

    public static void ErrorField(string str)
    {
        GUIContent c = new GUIContent(str, TexWarning);
        EditorGUILayout.LabelField(c, FontBoldRed, GUILayout.Height(22));
    }

    public static void DrawHeader(string str)
    {
        str = $"::: {str} :::";

        EditorGUILayout.LabelField(str, EditorStyles.boldLabel);
    }

    public static void Space(float size = 10f)
    {
        GUILayout.Space(size);
    }

    public static void DrawRaycastPaddingControl(Graphic target)
    {
        if (target.raycastTarget)
        {
            Space();
            DrawHeader("Padding Control");

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Padding Size UP"))
                {
                    AddRaycastPadding(-5);
                }

                if (GUILayout.Button("Padding Size DOWN"))
                {
                    AddRaycastPadding(+5);
                }
            }
        }

        void AddRaycastPadding(float f)
        {
            var origin = target.raycastPadding;

            origin.x += f;
            origin.y += f;
            origin.z += f;
            origin.w += f;

            target.raycastPadding = origin;

            EditorUtility.SetDirty(target);
        }
    }

    public static void Draw_Separator()
    {
        GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(2f));
    }

    public static void Draw_Separator(Color color, float space = 5f)
    {
        EditorGUILayout.Space(space);
        Rect rt = EditorGUILayout.GetControlRect(false, GUILayout.Height(2));
        rt.height = 2f;
        EditorGUI.DrawRect(rt, color);
        EditorGUILayout.Space(space);
    }

    public static bool GUI_CenterButton(string label, params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        bool ret = GUILayout.Button(label, options);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        return ret;
    }

    public static void CenterLabel(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(content, style, options);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public static void HelpBox(string label, MessageType type, bool wide)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        {
            EditorGUILayout.HelpBox(label, MessageType.Info, wide);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public static bool ToggleChanged(string label, bool value)
    {
        bool retVal = GUILayout.Toggle(value, label, GUILayout.Width(150f));

        return retVal != value;
    }

    /// <summary> 가운데 정렬 기반 GUILayout.Toolbar </summary>
    public static int CenteredToobar(int selected, GUIContent[] contents)
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            selected = GUILayout.Toolbar((int)selected, contents, "LargeButton", GUILayout.ExpandWidth(true), GUILayout.Height(40));
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        return selected;
    }

    /// <summary> 가운데 정렬 기반 GUILayout.Toolbar </summary>
    public static int CenteredToolbar(int selected, GUIContent[] contents, int width)
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            selected = GUILayout.Toolbar((int)selected, contents, "LargeButton", GUILayout.ExpandWidth(true), GUILayout.Width(width), GUILayout.Height(40));
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        return selected;
    }

    /// <summary> 색상 칠해진 박스 그리기 </summary>
    public static void ColoredBox(Color color, int width, int height)
    {
        var bgTex = GUI.skin.box.normal.background;
        GUI.skin.box.normal.background = GetColoredTexture(color);
        GUILayout.Box(GUIContent.none, GUILayout.Width(width), GUILayout.Height(height));
        GUI.skin.box.normal.background = bgTex;
    }

    /// <summary> 색상 칠해진 박스 그리기 </summary>
    public static void ColoredBox(Rect rect, Color color)
    {
        var bgTex = GUI.skin.box.normal.background;
        GUI.skin.box.normal.background = GetColoredTexture(color);
        GUI.Box(rect, GUIContent.none);
        GUI.skin.box.normal.background = bgTex;
    }

    /// <summary> 렉트 영역의 가운데 렉트 계산 </summary>
    public static Rect GetCenterRect(Rect rt, float width, float height)
    {
        return new Rect(rt.x + (rt.width * 0.5f) - width * 0.5f, rt.y + (rt.height * 0.5f) - height * 0.5f, width, height);
    }

    public static Vector2 DrawGridItems(Vector2 scrollPos, float gapSpace, int slotCount, float gridBoardWidth, Vector2 slotSize, int bottomPadding, Action<int, Rect> onDraw)
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        {
            int slotHorCnt = (int)(gridBoardWidth / slotSize.x);
            if (slotHorCnt <= 0)
            {
                slotHorCnt = 1;
            }

            int slotVerCnt = slotCount / slotHorCnt;

            if (slotCount % slotHorCnt > 0)
            {
                slotVerCnt++;
            }

            if (slotVerCnt <= 0)
            {
                slotVerCnt = 1;
            }

            for (int i = 0; i < slotVerCnt; i++)
            {
                GUILayout.BeginHorizontal();
                {
                    for (int j = 0; j < slotHorCnt; j++)
                    {
                        int idx = j + (i * slotHorCnt);

                        if (idx >= slotCount)
                        {
                            break;
                        }

                        var layoutArea = GUILayoutUtility.GetRect(slotSize.x, slotSize.y, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
                        onDraw(idx, layoutArea);

                        GUILayout.Space(gapSpace);
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();

        if (bottomPadding > 0)
        {
            GUILayout.Space(bottomPadding);
        }

        return scrollPos;
    }

    static ColoredTexture ColoredTextureBuffer { get; set; } = new ColoredTexture(1, 1);
    public static string OpenFilePanelWithFilters(string inFilePath, string[] filters = null)
    {
        string path = EditorUtility.OpenFilePanelWithFilters("Open File", Application.dataPath, filters);

        if (string.IsNullOrEmpty(path) == false)
        {
            inFilePath = path;
            // 유효하지 않을떄 처리
            //EditorPrefs.SetString("", path);
        }

        return inFilePath;
    }

    public static string SaveFilePanel(string inFilePath, string extension = "dat")
    {
        string path = EditorUtility.SaveFilePanel("Save File", Application.dataPath, string.Empty, extension);

        if (string.IsNullOrEmpty(path) == false)
        {
            inFilePath = path;
            // 유효하지 않을떄 처리
            //EditorPrefs.SetString("", path);
        }

        return inFilePath;
    }

    /// <summary> 주어진 경로에 해당하는 Asset을 찾아서 Project창에 포커싱. </summary>
    public static bool SelectObjectInProject(string assetPath)
    {
        EditorUtility.FocusProjectWindow();

        var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath/*"Assets/Scripts/Player/Okay"*/);

        Selection.activeObject = obj;

        EditorGUIUtility.PingObject(obj);

        return null != obj;
    }

    public static Texture2D GetColoredTexture(Color32 color)
    {
        return ColoredTextureBuffer.GetTex(color);
    }

    public static GUIStyle GetStyle(string styleName)
    {
        return GUI.skin.GetStyle(styleName);
    }

    /// <summary> <see cref="GUIContent"/> 을 색상별 Textrue 로 사용 클래스 </summary>
    class ColoredTexture
    {
        public Dictionary<Color32, Texture> buffer = new Dictionary<Color32, Texture>();
        public int width, height;
        Color32[] colorsReuse;

        public ColoredTexture(int width, int height)
        {
            this.width = width;
            this.height = height;
            colorsReuse = new Color32[width * height];
        }

        public void AddInternal(Color32 color)
        {
            var tex = new Texture2D(width, height);
            for (int i = 0; i < width * height; i++)
            {
                colorsReuse[i] = color;
            }

            tex.SetPixels32(colorsReuse);
            tex.Apply();

            if (buffer.ContainsKey(color))
            {
                buffer[color] = tex;
            }
            else
            {
                buffer.Add(color, tex);
            }
        }

        public Texture Get(Color32 color)
        {
            if (buffer.ContainsKey(color) == false || buffer[color] == null)
            {
                AddInternal(color);
            }

            return buffer[color];
        }

        public Texture2D GetTex(Color32 color)
        {
            if (buffer.ContainsKey(color) == false || buffer[color] == null)
            {
                AddInternal(color);
            }

            return buffer[color] as Texture2D;
        }
    }
}
#endif
