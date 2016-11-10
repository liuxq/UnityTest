using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class BuildAb{

    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        //BuildPipeline.BuildAssetBundles("lxqAssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);


        List<AssetBundleBuild> maps = new List<AssetBundleBuild>();
        maps.Clear();
        //资源打包

        string[] files = {
                            "Assets/Resources/Fbx/LongZuoQi.FBX",
                            //"Assets/Resources/Fbx/Materials/konglongzuoqi.mat"
                         };

        string[] files1 = {
                            //"Assets/Resources/Fbx/LongZuoQi.FBX",
                            "Assets/Resources/Fbx1/Man.FBX"
                         };

        string[] files2 = {
                            //"Assets/Resources/Fbx/LongZuoQi.FBX",
                            "Assets/TestShader.shader",
                            "Assets/ModelMark.shader"

                         };

        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = "fbx";
        build.assetNames = files;
        maps.Add(build);

        AssetBundleBuild build1 = new AssetBundleBuild();
        build1.assetBundleName = "fbx1";
        build1.assetNames = files1;
        //maps.Add(build1);

        AssetBundleBuild build2 = new AssetBundleBuild();
        build2.assetBundleName = "shader";
        build2.assetNames = files2;
        maps.Add(build2);

        //string resPath = "Assets/" + "StreamingAssets";
        BuildAssetBundleOptions options = BuildAssetBundleOptions.None;
        BuildPipeline.BuildAssetBundles("lxqAssetBundles", maps.ToArray(), options, BuildTarget.StandaloneWindows);

        //maps.Clear();
        //maps.Add(build);
        //maps.Add(build1);
        //BuildPipeline.BuildAssetBundles("lxqAssetBundles", maps.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        AssetDatabase.Refresh();
    }
}
