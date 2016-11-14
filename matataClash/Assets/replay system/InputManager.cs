using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public GameObject testObj;
	Vector2 oldMousePos = Vector2.zero;
	
    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        Vector2 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;

        if (Input.GetMouseButton(0))
        {			
            Vector2 touchDeltaPosition = mousePos - oldMousePos;
			oldMousePos = mousePos;
			
            // Move object across XY plane
            testObj.transform.Translate(-touchDeltaPosition.x/Screen.width, 0, -touchDeltaPosition.y/Screen.height);
        }
		else
		{
			oldMousePos = mousePos;
		}

#endif

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            // Move object across XY plane
            testObj.transform.Translate(-touchDeltaPosition.x * 1, 0, -touchDeltaPosition.y * 1);
        }
    }
}
