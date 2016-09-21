using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    void Awake()
    {
        if (!Instance) Instance = this;
    }

    void Start()
    {
        // open first scene
    }

    public List<SceneItem> sceneList = new List<SceneItem>();
    public SceneItem currentScene;
    public SceneItem defaultScene;
    public bool isCombatMap;

    public void GoToScene(string sceneName)
    {
        SceneItem chosenScene = defaultScene;
        foreach (SceneItem si in sceneList)
        {
            if (si.mapName == sceneName)
            {
                chosenScene = si;
                break;
            }
        }


        if (chosenScene == currentScene) return;
        else
        {
            // if exiting headquarters, save the layout
            if (currentScene.mapName == "Headquarters")
            {
                SaveHeadquartersLayout();
            }

            currentScene = chosenScene;
        }

        // load mapDataFile and open all panelsToOpen
        MapManager.Instance.LoadMapLayout(chosenScene.mapDataFile);
        FadeInPanels(chosenScene.panelsToOpen);
        isCombatMap = chosenScene.isCombatMap;

        // if coming back to HQ from battle
        if (chosenScene.mapName == "Headquarters") Invoke("ReturnSurvivingTroopsToCamp", 0.1f);
    }

    public void ReturnSurvivingTroopsToCamp()
    {
        // reset camp slot
        TroopsManager.Instance.availableCampSlot = 0;

        foreach (GameObject c in TroopsManager.Instance.campList)
        {
            c.GetComponent<CampScript>().campedTroops.Clear();
            TroopsManager.Instance.availableCampSlot += c.GetComponent<CampScript>().availableSlot;
        }

        foreach (GameObject t in TroopsManager.Instance.survivingTroops)
        {
            int x = t.transform.GetChild(0).GetComponent<TroopScript>().campIndex;
            TroopsManager.Instance.campList[x].GetComponent<CampScript>().addTroops(t);
            TroopsManager.Instance.availableCampSlot--;
        }

        TroopsManager.Instance.survivingTroops.Clear();
        TroopsManager.Instance.UpdateTroops();
    }

    public MapData headquartersDataFile;

    public void SaveHeadquartersLayout()
    {
        headquartersDataFile.mapEntities.Clear();

        foreach (GridEntity ge in gridScript.Instance.entities)
        {
            headquartersDataFile.mapEntities.Add(MapEntity.CreateFromGrid(ge));
        }
    }

    public void GoToScene(int index)
    {

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

    public void FadeInPanels(List<CanvasGroup> cgs)
    {
        foreach (CanvasGroup cg in cgs)
        {
            FadePanel(cg, true);
        }

        // fade out others
        foreach (CanvasGroup c in GetComponentsInChildren<CanvasGroup>())
        {
            if (cgs.Contains(c)) continue;
            if (cgs.Find(x => x.name == "transparent")) FadePanel(c, false, true);
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

[System.Serializable]
public class SceneItem
{
    public string mapName;
    public List<CanvasGroup> panelsToOpen = new List<CanvasGroup>();
    public bool isCombatMap;
    public MapData mapDataFile;
}