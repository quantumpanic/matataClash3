using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageActor : MonoBehaviour
{

    public List<Transform> actorStates = new List<Transform>();

    public static StageActor CreateComponent(Vector3 position, Quaternion rotation, string name = "New Actor")
    {
        GameObject go = new GameObject();
        StageActor script = go.AddComponent<StageActor>();

        go.transform.position = position;
        go.transform.rotation = rotation;
        go.name = name;

        return script;
    }

    public enum AnimState
    {
        Idle = 0,
        Walking = 1,
        Pose = 2
    }

    public AnimState animState;

    public void SetAnim(AnimState anim)
    {
        animState = anim;
    }

    public QuerySDMecanimController.QueryChanSDAnimationType testAnimState;
    public QuerySDMecanimController cont;

    public void TestAnim(QuerySDMecanimController.QueryChanSDAnimationType anim)
    {
        cont.ChangeAnimation(anim);
    }

    // Use this for initialization
    void Start()
    {
        cursor = new GameObject();
        cursor.transform.position = transform.position;
        cont = gameObject.GetComponent<QuerySDMecanimController>();
    }

    // Update is called once per frame
    void Update()
    {
        TestAnim(testAnimState);
    }

    void GetTranslation(Vector3 vector)
    {

    }

    void SimulateMovement()
    {
        // using the nodes from each frame
        // try to simulate movement using interpolation

    }

    public void RecordTransform()
    {
        actorStates.Add(transform);
    }

    GameObject cursor;

    void OnTouchDown(Vector3 point)
    {
        transform.LookAt(point);
        transform.position = point;
    }

    void OnTouchUp(Vector3 point)
    {
        // destroy cursor
    }

    void OnTouchStay(Vector3 point)
    {

    }

    void OnTouchExit(Vector3 point)
    {

    }
}
