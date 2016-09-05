using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    static SceneManager Instance;

    void Awake()
    {
        if (!Instance) Instance = this;
    }

    void Start()
    {
        // open first scene
    }

    void FadeOutPanel(CanvasGroup cg, bool poof = false)
    {
        FadePanel(cg, false);
    }

    public void FadeInPanel(CanvasGroup cg)
    {
        FadePanel(cg, true);

        // fade out others
        foreach (CanvasGroup c in GetComponentsInChildren<CanvasGroup>())
        {
            if (c == cg) continue;
            if (cg.name == "transparent") FadePanel(c, false, true);
            else FadeOutPanel(c);
        }
    }

    void FadePanel(CanvasGroup cg, bool active, bool poof = false, float fadeTime = 1f)
    {
        StartCoroutine(DoFadePanel(cg, active, poof, fadeTime));
    }

    IEnumerator DoFadePanel(CanvasGroup cg, bool active, bool poof = false, float fadeTime = 1f)
    {
        float start = active ? 0 : 1;
        float finish = active ? 1 : 0;
        finish = poof ? 0 : 1;

        if (active)
        {
            cg.gameObject.SetActive(true);
            cg.transform.SetAsLastSibling();
        }

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            cg.alpha = Mathf.Lerp(start, finish, t);
            yield return null;
        }

        if (!active)
        {
            cg.gameObject.SetActive(false);
        }
    }
}