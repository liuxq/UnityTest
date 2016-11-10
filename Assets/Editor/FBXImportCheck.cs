/*
 * CREATED:     2016-9-22
 * PURPOSE:     fbx import check
 * AUTHOR:      Liuxiaoqiang
 */

using UnityEngine;
using System.Collections;
using UnityEditor;

public class FBXImportCheck : AssetPostprocessor
{
    private string path = "Assets/Resources/Model";
    private void OnPreprocessModel()
    {
        ModelImporter modelim = assetImporter as ModelImporter;
        if (modelim == null)
            return;

        //model目录下带动作的fbx不要导入材质
        if (assetPath.Contains(path))
        {
            if (modelim.importAnimation && modelim.clipAnimations.Length > 0 && modelim.importMaterials)
            {
                modelim.importMaterials = false;
            }
        }
    }

    //模型导入之前调用  
    public void OnPostprocessModel(GameObject go)
    {
        if (go != null)
        {
            Renderer rd = go.GetComponentInChildren<Renderer>();

            //提示材质有问题的fbx
            if (rd != null && rd.sharedMaterial.mainTexture == null)
            {
                ModelImporter modelim = assetImporter as ModelImporter;
                if (modelim != null && modelim.importMaterials == true)
                {
                    UnityEngine.Debug.LogError("fbx材质问题, 模型:" + assetPath + "的材质mainTexture是空！检查是否要取消勾选Import Materials");
                }
            }
        }
    }

    //private void OnAssignMaterialModel(Material material, Renderer renderer)
    //{
    //    string path = assetImporter.assetPath.Replace("Assets/Resources/", "");
    //    path = path.Replace(".FBX", "");
    //    UnityEngine.GameObject go = Resources.Load(path) as GameObject;

    //    Renderer rd = go.GetComponentInChildren<Renderer>();
        

    //    //提示材质有问题的fbx
    //    if (material.mainTexture == null)
    //    {
    //        ModelImporter modelim = assetImporter as ModelImporter;
    //        if (modelim != null && modelim.importMaterials == true)
    //        {
    //            EditorUtility.DisplayDialog("fbx材质问题", "模型:" + assetPath + "的材质mainTexture是空！检查是否要取消勾选Import Materials", "ok");
    //        }
    //    }
    //}
}
