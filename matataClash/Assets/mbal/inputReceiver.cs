using UnityEngine;
using System.Collections;

public class inputReceiver : TouchInputReceiver
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}

public abstract class TouchInputReceiver : MonoBehaviour, IClickable
{
    public Color matCol;
    void Awake()
    {
        //matCol = GetComponent<Renderer>().material.color;
    }

    public virtual void OnHover()
    {
        //StartCoroutine(YellowWhite());
    }

    public virtual void OnPress()
    {
        //GetComponent<Renderer>().material.color = Color.green;
    }

    public virtual void OnClick()
    {
        try
        {
            inputManager.Instance.selectedEntity = GetComponent<GridObject>().BlueprintOrEntity.GetComponent<GridEntity>();
            gridScript.Instance.UpdateGridCursor();
            foreach (GridEntity ge in gridScript.Instance.entities)
            {
                if (ge == null) continue;
                if (ge != inputManager.Instance.selectedEntity && ge.isBlueprint)
                {
                    Destroy(ge);
                }
            }
        }
        catch (System.Exception)
        {
            foreach (GridEntity ge in gridScript.Instance.entities)
            {
                if (ge == null) continue;
                if (ge.isBlueprint)
                {
                    Destroy(ge);
                    inputManager.Instance.selectedEntity = null;
                }
            }
            gridScript.Instance.ToggleGridCursor(false);
            print("no entity found");
        }
    }

    IEnumerator YellowWhite()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForEndOfFrame();
        GetComponent<Renderer>().material.color = matCol;
    }
}

public interface IClickable
{
    void OnClick();
    void OnPress();
    void OnHover();
}