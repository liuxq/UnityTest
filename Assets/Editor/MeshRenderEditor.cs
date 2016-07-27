using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SmfReader))]
public class MeshRenderEditor : Editor {

    public override void OnInspectorGUI()
    {
        //得到Test对象
        SmfReader mr = (SmfReader)target;
        //绘制一个窗口
        //Texture lxq;
        //lxq = EditorGUILayout.ObjectField("增加一个贴图", lxq, typeof(Texture), true) as Texture;
        mr.m_smfMesh = EditorGUILayout.ObjectField("myMesh", mr.m_smfMesh, typeof(Object), true) as Object;
    }
}
