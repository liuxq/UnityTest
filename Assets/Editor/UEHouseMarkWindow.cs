using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UEEngine;


public class UEHouseMarkWindow: EditorWindow
{
    private static UEHouseMarkWindow mWindow = null;

    float m_xLen = 8f;
    float m_zLen = 8f;
    Vector3 m_vPos = Vector3.zero;
    HousePlane m_plane = null;
    bool m_bFirst = true;

    [MenuItem("UE/房子掩码示意")]
    public static void Init()
    {
        if (null != mWindow)
        {
            mWindow.Close();
            mWindow = null;
            return;
        }

        mWindow = EditorWindow.GetWindow(typeof(UEHouseMarkWindow)) as UEHouseMarkWindow;
        SceneView.onSceneGUIDelegate += mWindow.OnSceneGUI;
        mWindow.Show();
    }

    void OnDestroy()
    {
        if (null == mWindow)
        {
            return;
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
        if (m_plane == null)
            m_plane = new HousePlane();

        EditorGUILayout.HelpBox("ctrl + 左键： 设置或取消选中\n房屋位置用箭头设置或者在transform界面设置", MessageType.Info);

        float xLen = EditorGUILayout.FloatField("x宽度：", m_xLen);
        float zLen = EditorGUILayout.FloatField("z宽度：", m_zLen);
        //Vector3 pos = EditorGUILayout.Vector3Field("房屋位置", m_vPos);

        if (xLen != m_xLen || zLen != m_zLen || m_bFirst)
        {
            m_xLen = xLen;
            m_zLen = zLen;
            m_vPos = Vector3.zero;
            AABB bound = new AABB();
            bound.Clear();
            bound.AddVertex(m_vPos);
            bound.AddVertex(m_vPos + new Vector3(m_xLen, 0, m_zLen));
            bound.CompleteCenterExts();

            m_plane.UpdateMesh(bound);

            m_bFirst = false;
        }
    }
}


public class HousePlane
{
    GameObject m_object = null;
    public float m_seg = 0.5f;

    public ulong m_mark;//占用49位

    public Material m_material = null;
    public Texture2D m_tMark = null;

    public int m_xCount;
    public int m_zCount;

    private Color[] m_colors = new Color[49];

    Vector2[] uvs = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

    int[] triangles = new int[]
        {
            0,2,1,
            0,3,2
        };

    public HousePlane()
    {
    }

    public void BuildMark(int x, int y)
    {
        int count = m_xCount * m_zCount;
        int index = x + y * m_xCount;

        if (m_colors[index].r == 1)
            m_colors[index].r = 0;
        else
            m_colors[index].r = 1;
            
        m_tMark.SetPixels(m_colors);
        m_tMark.Apply();
        m_material.SetTexture("_mark", m_tMark);
    }

    public void Destroy()
    {
        if (m_object != null)
        {
            Object.DestroyImmediate(m_object);
            m_object = null;
        }
    }

    public void PaintMark(Vector2 uv)
    {
        int x = Mathf.Clamp((int)Mathf.Floor(uv.x * m_xCount), 0, m_xCount - 1);
        int y = Mathf.Clamp((int)Mathf.Floor(uv.y * m_zCount), 0, m_zCount - 1);
            
        BuildMark(x, y);
    }

    public void UpdateMesh(AABB Bound)
    {
        if (m_object == null)
        {
            m_object = new GameObject();
            m_object.name = "plane";
        }
            
        float y = Bound.Mins.y;

        m_xCount = (int)Mathf.Ceil(Bound.Extents.x * 2 / 0.5f);
        m_zCount = (int)Mathf.Ceil(Bound.Extents.z * 2 / 0.5f);

        Vector3[] points = new Vector3[]
        {
            new Vector3(Bound.Mins.x, 0, Bound.Mins.z),
            new Vector3(Bound.Maxs.x, 0, Bound.Mins.z),
            new Vector3(Bound.Maxs.x, 0, Bound.Maxs.z),
            new Vector3(Bound.Mins.x, 0, Bound.Maxs.z),
        };

        MeshRenderer mr = m_object.GetComponent<MeshRenderer>();
        if(mr == null)  mr = m_object.AddComponent<MeshRenderer>();

        MeshFilter mf = m_object.GetComponent<MeshFilter>();
        if (mf == null) mf = m_object.AddComponent<MeshFilter>();

        if (mf.sharedMesh == null)
        {
            mf.sharedMesh = new Mesh();
        }
        mf.sharedMesh.vertices = points;
        mf.sharedMesh.triangles = triangles;
        mf.sharedMesh.uv = uvs;

        MeshCollider mc = m_object.GetComponent<MeshCollider>();
        if (mc == null)
            mc = m_object.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.sharedMesh;
            
        if (mr.sharedMaterial == null)
        {
            m_material = mr.sharedMaterial = new Material(Shader.Find("UE/Editor/ModelMark"));
        }

        if (m_tMark == null)
            m_tMark = new Texture2D(m_xCount, m_zCount);

        if (m_colors.Length != m_xCount * m_zCount)
        {
            m_colors = new Color[m_xCount * m_zCount];
            m_tMark.Resize(m_xCount, m_zCount);
        }

        m_material.SetTexture("_mark", m_tMark);
        m_material.SetInt("_xCount", m_xCount);
        m_material.SetInt("_zCount", m_zCount);
    }
}


