using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gridScript : MonoBehaviour
{

    public Vector3[][] gridNodes = new Vector3[44][];
    public GridObject[][] gridTiles = new GridObject[44][];

    public static gridScript Instance;
    public List<GridEntity> entities = new List<GridEntity>();

    void Awake()
    {
        if (!Instance) Instance = this;

        for (int step = 0; step < 44; step++)
        {
            gridNodes[step] = new Vector3[44];
        }
        for (int step = 0; step < 44; step++)
        {
            gridTiles[step] = new GridObject[44];
        }

        InitNodes();
        InitTiles();


    }

    void InitNodes()
    {
        for (int x = 0; x < 44; x++)
            for (int y = 0; y < 44; y++)
            {
                gridNodes[x][y] = new Vector3(0.2f * x - 4.4f, 0.1f, 0.2f * y - 4.4f);
            }
    }

    void InitTiles()
    {
        int countX = -1;
        int countY = -1;

        foreach (Vector3[] TileRow in gridNodes)
        {
            countY = -1;
            countX++;
            foreach (Vector3 TileCol in TileRow)
            {
                countY++;

                if (gridTiles[countX][countY] != null) continue;

                // create the tiles (grid object)
                GridObject g = GridObject.CreateComponent(tileBase, TileCol, countX, countY);
                gridTiles[countX][countY] = g;
                g.transform.parent = transform;

                // add to list
                allTiles.Add(g);
            }
        }
    }

    public List<GridObject> allTiles = new List<GridObject>();

    public Vector3 GetNode(int x, int y)
    {
        return gridNodes[x][y];
    }

    public GridObject GetTile(int x, int y)
    {
        return gridTiles[x][y];
    }

    public GridObject TileLookup(GridObject grd, int right, int down)
    {
        int x = Mathf.Clamp(grd.Index[0] + right, 0, 43);
        int y = Mathf.Clamp(grd.Index[1] + down, 0, 43);

        return gridTiles[x][y];
    }

    public GridObject StrictTileLookup(GridObject grd, int right, int down)
    {
        int x = grd.Index[0] + right;
        int y = grd.Index[1] + down;

        if (x > 43 | x < 0 | y > 43 | y < 0)
        {
            return null;
        }

        return gridTiles[x][y];
    }

    public Vector3[] TileList;
    public GameObject tileBase;
    public GameObject tileObj;
    public GameObject entBase;

    // Use this for initialization
    void Start()
    {
        ToggleGridCursor(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OccupyTile(GridObject grd, bool occupy = true)
    {

    }

    public void OccupyTile(int x, int y, bool occupy = true)
    {
        GridObject g = GetTile(x, y);
        OccupyTile(g, occupy);
    }

    public GridEntity MakeEntity(int sizeX, int sizeY, int x = 21, int y = 21)
    {
        if (x > 43) x = 43;
        if (x < 0) x = 0;
        if (y > 43) y = 43;
        if (y < 0) y = 0;

        return gridTiles[x][y].MakeEntityHere(sizeX, sizeY, false);
    }

    public GridEntity MakeBlueprint(int sizeX, int sizeY)
    {
        return gridTiles[21][21].MakeEntityHere(sizeX, sizeY, true);
    }

    public void ToggleGridCursor(bool active)
    {
        Renderer renderer;
        Color color;
        foreach (GridObject g in gridScript.Instance.allTiles)
        {
            renderer = g.GetComponent<Renderer>();
            color = renderer.material.color;
            color.a = active ? 0.3f : 0;
            renderer.material.color = color;
        }
    }

    public void ResetGridCursor()
    {
        Renderer renderer;
        Color color;
        foreach (GridObject g in gridScript.Instance.allTiles)
        {
            renderer = g.GetComponent<Renderer>();
            color = Color.yellow; // default, yellow
            color.a = 0;
            renderer.material.color = color;
        }
    }

    public void UpdateGridCursor()
    {
        Renderer renderer;
        foreach (GridObject g in gridScript.Instance.allTiles)
        {
            renderer = g.GetComponent<Renderer>();
            if (!renderer) continue;

            Color color = Color.yellow; // default, yellow
            color.a = 0.3f;

            // occupied by building
            if (g.gridChild)
            {
                color.a = 0.5f;
            }

            // new building
            if (g.gridBlueprint)
            {
                color = Color.green;
                color.a = 0.5f;
            }

            // cannot build here
            if (g.gridBlueprint && g.gridChild)
            {
                color = Color.red;
                color.a = 0.5f;
            }

            if (g.entity == inputManager.Instance.selectedEntity)
            {
                color = Color.green;
                color.a = 0.5f;
            }

            renderer.material.color = color;
        }
    }

    public void LateGenerateNavMesh()
    {
        StartCoroutine(NavMeshCoroutine());
    }

    IEnumerator NavMeshCoroutine()
    {
        yield return new WaitForEndOfFrame();
        DefenderEnvironment.Instance.GenerateNavmesh();
    }
}

public abstract class GridAbstract : MonoBehaviour, ISnappable, IArrangeable
{
    public virtual int[] Index { get; set; }

    public virtual bool SnapTo(GridObject other)
    {
        return false;
    }

    public bool IsOccupied()
    {
        return (gridChild != null);
    }

    public void DestroyChild()
    {
        if (gridChild)
        {
            //Destroy(gridChild);
            PoolManager.Instance.ReturnToPool(gridChild);
            gridChild = null;
        }
    }

    public void DestroyBlueprint()
    {
        if (gridBlueprint)
        {
            Destroy(gridBlueprint);
            gridBlueprint = null;
        }
    }

    public void Orphan(bool isBlueprint = false)
    {
        if (isBlueprint) blueprint = null;
        else entity = null;
    }

    public void GiveChild(GameObject go, bool blueprint = false)
    {
        if (blueprint)
        {
            gridBlueprint = go;
            //go.transform.position = transform.position;
            //go.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            gridChild = go;
            //go.transform.position = transform.position;
            //go.GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    public bool GiveChildTo(GridObject grd, bool duplicate = false)
    {
        if (grd == null | grd.gridChild == null)
        {
            grd.GiveChild(gridChild);
            if (!duplicate) gridChild = null;
            grd.entity = entity;
            if (!duplicate) entity = null;
            return true;
        }

        return false;
    }

    public GameObject gridChild;
    public GameObject gridBlueprint;
    public GameObject entity;
    public GameObject blueprint;
    public GameObject BlueprintOrEntity
    {
        get
        {
            if (entity) return entity;
            else return blueprint;
        }
    }
}

public class GridObject : GridAbstract
{
    public GridObject(int posX = 0, int posY = 0)
    {
        // only need position coz the entity will be resizeable
        Initialize(posX, posY);
    }

    public void Initialize(int posX = 0, int posY = 0)
    {
        //if (Random.Range(0, 100) > 98) GiveChild((GameObject)Instantiate(gridScript.Instance.tileObj, Vector3.zero, Quaternion.identity));
        Index = new int[2] { posX, posY };
    }

    public override bool SnapTo(GridObject other)
    {
        if (gridBlueprint || gridChild)
        {
            //if (GiveChildTo(other))
            if (blueprint && blueprint.GetComponent<GridEntity>().ParallelHoverOnGrid(this, other))
                return true;
            else if (entity && entity.GetComponent<GridEntity>().ParallelMoveOnGrid(this, other))
                return true;
        }
        return false;
    }

    void Start()
    {
        //if (Random.Range(0, 100) > 98 && gridChild == null) MakeEntityHere();
        //if (Index[0] == 43 || Index[1] == 43) MakeEntityHere();
    }

    public static GridObject CreateComponent(GameObject go, Vector3 realPos, int posX = 0, int posY = 0)
    {
        GameObject g = (GameObject)Instantiate(go, realPos, Quaternion.identity);
        GridObject init = g.AddComponent<GridObject>();
        //init = new GridObject(posX, posY, sizeX, sizeY);
        init.Initialize(posX, posY);
        g.name = "GridObject " + posX.ToString() + "." + posY.ToString();

        return init;
    }

    public GridEntity MakeEntityHere(int sizeX, int sizeY, bool isBlueprint = false)
    {
        GridEntity ge = GridEntity.CreateComponent(
            gridScript.Instance.tileObj,
            Index[0],
            Index[1],
            sizeX,
            sizeY,
            isBlueprint
            );

        return ge;
    }

    public GridEntity MakeRandomEntityHere()
    {
        GridEntity ge = GridEntity.CreateComponent(
            gridScript.Instance.tileObj,
            Index[0],
            Index[1],
            UnityEngine.Random.Range(1, 4),
            UnityEngine.Random.Range(1, 4)
            );

        return ge;
    }

    public bool stackedChild;
}

public class GridEntity : MonoBehaviour, IArrangeable, IDimensions
{
    public virtual int[] Index { get; set; }
    int columns;
    int rows;

    public List<GridObject> anchors = new List<GridObject>();
    public GridObject MainAnchor
    {
        get
        {
            if (anchors.Count <= 0)
            {
                GridObject gr = gridScript.Instance.GetTile(Index[0], Index[1]);
                anchors.Add(gr);
                return gr;
            }
            else
            {
                return gridScript.Instance.GetTile(anchors[0].Index[0], anchors[0].Index[1]);
            }
        }
        set
        {
            if (anchors.Contains(value)) anchors.Remove(value);
            anchors.Insert(0, value);
        }
    }

    int tempX;
    int tempY;

    void DelayedInit(int x, int y)
    {
        tempX = x;
        tempY = y;
    }

    public void Start()
    {
        if (!isBlueprint)
        {
            Root(true);
            RespawnAnchors(true);
        }
        else
        {
            Root(false);
            ProjectAnchors();
        }

        if (avatar.GetComponent<BuildingScript>().targetModule != null) avatar.GetComponent<BuildingScript>().targetModule.MakeTargetNodes();

        avatar.GetComponent<BuildingScript>().destroyEvt += AvatarHandler;
    }

    void AvatarHandler(GameObject g)
    {
        Destroy(gameObject);
    }

    public void Update()
    {
        CenterAvatar();
    }

    public void Initialize(int posX = 0, int posY = 0, int sizeX = 1, int sizeY = 1, bool isBlueprint = false)
    {
        Index = new int[2] { posX, posY };
        Resize(sizeX, sizeY);
        this.isBlueprint = isBlueprint;
    }

    static int entityCount;
    public bool isBlueprint;

    public static GridEntity CreateComponent(GameObject go, int posX = 0, int posY = 0, int sizeX = 1, int sizeY = 1, bool isBlueprint = false)
    {
        GameObject g = (GameObject)Instantiate(go, Vector3.one * 10f, Quaternion.identity);
        GridEntity init = g.AddComponent<GridEntity>();
        //init = new GridObject(posX, posY, sizeX, sizeY);
        init.Initialize(posX, posY, sizeX, sizeY, isBlueprint);
        //init.DelayedInit(sizeX, sizeY);
        g.name = "Entity" + entityCount++.ToString();

        gridScript.Instance.entities.Add(init);

        return init;
    }

    public GridObject GetAnchorTile(int x, int y)
    {
        return null;
    }

    public List<GridObject> GetAnchorTiles()
    {
        return null;
    }

    public bool ParallelMoveOnGrid_old(GridObject target)
    {
        // first check if all the anchors can move
        for (int i = target.Index[0]; i == target.Index[0] + Rows - 1; i++)
            for (int j = target.Index[1]; j == target.Index[1] + Columns - 1; j++)
            {
                if (gridScript.Instance.gridTiles[i][j].gridChild)
                {
                    if (gridScript.Instance.gridTiles[i][j].gridChild == this) continue;
                    else return false;
                }
            }

        // then move the anchors one by one

        //if (GiveChildTo(target)) return true;
        return false;
    }

    public bool gridPosChanged = false;

    public bool ParallelMoveOnGrid(GridObject start, GridObject target)
    {
        // where the destination tile is relative to MainAnchor
        int deltaX = target.Index[0] - MainAnchor.Index[0];
        int deltaY = target.Index[1] - MainAnchor.Index[1];

        // project the new main anchor and check the adjacent tiles
        bool canMove = true;

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                GridObject g = gridScript.Instance.StrictTileLookup(target, c, r);
                if (g == null) return false;
                if (!anchors.Contains(g) && g.IsOccupied()) canMove = false;
            }
        }

        if (canMove)
        {
            //gridPosChanged = false; (moved to inputreceiver OnPressed)
            if (Index != target.Index) gridPosChanged = true;
            
            Index = target.Index;
            RespawnAnchors();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ParallelHoverOnGrid(GridObject start, GridObject target)
    {
        // where the destination tile is relative to MainAnchor
        int deltaX = target.Index[0] - MainAnchor.Index[0];
        int deltaY = target.Index[1] - MainAnchor.Index[1];

        // project the new main anchor and check the adjacent tiles
        bool canMove = true;

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                GridObject g = gridScript.Instance.StrictTileLookup(target, c, r);
                if (g == null) return false;
            }
        }

        if (canMove)
        {
            Index = target.Index;
            ProjectAnchors();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool MoveOnGrid(int right, int down)
    {
        if (MainAnchor.GiveChildTo(gridScript.Instance.TileLookup(MainAnchor, right, down)))
            return true;

        return false;
    }

    public void ProjectAnchors()
    {
        DeactivateAnchors(true);

        // which is the anchore
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                GridObject g = gridScript.Instance.TileLookup(MainAnchor, c, r);
                g.GiveChild((GameObject)Instantiate(gridScript.Instance.tileObj, Vector3.zero, Quaternion.identity), true);
                g.blueprint = gameObject;
                // except MainAnchor
                if (g != MainAnchor) anchors.Add(g);

                //parent
                g.gridBlueprint.transform.parent = g.blueprint.transform;
            }
        }

        gridScript.Instance.UpdateGridCursor();
    }

    public void RespawnAnchors(bool init = false)
    {
        if (init == false)
        {
            DeactivateAnchors();
        }

        // which is the anchore
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                GridObject g = gridScript.Instance.TileLookup(MainAnchor, c, r);
                //g.GiveChild((GameObject)Instantiate(gridScript.Instance.tileObj, Vector3.zero, Quaternion.identity));
                g.GiveChild(PoolManager.Instance.GetFromPool());
                g.entity = gameObject;
                // except MainAnchor
                if (g != MainAnchor) anchors.Add(g);
#if UNITY_EDITOR
                //parent
                //g.gridChild.transform.parent = g.entity.transform;
#endif
            }
        }

        gridScript.Instance.UpdateGridCursor();
        //if (gridPosChanged) StartCoroutine(LateGenerateNavMesh());
    }

    public void DeactivateAnchors(bool isBlueprint = false)
    {
        foreach (GridObject g in anchors)
        {
            if (isBlueprint) g.DestroyBlueprint();
            else g.DestroyChild();
            g.Orphan(isBlueprint);
        }

        anchors.Clear();
        gridScript.Instance.ResetGridCursor();
    }

    public bool Root(bool rootThis = true)
    {
        if (rootThis)
        {
            // try to root his entity in place by checking if
            // there is another entity underneath

            foreach (GridObject g in anchors)
            {
                if (g.gridChild) return false;
            }

            foreach (GridObject g in anchors)
            {
                g.DestroyBlueprint();
                g.Orphan(true);
                g.GiveChild((GameObject)Instantiate(gridScript.Instance.tileObj, Vector3.zero, Quaternion.identity));
                g.entity = gameObject;
                g.gridChild.transform.parent = g.entity.transform;
            }

            isBlueprint = false;
            gridScript.Instance.ToggleGridCursor(false);
            gridScript.Instance.LateGenerateNavMesh();
        }

        if (!rootThis)
        {
            // unroot

            foreach (GridObject g in anchors)
            {
                if (g.entity != this) continue;
                g.DestroyChild();
                g.Orphan();
                g.GiveChild((GameObject)Instantiate(gridScript.Instance.tileObj, Vector3.zero, Quaternion.identity), true);
                g.blueprint = gameObject;
                g.gridBlueprint.transform.parent = g.blueprint.transform;
            }

            isBlueprint = true;
            gridScript.Instance.ToggleGridCursor(true);
        }

        return true;
    }

    public void ToggleRoot()
    {
        var a = isBlueprint ? Root(true) : Root(false);
    }

    public virtual void Resize_old(int x, int y)
    {
        GridObject g = gridScript.Instance.TileLookup(MainAnchor, x, y);

        print(g.Index[0] + " " + MainAnchor.Index[0] + " " + x);
        print(g.Index[1] + " " + MainAnchor.Index[1] + " " + y);

        Columns = g.Index[0] - MainAnchor.Index[0] + x;
        Rows = g.Index[1] - MainAnchor.Index[1] + y;
    }

    public virtual void Resize(int x, int y)
    {
        GridObject g = gridScript.Instance.TileLookup(MainAnchor, x, y);

        //print(g.Index[0] + " " + MainAnchor.Index[0] + " " + x);
        //print(g.Index[1] + " " + MainAnchor.Index[1] + " " + y);

        columns = Mathf.Clamp(g.Index[0] - MainAnchor.Index[0], 1, 44);
        rows = Mathf.Clamp(g.Index[1] - MainAnchor.Index[1], 1, 44);
    }

    void OnDestroy()
    {
        foreach (GridObject a in anchors)
        {
            if (isBlueprint) a.DestroyBlueprint();
            else a.DestroyChild();

            a.Orphan(isBlueprint);
        }

        if (avatar)
        {
            avatar.GetComponent<BuildingScript>().destroyEvt -= AvatarHandler;
            Destroy(avatar);
        }

        if (isBlueprint && inputManager.Instance.selectedEntity) gridScript.Instance.UpdateGridCursor();
        else if (isBlueprint) gridScript.Instance.ToggleGridCursor(false);
    }

    public virtual int Columns
    {
        get
        {
            return columns;
        }
        set
        {
            Resize(rows, value);
            RespawnAnchors();
        }
    }

    public virtual int Rows
    {
        get
        {
            return rows;
        }
        set
        {
            Resize(value, columns);
            RespawnAnchors();
        }
    }

    public virtual int TopLeft
    {
        get;
        set;
    }
    public virtual int TopRight
    {
        get;
        set;
    }
    public virtual int BottomLeft
    {
        get;
        set;
    }
    public virtual int BottomRight
    {
        get;
        set;
    }

    public GameObject avatar;

    public void CenterAvatar()
    {
        Vector3 midPos = Vector3.Lerp(MainAnchor.transform.position, gridScript.Instance.TileLookup(MainAnchor, Rows - 1, Columns - 1).transform.position, 0.5f);
        if (avatar)
        {
            avatar.transform.position = midPos;
            avatar.transform.parent = transform;
        }
    }
}

public interface ISnappable
{
    bool SnapTo(GridObject other);
}

interface IArrangeable
{
    int[] Index { get; set; }
}

interface IDimensions
{
    int Columns { get; set; }
    int Rows { get; set; }
    int TopLeft { get; }
    int TopRight { get; }
    int BottomLeft { get; }
    int BottomRight { get; }
    void Resize(int x, int y);
}