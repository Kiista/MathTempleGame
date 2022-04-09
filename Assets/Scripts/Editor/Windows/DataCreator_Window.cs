using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataCreator_Window : EditorWindow
{
    [SerializeField] Data data = new Data();

    SerializedObject serializedObject = null;
    SerializedProperty questionsProp = null;

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        data.Questions = new Question[0];
        questionsProp = serializedObject.FindProperty("data").FindPropertyRelative("Questions");

    }

    [MenuItem("Game/Data Creator")]

    public static void OpenWindow ()
    {
        var window = EditorWindow.GetWindow<DataCreator_Window>("Creator");

        window.minSize = new Vector2(510.0f, 344.0f );
        window.Show();
    }

    private void OnGUI()
    {
        #region Header Section
        Rect headerRect = new Rect(15, 15, this.position.width - 30, 65);
        GUI.Box(headerRect, GUIContent.none);

        GUIStyle headerStyle = new GUIStyle(EditorStyles.largeLabel)
        {
            fontSize = 26,
            alignment = TextAnchor.UpperLeft,
        };
        headerRect.x += 5;
        headerRect.width -= 10;
        headerRect.y += 5;
        headerRect.height -= 10;
        GUI.Label(headerRect, "Data to XML Creator", headerStyle);

        Rect summatyRect = new Rect(headerRect.x + 25, headerRect.y + headerRect.height - 20, headerRect.width - 50, 15);
        GUI.Label(summatyRect, "Create the data that needs to be included into the XML file");
        #endregion

        #region Body Section

        Rect bodyRect = new Rect(15, (headerRect.y + headerRect.height) + 15, this.position.width - 30, this.position.height - (headerRect.y + headerRect.height) -80 );
        GUI.Box(bodyRect, GUIContent.none);

        #endregion
    }
}
