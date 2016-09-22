using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuButtonAnim : MonoBehaviour
{
    Vector2 oldPos;
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        oldPos = rect.localPosition;
    }

    // Use this for initialization
    void Start()
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - 1000);
		Invoke("SlideIn",1);
    }

    // Update is called once per frame
    void Update()
    {

    }

	void SlideIn()
	{
        StartCoroutine(Animate());
	}

    IEnumerator Animate()
    {
        float newPosY;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 3f)
        {
            newPosY = Mathf.Lerp(rect.anchoredPosition.y, oldPos.y, t);
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, newPosY);
            yield return null;
        }
    }
}
