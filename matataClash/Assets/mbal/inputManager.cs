using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class inputManager : MonoBehaviour
{
    public static inputManager Instance;

    void Awake()
    {
        if (!Instance) Instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    void ReceiveRaycast(RaycastHit hit)
    {
        //
    }

    public LayerMask touchInputMask;
    private List<GameObject> touchList = new List<GameObject>();
    public GameObject[] touchesOld;
    public RaycastHit hit;
    public RaycastHit oldHit;


    void DraggingPhase(GridObject go)
    {
        if (SceneManager.Instance.isCombatMap) return;
        GridObject g = oldHit.transform.GetComponent<GridObject>();
        if (g && g.blueprint)
        {
            GridEntity ge = g.blueprint.GetComponent<GridEntity>();
            BuildingScript bs = ge.avatar.GetComponent<BuildingScript>();
            if (ge == selectedEntity && bs & !bs.isBuilding)
            {
                if (g.SnapTo(go))
                {
                    oldHit = hit;
                }
                else isDraggingPhase = false;
            }
        }

        else if (g && g.entity)
        {
            GridEntity ge = g.entity.GetComponent<GridEntity>();
            BuildingScript bs = ge.avatar.GetComponent<BuildingScript>();
            if (ge == selectedEntity && bs & !bs.isBuilding)
            {
                if (g.SnapTo(go))
                {
                    oldHit = hit;
                }
                else isDraggingPhase = false;
            }
        }
    }

    public GridEntity selectedEntity;

    public bool isDraggingPhase;
    public Transform hitTrans;
    public Transform oldHitTrans;
    RaycastHit firstHit;
    void Update()
    {

        hitTrans = hit.transform;
        oldHitTrans = oldHit.transform;

        if (isDraggingPhase && hit.transform && oldHit.transform)
        {
            DraggingPhase(hit.transform.GetComponent<GridObject>());
        }
        else
        {
            isDraggingPhase = false;
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(ray1.origin, ray1.direction, Color.yellow);

        if (Physics.Raycast(ray1, out hit, Mathf.Infinity, touchInputMask))
        {
            IClickable clicked = (IClickable)hit.transform.GetComponent<IClickable>();
            //if (clicked == null) return;
            if (clicked != null)
            {
                Debug.DrawLine(ray1.origin, hit.point);
                clicked.OnHover();
                // show active blocks here
            }

            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                touchesOld = new GameObject[touchList.Count];
                touchList.CopyTo(touchesOld);
                touchList.Clear();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

                if (Input.GetMouseButtonDown(0))
                {
                    if (!isDraggingPhase)
                    {
                        oldHit = hit;
                        firstHit = oldHit;
                        isDraggingPhase = true;

                        clicked.OnMouseDown();
                    }
                }

                if (Input.GetMouseButton(0))
                {
                    if (isDraggingPhase = true && clicked != null)
                    {
                        clicked.OnPress();
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    isDraggingPhase = false;
                    if (Vector3.Distance(firstHit.point, hit.point) < 0.1f)
                    {
                        clicked.OnClick();
                    }
                    else
                    {
                        clicked.OnMouseUp();
                    }
                }

                // show building options when clicked
            }
        }


#endif

        if (Input.touchCount > 0)
        {
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();

            foreach (Touch touch in Input.touches)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit, touchInputMask))
                {
                    GameObject recipient = hit.transform.gameObject;
                    touchList.Add(recipient);

                    if (touch.phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Stationary)
                    {
                        recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Canceled)
                    {
                        recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

            foreach (GameObject g in touchesOld)
            {
                if (!touchList.Contains(g))
                {
                    g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

    }
}