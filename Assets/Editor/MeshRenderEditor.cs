using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SmfReader))]
public class MeshRenderEditor : Editor {

    bool isControlPos = true;
    bool isControlRot = true;

    Vector3 pos = new Vector3();
    Vector3 rot = new Vector3();
    public override void OnInspectorGUI()
    {
        SmfReader mr = (SmfReader)target;

        Object pre = mr.m_smfMesh;
        mr.m_smfMesh = EditorGUILayout.ObjectField("myMesh", mr.m_smfMesh, typeof(Object), true) as Object;
        if(pre != mr.m_smfMesh)
        {
            mr.UpdateMesh();
        }
        isControlPos = EditorGUILayout.BeginToggleGroup("控制位置", isControlPos);
        Vector3 preP = pos;
        pos = EditorGUILayout.Vector3Field("Pos", pos);
        if(preP != pos)
        {
            mr.transform.position = pos;
        }
        EditorGUILayout.EndToggleGroup();

        isControlRot = EditorGUILayout.BeginToggleGroup("控制旋转", isControlRot);
        Vector3 preR = rot;
        rot = EditorGUILayout.Vector3Field("Rot", rot);
        if (preR != rot)
        {
            mr.transform.rotation = Quaternion.Euler(rot);
        }
        EditorGUILayout.EndToggleGroup();
    }

    void OnSceneGUI()
    {
        SmfReader mr = (SmfReader)target;
        if (mr.m_smfMesh == null)
            return;
        Transform starTransform = mr.transform;
        Vector3 middle = new Vector3(mr.fileFilter.m_middleX, mr.fileFilter.m_middleY, mr.fileFilter.m_middleZ);
        if(isControlPos)
        {
            Vector3 oldPoint = starTransform.TransformPoint(middle);
            Vector3 newPoint = Handles.PositionHandle(oldPoint, Quaternion.identity);
            if(oldPoint != newPoint)
                starTransform.Translate(starTransform.InverseTransformPoint(newPoint) - middle);

            pos = starTransform.position;
        }
        
        if(isControlRot)
        {
            starTransform.Translate(middle);
            starTransform.rotation = Handles.RotationHandle(starTransform.rotation, starTransform.position);
            starTransform.Translate(-middle);

            rot = starTransform.rotation.eulerAngles;
        }
        

    }
}
