using UnityEngine;
using System.Collections;

public class SmfReader : MonoBehaviour {

    public Object m_smfMesh;

    SmfMesh fileFilter = new SmfMesh();
	// Use this for initialization
	void Start () {
        string filepath = "Assets/" + m_smfMesh.name + ".smf";
        fileFilter.loadFile(filepath);
        
        int[] triangles = new int[fileFilter.m_listTriangles.Count * 3];
        for(int i = 0; i < fileFilter.m_listTriangles.Count; i++)
        {
            triangles[i * 3] = (int)fileFilter.m_listTriangles[i].p1_;
            triangles[i * 3+1] = (int)fileFilter.m_listTriangles[i].p2_;
            triangles[i * 3+2] = (int)fileFilter.m_listTriangles[i].p3_;
        }

        Mesh mesh = gameObject.AddComponent<MeshFilter>().mesh;
        gameObject.AddComponent<MeshRenderer>();
        mesh.vertices = fileFilter.m_listPoints.ToArray();
        mesh.triangles = triangles;
        mesh.uv = fileFilter.m_listUVs.ToArray();
        mesh.normals = fileFilter.m_listNormals.ToArray();

        Material mat = new Material(Shader.Find(fileFilter.m_MatShaderName));
        Texture2D tex = Resources.Load(fileFilter.m_MatMainTextureName) as Texture2D;
        mat.mainTexture = tex;

        GetComponent<MeshRenderer>().material = mat;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
