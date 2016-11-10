using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float[] distances = new float[32];

        for (int i = 0; i < Camera.main.layerCullDistances.Length; i++)
            distances[i] = 2f;
        Camera.main.layerCullDistances = distances;
            
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < Camera.main.layerCullDistances.Length; i++)
            Camera.main.layerCullDistances[i] = 3f;
	}
}
