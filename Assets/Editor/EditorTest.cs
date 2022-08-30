using System.Diagnostics;
using System.IO;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Debug = UnityEngine.Debug;

public class EditorTest : EditorWindow
{
    public static void DoBuild()
    {
        string shPath = $"{Directory.GetParent(Application.dataPath).FullName}/Test.sh";
        Debug.Log("ShPath : " + shPath);

        ProcessStartInfo info = new ProcessStartInfo($"{shPath}");
        var process = System.Diagnostics.Process.Start(info);
        process.WaitForExit();
    }

    [MenuItem("Test/Open %q", priority = 1)]
    static void Test()
    {
        GetWindow<EditorTest>();
    }

    [Serializable]
    public class TexData
    {
        public TextureImporter importer;
        public GUIContent content;
        public int size;

        public TexData(TextureImporter importer)
        {
            this.importer = importer;
            content = new GUIContent(Path.GetFileNameWithoutExtension(Path.GetFileName(importer.assetPath)));
            size = importer.GetPlatformTextureSettings("Android").maxTextureSize;
        }
    }

    [Serializable]
    public class Category
    {
        public string name;

        public Category(string name)
        {
            this.name = name;
        }
    }

    public class Group
    {
        public Category ctg;
        public List<TexData> texList = new List<TexData>();
        public bool isFolded;

        public Group(Category ctg, List<TexData> texList)
        {
            this.ctg = ctg;
            this.texList = texList;
        }
    }

    public List<Group> groups = new List<Group>();
    class Styles
    {
        public static GUIContent TexMenuContent = EditorGUIUtility.IconContent("_Menu", "¸Þ´º");
    }

    GUIStyle FoldHeaderStyle;
    GUIStyle TexDataStyle;

    #region ==== Utils ========

    //public static string GetAssetGUIDs<AssetType>(params Type[] types)
    //    where AssetType : UnityEngine.Object
    //{
    //    return null;
    //    //AssetDatabase.FindAssets()
    //}

    #endregion

    public enum CategoryType
    {
        None = -1,
        Category01,
        Category02,
        Max
    }

    float btnPosX;
    float namePosX;
    float sizePosX;

    bool doInitOnFirstOnGUI;

    void Init()
    {
        FoldHeaderStyle = new GUIStyle(EditorStyles.foldoutHeader);
        {
            FoldHeaderStyle.fontSize = 20;
        }

        TexDataStyle = new GUIStyle(EditorStyles.label);
        {
            TexDataStyle.fontSize = 15;
        }
    }

    private void OnEnable()
    {
        Init();

        groups.Clear();

        for (int i = 0; i < (int)CategoryType.Max; i++)
        {
            groups.Add(new Group(new Category(((CategoryType)i).ToString()), new List<TexData>(256)));
        }

        var texPaths = AssetDatabase.FindAssets($"t:Texture", new string[] { "Assets" });
        for (int i = 0; i < texPaths.Length; i++)
        {
            var importer = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(texPaths[i])) as TextureImporter;
            if (importer != null)
            {
                groups[0].texList.Add(new TexData(importer));
            }
        }

        doInitOnFirstOnGUI = true;
    }

    private void OnGUI()
    {
        if (doInitOnFirstOnGUI)
        {
            doInitOnFirstOnGUI = false;

            btnPosX = position.width * 0.1f;
            namePosX = position.width * 0.15f;
            sizePosX = position.width * 0.5f;
        }

        EditorGUILayout.Space();

        for (int i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            group.isFolded = EditorGUILayout.Foldout(group.isFolded, group.ctg.name, true, FoldHeaderStyle);
            if (group.isFolded)
            {
                EditorGUILayout.Space();

                EditorGUI.indentLevel += 2;
                {
                    foreach (var texData in group.texList)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        {
                            // GUILayout.Space(20);

                            Rect rt = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(22));

                            rt.width = EditorStyles.toolbarButton.CalcSize(Styles.TexMenuContent).x; //  GUILayoutUtility.GetRect(Styles.TexMenuContent, EditorStyles.toolbarButton, GUILayout.Width(50));
                            rt.x = btnPosX;

                            if (GUI.Button(rt, Styles.TexMenuContent, EditorStyles.toolbarButton))
                            {

                            }

                            // var rt = GUILayoutUtility.GetRect(texData.content, TexDataStyle, GUILayout.Width(TexDataStyle.CalcSize(texData.content).x));
                            // GUI.Box(rt, "ABC");
                            rt.x = namePosX;
                            rt.width = TexDataStyle.CalcSize(texData.content).x;
                            GUI.Label(rt, texData.content, TexDataStyle);

                            Debug.Log(sizePosX);
                            rt.x = sizePosX;
                            rt.width = TexDataStyle.CalcSize(new GUIContent(texData.size.ToString() + " MB")).x;
                            GUI.Label(rt, $"{texData.size} MB", TexDataStyle);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                EditorGUI.indentLevel -= 2;
            }

            EditorGUILayout.Space();
        }
    }
}

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();
    [SerializeField]
    private List<TValue> values = new List<TValue>();
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}