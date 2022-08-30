using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ZTestClass))]
public class TestSceneView : Editor
{
    ZTestClass Target;

    Rect rtWindow;

    private void OnEnable()
    {
        Debug.LogError("OnEnable");

        Target = base.target as ZTestClass;

        rtWindow = new Rect(30, 30, 200, 200);
    }

    private void OnSceneGUI()
    {
        if (Target == null)
        {
            return;
        }

        Handles.BeginGUI();
        {
            if (Event.current.type != EventType.Repaint)
            {
                int controlId = GUIUtility.GetControlID(FocusType.Passive);
                rtWindow = GUILayout.Window(controlId, rtWindow, OnDraw, "Fx¡§∫∏");
            }
        }
        Handles.EndGUI();
    }

    private void OnDraw(int windowId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
        //GUILayout.BeginHorizontal();
        //GUILayout.FlexibleSpace();
        ////GUILayout.Label("Holy Title!");
        //GUILayout.FlexibleSpace();
        //GUILayout.EndHorizontal();
    }
}
