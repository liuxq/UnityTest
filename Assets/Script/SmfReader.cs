using UnityEngine;
using System.Collections;

[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SmfReader : MonoBehaviour {
    
    public Object m_smfMesh;
    
    public int a = 0;
    public SmfMesh fileFilter;
    Mesh mesh;
	// Use this for initialization
	void Start () {
        
	}

    public void UpdateMesh()
    {
        if (m_smfMesh == null)
        {
            GetComponent<MeshFilter>().mesh = null;
            DestroyImmediate(mesh);
            return;
        }
            
        if(fileFilter == null)
            fileFilter = new SmfMesh();

        string filepath = "Assets/" + m_smfMesh.name + ".smf";
        fileFilter.loadFile(filepath);

        int[] triangles = new int[fileFilter.m_listTriangles.Count * 3];
        for (int i = 0; i < fileFilter.m_listTriangles.Count; i++)
        {
            triangles[i * 3] = (int)fileFilter.m_listTriangles[i].p1_;
            triangles[i * 3 + 1] = (int)fileFilter.m_listTriangles[i].p2_;
            triangles[i * 3 + 2] = (int)fileFilter.m_listTriangles[i].p3_;
        }
        if (mesh == null)
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Mesh";
            mesh.hideFlags = HideFlags.HideAndDontSave;
        }

        mesh.vertices = fileFilter.m_listPoints.ToArray();
        mesh.triangles = triangles;
        mesh.uv = fileFilter.m_listUVs.ToArray();
        mesh.normals = fileFilter.m_listNormals.ToArray();

        Material mat = new Material(Shader.Find(fileFilter.m_MatShaderName));
        Texture2D tex = Resources.Load(fileFilter.m_MatMainTextureName) as Texture2D;
        mat.SetTexture("_NormalTex",tex);

        GetComponent<MeshRenderer>().material = mat;
    }

    void OnEnable()
    {
        UpdateMesh();
    }

    void OnDisable()
    {
        if (Application.isEditor)
        {
            GetComponent<MeshFilter>().mesh = null;
            DestroyImmediate(mesh);
        }
    }

    void Reset()
    {
        UpdateMesh();
    }

   

}
