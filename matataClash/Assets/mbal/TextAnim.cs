using UnityEngine;
using System.Collections;

public class TextAnim : MonoBehaviour
{
    public string txt = "^";
    public float fadeDelay = 0;
    public float fadeTime = 2;
    public Color txtColor = Color.white;
    public int txtSize = 10;
    public float offsetY = 0;
    public float moveDist = 0;

    UnityEngine.UI.Text component;

    void Awake()
    {
        component = GetComponentInChildren<UnityEngine.UI.Text>();
    }

    // Use this for initialization
    void Start()
    {
        component.color = txtColor;
        component.text = txt;
        component.fontSize = txtSize;
        StartCoroutine(BeginFade());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BeginFade()
    {
        Color newColor;
        float newPosY;

        yield return new WaitForSeconds(fadeDelay);

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            newColor = new Color(txtColor.r, txtColor.g, txtColor.b, Mathf.Lerp(component.color.a, 0, t));
            component.color = newColor;
            newPosY = Mathf.Lerp(offsetY, offsetY + moveDist, t);
            print (offsetY + moveDist);
            component.rectTransform.anchoredPosition = new Vector2(component.rectTransform.anchoredPosition.x, newPosY);
            yield return null;
        }

        Destroy(gameObject);
    }
}
