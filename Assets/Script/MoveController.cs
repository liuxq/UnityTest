using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {

    bool isPressDown = false;
    Vector2 pos;
    void OnEnable()
    {
        Debug.Log("Registering finger gesture events from C# script");

        // register input events
        FingerGestures.OnFingerDown += OnFingerDown;
        FingerGestures.OnFingerUp += OnFingerUp;
    }

    void OnDisable()
    {
        // unregister finger gesture events
        FingerGestures.OnFingerDown -= OnFingerDown;
        FingerGestures.OnFingerUp -= OnFingerUp;
    }

    void Start()
    {

    }

    void OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        isPressDown = true;
    }
    void OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
    {
        isPressDown = false;
        
    }
    void Update()
    {
        
        if (isPressDown && Input.mousePosition.x != pos.x && Input.mousePosition.y != pos.y)
        {
            //设置角色的朝向（朝向当前坐标+摇杆偏移量）
            Quaternion r = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

            Vector3 dir = r * new Vector3(Input.mousePosition.x - pos.x, 0, Input.mousePosition.y - pos.y);
            Camera.main.transform.Translate(new Vector3(dir.x * 0.001f, 0, dir.z * 0.001f));

            
            
        }
        pos = Input.mousePosition;
    }
}
