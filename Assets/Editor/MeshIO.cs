using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;

namespace Assets.Editor
{
    public class MeshIO : EditorWindow
    {

        float m_fWidth = 8f;
        float m_fHeight = 8f;
        int m_iRow = 2;
        int m_iCol = 2;
        Shader m_shader = Shader.Find("Mobile/Unlit (Supports Lightmap)");
        string m_texName = "env";
        string m_saveFileName = "mesh.smf";
        SmfMesh m_smfMesh = new SmfMesh();

        static MeshIO m_win;
        static MeshIO2 m_win2;

        // Add menu named "My Window" to the Window menu
        //添加菜单项My Window到Window菜单
        [MenuItem("Window/My Window")]
        public static void ShowWindow()
        {
            m_win = EditorWindow.GetWindow(typeof(MeshIO)) as MeshIO;
            m_win2 = EditorWindow.GetWindow(typeof(MeshIO2)) as MeshIO2;

            Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
            Type lxq = assembly.GetType("UnityEditor.EditorWindow");
            var pi = lxq.GetField("m_Parrent", BindingFlags.NonPublic | BindingFlags.Instance);
            int a = 0;
        }

        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            //myString = EditorGUILayout.TextField("Text Field", myString);

            //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            //myBool = EditorGUILayout.Toggle("Toggle", myBool);
            //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            //EditorGUILayout.EndToggleGroup();

            m_fWidth = EditorGUILayout.Slider("格宽度", m_fWidth, 1, 10);
            m_fHeight = EditorGUILayout.Slider("格高度", m_fHeight, 1, 10);
            m_iRow = (int)EditorGUILayout.Slider("行数", m_iRow, 1, 4);
            m_iCol = (int)EditorGUILayout.Slider("列数", m_iCol, 1, 4);
            m_shader = EditorGUILayout.ObjectField("Shader", m_shader, typeof(Shader), true) as Shader;
            m_texName = EditorGUILayout.TextField("纹理名字，请放到resources", m_texName);
            m_saveFileName = EditorGUILayout.TextField("保存的smf文件路径", m_saveFileName);
            if(GUILayout.Button("创建mesh文件（.smf）"))
            {
                if(m_shader!= null)
                    m_smfMesh.saveFile("Assets/lxq.smf", m_fWidth, m_fHeight, m_iRow, m_iCol, m_shader.name, m_texName);
                
            }
        }


    }

    public class MeshIO2 : EditorWindow
    {

        float m_fWidth = 8f;
        float m_fHeight = 8f;
        int m_iRow = 2;
        int m_iCol = 2;
        Shader m_shader = Shader.Find("Mobile/Unlit (Supports Lightmap)");
        string m_texName = "env";
        string m_saveFileName = "mesh.smf";
        SmfMesh m_smfMesh = new SmfMesh();

        // Add menu named "My Window" to the Window menu
        //添加菜单项My Window到Window菜单
        [MenuItem("Window/My Window2")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MeshIO2));


        }

        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            //myString = EditorGUILayout.TextField("Text Field", myString);

            //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            //myBool = EditorGUILayout.Toggle("Toggle", myBool);
            //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            //EditorGUILayout.EndToggleGroup();

            m_fWidth = EditorGUILayout.Slider("格宽度", m_fWidth, 1, 10);
            m_fHeight = EditorGUILayout.Slider("格高度", m_fHeight, 1, 10);
            m_iRow = (int)EditorGUILayout.Slider("行数", m_iRow, 1, 4);
            m_iCol = (int)EditorGUILayout.Slider("列数", m_iCol, 1, 4);
            m_shader = EditorGUILayout.ObjectField("Shader", m_shader, typeof(Shader), true) as Shader;
            m_texName = EditorGUILayout.TextField("纹理名字，请放到resources", m_texName);
            m_saveFileName = EditorGUILayout.TextField("保存的smf文件路径", m_saveFileName);
            if (GUILayout.Button("创建mesh文件（.smf）"))
            {
                if (m_shader != null)
                    m_smfMesh.saveFile("Assets/lxq.smf", m_fWidth, m_fHeight, m_iRow, m_iCol, m_shader.name, m_texName);

            }
        }


    }
}
