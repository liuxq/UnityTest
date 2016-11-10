using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Assets.Editor
{
    public class FBXProcess : EditorWindow
    {

        float m_fWidth = 8f;
        float m_fHeight = 8f;
        int m_iRow = 2;
        int m_iCol = 2;
        Object mObj = null;
        AnimationClip ac = null;
        AnimationClip ac2 = null;
        AnimationClip ac3 = null;
        string m_texName = "normal";
        string m_saveFileName = "mesh.smf";
        SmfMesh m_smfMesh = new SmfMesh();

        AnimatorOverrideController mAnimatorControl = null;

        UnityEditor.Animations.AnimatorController mControler = null;

        Animator anim;

        // Add menu named "My Window" to the Window menu
        //添加菜单项My Window到Window菜单
        [MenuItem("Window/FBXProcess")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(FBXProcess));
        }

        void OnGUI()
        {
            Event e = Event.current;
            if(e.type == EventType.MouseUp)
            {
                int a = 0;
            }
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            mObj = EditorGUILayout.ObjectField("FBX", mObj, typeof(Object), true) as Object;
            ac = EditorGUILayout.ObjectField("FBX", ac, typeof(AnimationClip), true) as AnimationClip;
            ac2 = EditorGUILayout.ObjectField("FBX", ac2, typeof(AnimationClip), true) as AnimationClip;
            ac3 = EditorGUILayout.ObjectField("FBX", ac3, typeof(AnimationClip), true) as AnimationClip;
            mControler = EditorGUILayout.ObjectField("FBX", mControler, typeof(UnityEditor.Animations.AnimatorController), true) as UnityEditor.Animations.AnimatorController;

            if (GUILayout.Button("加载模型"))
            {
                anim = (mObj as GameObject).GetComponentInChildren<Animator>(true);
                //anim.runtimeAnimatorController = mControler;
                //mAnimatorControl = new AnimatorOverrideController();
                //anim.stabilizeFeet = true;
            }
            if (GUILayout.Button("站立"))
            {
                AnimatorOverrideController overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController == null)
                {
                    overrideController = new AnimatorOverrideController();
                    overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
                }
                overrideController["StandbyLayer"] = ac3;
                //                 if (ReferenceEquals(anim.runtimeAnimatorController, overrideController) == false)
                //                 {
                anim.runtimeAnimatorController = null;
                anim.runtimeAnimatorController = overrideController;
                //}

                anim.CrossFade("active", 0.3f, 0, 0f);
            }
            if (GUILayout.Button("战斗站立"))
            {
                AnimatorOverrideController overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController == null)
                {
                    overrideController = new AnimatorOverrideController();
                    overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
                }
                overrideController["RunLayer"] = ac3;

                anim.runtimeAnimatorController = null;
                anim.runtimeAnimatorController = overrideController;

                //anim.Play("active", 0, 0f);
                anim.CrossFade("active", 0.3f, 1, 0f);
            }
            if (GUILayout.Button("跑步左"))
            {
                AnimatorOverrideController overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController == null)
                {
                    overrideController = new AnimatorOverrideController();
                    overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
                }
                overrideController["RunLayer"] = ac;

                anim.runtimeAnimatorController = null;
                anim.runtimeAnimatorController = overrideController;

                //anim.Play("active", 0, 0f);
                anim.CrossFade("active", 0.3f, 2, 0f);
            }
            if (GUILayout.Button("跑步右"))
            {
                AnimatorOverrideController overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController == null)
                {
                    overrideController = new AnimatorOverrideController();
                    overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
                }
                overrideController["RunLayer"] = ac2;

                anim.runtimeAnimatorController = null;
                anim.runtimeAnimatorController = overrideController;

                //anim.Play("active", 0, 0f);
                anim.CrossFade("active", 0.3f, 2, 0f);
                //anim.CrossFade("active", 1.5f, 0, 0f);
            }

            if (GUILayout.Button("跑步"))
            { 
                //anim.Play("active", 0, 0f);
                anim.CrossFade("active", 0.3f, 2, 0f);
                //anim.CrossFade("active", 1.5f, 0, 0f);
            }
            if (GUILayout.Button("拔刀"))
            {
                //anim.Play("active", 0, 0f);
                anim.CrossFade("active", 0.3f, 3, 0f);
                //anim.CrossFade("active", 1.5f, 0, 0f);
            }
        }


    }
}
