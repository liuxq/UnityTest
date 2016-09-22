using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UEEngine;

namespace Assets.Editor
{
    public class UEModelMarkWindow : EditorWindow
    {
        private static UEModelMarkWindow mWindow = null;

        float m_fWidth = 8f;
        float m_fHeight = 8f;
        int m_iRow = 2;
        int m_iCol = 2;
        Object m_fbx = null;
        Transform m_trans = null;
        GameObject m_model = null;
        Plane m_plane = new Plane();

        [MenuItem("UE/ModelMarkWindow")]
        public static void Init()
        {
            if (null != mWindow)
            {
                mWindow.Close();
                mWindow = null;
                return;
            }

            mWindow = EditorWindow.GetWindow(typeof(UEModelMarkWindow)) as UEModelMarkWindow;
            SceneView.onSceneGUIDelegate += mWindow.OnSceneGUI;
            mWindow.Show();
        }

        void OnDestroy()
        {
            if (null == mWindow)
            {
                return;
            }
            if (null != m_model)
            {
                DestroyImmediate(m_model);
            }

            SceneView.onSceneGUIDelegate -= mWindow.OnSceneGUI;
            mWindow = null;
            m_plane.Destroy();
        }

        

        private void OnSceneGUI(SceneView sv)
        {
            switch (Event.current.type)
            {
                case EventType.mouseDown:
                    {
                        if (Event.current.control && Event.current.button == 0)
                        {
                            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                            RaycastHit hitInfo;
                            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
                            {
                                m_plane.PaintMark(hitInfo.textureCoord);
                                Event.current.Use();
                                Repaint();
                            }
                        }
                    }
                    break;
            }
        }

        void OnGUI()
        {
            EditorGUILayout.HelpBox("1. 拖动模型到编辑器，点击显示生成掩码\n2. ctrl + 左键： 设置或取消掩码", MessageType.Info);
            m_fbx = EditorGUILayout.ObjectField("家具模型", m_fbx, typeof(Object), true) as Object;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("家具掩码：");
            ulong newMark = ulong.Parse(EditorGUILayout.TextField(m_plane.m_mark.ToString()));
            if(newMark != m_plane.m_mark)
            {
                m_plane.m_mark = newMark;
                m_plane.BuildMark();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("家具高度：" + m_plane.Bound.Extents.y * 2);

            if (GUILayout.Button("显示"))
            {
                if (m_fbx != null)
                {
                    m_model = Instantiate(m_fbx, Vector3.zero, Quaternion.identity) as GameObject;
                    MeshFilter mf = m_model.GetComponentInChildren<MeshFilter>();
                    m_plane.Bound.Clear();
                    for (int i = 0; i < mf.sharedMesh.vertices.Length; i++)
                    {
                        m_plane.Bound.AddVertex(mf.transform.TransformPoint(mf.sharedMesh.vertices[i]));
                    }
                    m_plane.Bound.CompleteCenterExts();
                    m_plane.CreateMesh();
                }
            }
        }
    }

    public class Plane
    {
        GameObject m_object = null;
        public float m_seg = 0.5f;
        public AABB Bound = new AABB();

        public ulong m_mark;//占用49位
        public int m_mark0;//各存20位
        public int m_mark1;
        public int m_mark2;
        public Material m_material = null;
        public Texture2D m_tMark = null;

        private Color[] m_colors = new Color[49];

        public Plane()
        {
            Bound = new AABB();
        }

        public void BuildMark()
        {
            for(int i = 0; i < 49; i++)
            {
                if ((m_mark & (1ul << i)) > 0)
                    m_colors[i] = new Color(1f, 0, 0);
                else
                    m_colors[i] = new Color(0.0f, 0, 0);
            }
            m_tMark.SetPixels(m_colors);
            m_tMark.Apply();
            m_material.SetTexture("_mark", m_tMark);
        }

        public void Destroy()
        {
            if(m_object != null)
            {
                Object.DestroyImmediate(m_object);
                m_object = null;
            }
            Bound = null;
        }

        public void PaintMark(Vector2 uv)
        {
            int x = Mathf.Clamp((int)Mathf.Floor(uv.x * 7), 0, 6);
            int y = Mathf.Clamp((int)Mathf.Floor(uv.y * 7), 0, 6);
            int index = x + y * 7;
            m_mark = m_mark ^ (1ul << index);

            BuildMark();
        }

        public void PaintMarkOverRide(Vector2 uv)
        {
            int x = Mathf.Clamp((int)Mathf.Floor(uv.x * 7), 0, 6);
            int y = Mathf.Clamp((int)Mathf.Floor(uv.y * 7), 0, 6);
            int index = x + y * 7;
            m_mark = m_mark | (1ul << index);

            BuildMark();
        }

        public void CreateMesh()
        {
            if(m_object != null)
            {
                Object.DestroyImmediate(m_object);
                m_object = null;
            }
            m_object = new GameObject();
            m_object.name = "plane";

            float y = Bound.Mins.y;

            float halfLen = m_seg * 3.5f;
            Vector3[] points = new Vector3[]
            {
                Bound.Center + new Vector3(-halfLen, y, -halfLen),
                Bound.Center + new Vector3(halfLen, y, -halfLen),
                Bound.Center + new Vector3(halfLen, y, halfLen),
                Bound.Center + new Vector3(-halfLen, y, halfLen)
            };

            Vector2[] uvs = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };

            int[] triangles = new int[]
            {
                0,1,2,
                0,2,3
            };

            Mesh _Mesh = new Mesh();
            _Mesh.vertices = points;
            _Mesh.triangles = triangles;
            _Mesh.uv = uvs;

            MeshRenderer mr = m_object.AddComponent<MeshRenderer>();
            m_object.AddComponent<MeshFilter>().mesh = _Mesh;
            m_object.AddComponent<MeshCollider>();
            if(mr.sharedMaterial == null)
            {
                m_material = mr.sharedMaterial = new Material(Shader.Find("UE/Editor/ModelMark"));

                m_tMark = new Texture2D(7,7);
                m_material.SetTexture("_mark", m_tMark);

                //m_material.SetInt("_mark0", m_mark0);
                //m_material.SetInt("_mark1", m_mark1);
                //m_material.SetInt("_mark2", m_mark2);
            }

            for (float x = Bound.Mins.x; x < Bound.Maxs.x; x += m_seg)
            {
                for (float z = Bound.Mins.z; z < Bound.Maxs.z; z += m_seg)
                {
                    Vector2 uv = new Vector2();
                    uv.x = Mathf.InverseLerp(-halfLen + Bound.Center.x, halfLen + Bound.Center.x, x);
                    uv.y = Mathf.InverseLerp(-halfLen + Bound.Center.z, halfLen + Bound.Center.z, z);
                    PaintMarkOverRide(uv);
                }
            }
        }
    }
}
