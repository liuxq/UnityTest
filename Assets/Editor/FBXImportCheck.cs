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

    private void OnAssignMaterialModel(Material material, Renderer renderer)
    {
        //提示材质有问题的fbx
        if (material.mainTexture == null)
        {
            ModelImporter modelim = assetImporter as ModelImporter;
            if (modelim != null && modelim.importMaterials == true)
            {
                EditorUtility.DisplayDialog("fbx材质问题", "模型:" + assetPath + "的材质mainTexture是空！检查是否要取消勾选Import Materials", "ok");
            }
        }
    }
}
