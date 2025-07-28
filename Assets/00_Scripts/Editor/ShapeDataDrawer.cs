using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ShapeData))]
[CanEditMultipleObjects]
[Serializable]
public class ShapeDataDrawer : Editor
{
    private ShapeData shapeDataIntance => target as ShapeData;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        InputFields();
        DrawBoard();
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(shapeDataIntance);
        }
    }

    private void InputFields()
    {
        int oldRow = shapeDataIntance.row;
        int oldColumn = shapeDataIntance.column;
        shapeDataIntance.row = Mathf.Max(0 , EditorGUILayout.IntField("Row Input", shapeDataIntance.row));
        shapeDataIntance.column = Mathf.Max(0, EditorGUILayout.IntField("Column Input", shapeDataIntance.column));
        if(oldRow != shapeDataIntance.row || oldColumn != shapeDataIntance.column) shapeDataIntance.CreateNewBoard();
    }

    private void DrawBoard()
    {

        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 65;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldsStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldsStyle.normal.background = Texture2D.grayTexture;
        dataFieldsStyle.onNormal.background = Texture2D.whiteTexture;
        for (int r = 0; r < shapeDataIntance.row; r++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);
            for (int c = 0; c < shapeDataIntance.column; c++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(shapeDataIntance.rows[r].columns[c], dataFieldsStyle);
                shapeDataIntance.rows[r].columns[c] = data;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private Texture2D MakeColorTexture(Color color)
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();
        return tex;
    }

   private GUIStyle GetToggleStyle(bool isOn)
    {
        var style = new GUIStyle(GUI.skin.button);
        Texture2D bg = MakeColorTexture(isOn ? Color.white : Color.gray);

        style.normal.background = bg;
        style.active.background = bg;
        style.hover.background = bg;
        style.onNormal.background = bg;
        style.onActive.background = bg;
        style.onHover.background = bg;

        style.fixedHeight = 30;
        style.fixedWidth = 30;
        style.margin = new RectOffset(2, 2, 2, 2);  
        style.padding = new RectOffset(0, 0, 0, 0); 
        return style;
    }

}
