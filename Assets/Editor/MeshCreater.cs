﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Assets.Editor
{
    public class MeshCreater : EditorWindow
    {

        float m_fWidth = 8f;
        float m_fHeight = 8f;
        int m_iRow = 2;
        int m_iCol = 2;
        Shader m_shader = Shader.Find("Custom/TestShader");
        string m_texName = "normal";
        string m_saveFileName = "mesh.smf";
        SmfMesh m_smfMesh = new SmfMesh();

        // Add menu named "My Window" to the Window menu
        //添加菜单项My Window到Window菜单
        [MenuItem("Window/MeshCreater")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MeshCreater));

            
        }

        void OnGUI()
        {
            Event e = Event.current;
            if(e.type == EventType.MouseUp)
            {
                int a = 0;
            }
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

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
                    m_smfMesh.saveFile("Assets/"+m_saveFileName, m_fWidth, m_fHeight, m_iRow, m_iCol, m_shader.name, m_texName);
                
            }
        }


    }
}
