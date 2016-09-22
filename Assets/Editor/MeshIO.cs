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
        static MeshIO3 m_win3;

        public static void DockEditorWindow(EditorWindow parent, EditorWindow child)
        {
            Vector2 screenPoint = parent.position.min + new Vector2(parent.position.width * .9f, 100f);

            Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
            Type ew = assembly.GetType("UnityEditor.EditorWindow");
            Type da = assembly.GetType("UnityEditor.DockArea");
            Type sv = assembly.GetType("UnityEditor.SplitView");
            Type ss = assembly.GetType("UnityEditor.SplitterState");

            var tp = ew.GetField("m_Parent", BindingFlags.NonPublic | BindingFlags.Instance);
            var opArea = tp.GetValue(parent);
            var ocArea = tp.GetValue(child);
            var tview = da.GetProperty("parent", BindingFlags.Public | BindingFlags.Instance);
            var oview = tview.GetValue(opArea, null);
            var tDragOver = sv.GetMethod("DragOver", BindingFlags.Public | BindingFlags.Instance);
            var oDropInfo = tDragOver.Invoke(oview, new object[] { child, screenPoint });
            var tDockArea_ = da.GetField("s_OriginalDragSource", BindingFlags.NonPublic | BindingFlags.Static);
            tDockArea_.SetValue(null, ocArea);
            var tPerformDrop = sv.GetMethod("PerformDrop", BindingFlags.Public | BindingFlags.Instance);
            tPerformDrop.Invoke(oview, new object[] { child, oDropInfo, null });

            //var tsplite = sv.GetField("splitState", BindingFlags.NonPublic | BindingFlags.Instance);
            //var osplite = tsplite.GetValue(oview);
            //var tRealSize = ss.GetField("realSizes", BindingFlags.Public | BindingFlags.Instance);

            //parent.position = new Rect(0, 0, 1000, 500);
            //parent.position = new Rect(500, 0, 500, 500);

            //int[] lxq = new int[] { 2500, 2500 };
            //tRealSize.SetValue(osplite, lxq);
            //var oRealSize = tRealSize.GetValue(osplite);

            ////var tReflow = sv.GetMethod("Reflow", BindingFlags.NonPublic | BindingFlags.Instance);
            ////tReflow.Invoke(oview, null);

            //var tSetupRectsFromSplitter = sv.GetMethod("SetupRectsFromSplitter", BindingFlags.NonPublic | BindingFlags.Instance);
            //tSetupRectsFromSplitter.Invoke(oview, null);
            //int a = 0;
            //parent.position = new Rect(0, 0, 900, 900);

            //Vector2 screenPoint = GUIUtility.GUIToScreenPoint(mousePos);
            //DockArea area = parent.m_Parent as DockArea;
            //SplitView sv = area.m_parent as SplitView;
            //DropInfo di = sv.DragOver(child, screenPoint);
            //DockArea.s_OriginalDragSource = child.m_Parent;
            //sv.PerformDrop(child, di, screenPoint);
        }

        // Add menu named "My Window" to the Window menu
        //添加菜单项My Window到Window菜单
        [MenuItem("Window/My Window")]
        public static void ShowWindow()
        {
            m_win = EditorWindow.GetWindow(typeof(MeshIO)) as MeshIO;
            m_win2 = EditorWindow.GetWindow(typeof(MeshIO2)) as MeshIO2;
            m_win3 = EditorWindow.GetWindow(typeof(MeshIO3)) as MeshIO3;


            m_win.position = new Rect(0, 0, 100,200);
            

            DockEditorWindow(m_win, m_win2);
            DockEditorWindow(m_win2, m_win3);

            m_win.position = new Rect(500, 500, 700, 700);
            
        }

        void OnGUI()
        {
            GUILayout.Label("Base Settings1", EditorStyles.boldLabel);
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

            if (Event.current.keyCode == KeyCode.K)
            {
                DockEditorWindow(m_win, m_win2);
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
            GUILayout.Label("Base Settings2", EditorStyles.boldLabel);
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

    public class MeshIO3 : EditorWindow
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
        [MenuItem("Window/My Window3")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MeshIO2));
        }

        void OnGUI()
        {
            GUILayout.Label("Base Settings3", EditorStyles.boldLabel);
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
