using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recorder : MonoBehaviour
{

    public static Recorder Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        testPlayback = true;
        //ToggleTestRec();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (testPlayback)
            {
                TestPlayback();
            }
            else
            {
                TestRec();
            }

            if (isRecording)
            {
                // rec here
            }
        }
    }

    public bool testPlayback;
    public move testActor;
    List<byte> playbackReel = new List<byte>();

    public void ToggleTestRec_old()
    {
        if (testPlayback)
        {
            playbackReel.Clear();
            testPlayback = false;
            testActor.BeginSimulation();
        }
        else
        {
            testActor.BeginRecording();
            testPlayback = true;
        }
    }

    public void ToggleTestRec()
    {
        if (testPlayback)
        {
            floatArrays.Clear();
            _curArray = NewFloatArray();
            floatArrays.Add(_curArray);
            testPlayback = false;
        }
        else
        {
            _curArray = floatArrays[0];
            _curArrayIndex = -1; // reads the floats from the beginning again
            testPlayback = true;
        }
    }

    public void StartRec()
    {
        if (!testPlayback)
            PauseOrResume();
        else
        {
            isActive = true;
            testPlayback = false;
            
            floatArrays.Clear();
            _curArray = NewFloatArray();
            floatArrays.Add(_curArray);
            _curArrayIndex = -1; // reads the floats from the beginning again
            
            totalFrames = 0;
            curFrame = 0;
        }
    }

    public void StartPlayback()
    {
        if (testPlayback)
            PauseOrResume();
        else
        {
            isActive = true;
            testPlayback = true;
            
            _curArray = floatArrays[0];
            _curArrayIndex = -1; // reads the floats from the beginning again
            
            curFrame = 0;
        }
    }

    public void PauseOrResume()
    {
        isActive = isActive ? false : true;
    }

    void FloatsToPositions()
    {

    }

    void PositionsToFloats()
    {

    }

    void TestRec()
    {
        // happens every frame that recording is active.
        // record movement of all active actors by
        // reading each actor's current position and
        // translate to x,y,z floats in current array.
        // if array exhausted then create new array


        for (int x = 0; x <= ActorManager.Instance.actors.Count - 1; x++)
        {
            WriteNext = ActorManager.Instance.actors[x].transform.position.x;
            WriteNext = ActorManager.Instance.actors[x].transform.position.y;
            WriteNext = ActorManager.Instance.actors[x].transform.position.z;
            WriteNext = (float)ActorManager.Instance.actors[x].testAnimState;
        }

        totalFrames++;

        /*
        foreach (StageActor sa in ActorManager.Instance.actors)
        {
            CurrentWriteArray[incrementArrayIndex] = sa.transform.position.x;
            CurrentWriteArray[incrementArrayIndex] = sa.transform.position.y;
            CurrentWriteArray[incrementArrayIndex] = sa.transform.position.z;
        }
        */
    }

    void TestPlayback()
    {
        // reads all the floats and puts them into
        // the values of each actor's transform in
        // corresponding order based on which actor was
        // recorded first

        for (int x = 0; x <= ActorManager.Instance.actors.Count - 1; x++)
        {
            ActorManager.Instance.actors[x].transform.position = new Vector3(
                ReadNext,
                ReadNext,
                ReadNext
            );
            ActorManager.Instance.actors[x].testAnimState = (QuerySDMecanimController.QueryChanSDAnimationType)ReadNext;
        }
        /*
        foreach (StageActor sa in ActorManager.Instance.actors)
        {
            sa.transform.position = new Vector3(
                CurrentReadArray[incrementArrayIndex],
                CurrentReadArray[incrementArrayIndex],
                CurrentReadArray[incrementArrayIndex]
            );
        }
        */

        curFrame++;
        if (curFrame >= totalFrames)
        {
            curFrame = 0;
            _curArrayListIndex = 0;
            _curArray = floatArrays[_curArrayListIndex];
            _curArrayIndex = -1;
        }
    }

    // keep all transform values in arrays.
    // for each actor, the last array has an index
    // of the last recorded float

    public List<float[]> floatArrays = new List<float[]>();
    int maxArraySize = 4096;

    int _curArrayIndex = -1;
    int incrementArrayIndex
    {
        get
        {
            _curArrayIndex++;
            return _curArrayIndex;
        }
    }

    public int curFrame;
    public int totalFrames;
    float[] _curArray;
    float[] CurrentWriteArray // when recording
    {
        get
        {
            if (_curArrayIndex >= maxArraySize - 1)
            {
                _curArray = NewFloatArray();
                floatArrays.Add(_curArray);
                _curArrayIndex = -1;
            }
            return _curArray;
        }
    }

    int _curArrayListIndex;
    float[] CurrentReadArray // when playback
    {
        get
        {
            if (_curArrayIndex >= maxArraySize - 1)
            {
                _curArrayListIndex++;
                if (_curArrayListIndex >= floatArrays.Count) _curArrayListIndex = 0;
                _curArray = floatArrays[_curArrayListIndex];
                _curArrayIndex = -1;
            }
            return _curArray;
        }
    }

    float[] NewFloatArray()
    {
        float[] temp = new float[maxArraySize];
        return temp;
    }

    public bool isActive = false; // is recorder paused?
    public bool isRecording;
    Reel _activeReel;
    Reel activeReel
    {
        get
        {
            if (_activeReel == null)
            {
                _activeReel = NewReel();
            }
            return _activeReel;
        }
    }

    float WriteNext
    {
        set
        {
            CurrentWriteArray[incrementArrayIndex] = value;
        }
    }
    float ReadNext
    {
        get
        {
            return CurrentReadArray[incrementArrayIndex];
        }
    }

    Reel NewReel()
    {
        return new Reel();
    }

    public void Record()
    {
        isActive = true;
        isRecording = true;
    }

    public void Stop()
    {
        isRecording = false;
    }

    void PlaybackReel(Reel reel)
    {
        // begin calling actor manager to
        // use nodes from each frame in reel
        // in order to animate all actors
    }

    void WriteFrame(Reel reel, Frame frame, int frameIndex = -1)
    {
        // overwrites information in a specific frame from a reel
        // or adds a new frame at the end.
        // serializes a data module (containing a state) to byte array
    }

    void WriteEmptyFrame(Reel reel)
    {
        // passes in a blank frame
    }

    void WriteFrameFromCurrentState()
    {

    }

    void SaveStateToData()
    {

    }

    void LoadStateFromData(byte[] stream)
    {
        DataLoader(stream);

        // add a buffer here
    }

    void DataLoader(byte[] stream)
    {
        // reads byte data to create a state

        // should return to a buffer
    }

    void LoadReel(Reel reel)
    {

    }

    Reel SaveReel()
    {
        return null;
    }
}

public class Reel : ScriptableObject
{
    // a set of frames.
    // has a marker indicating current frame0.
    
    int markerIndex;
    int totalFrames;
    List<float[]> floatArrays = new List<float[]>();
    int curFrame;
    int maxArraySize = 4096;

    void ExtendReel()
    {

    }

    float[] NewFloatArray()
    {
        float[] temp = new float[maxArraySize];
        return temp;
    }

    void ResetFrameCounter()
    {
        curFrame = 0;
    }

    int _curArrayIndex = -1;
    int incrementArrayIndex
    {
        get
        {
            _curArrayIndex++;
            return _curArrayIndex;
        }
    }

    float[] _curArray;
    float[] CurrentWriteArray // when recording
    {
        get
        {
            if (_curArrayIndex >= maxArraySize - 1)
            {
                _curArray = NewFloatArray();
                floatArrays.Add(_curArray);
                _curArrayIndex = -1;
            }
            return _curArray;
        }
    }

    int _curArrayListIndex;
    float[] CurrentReadArray // when playback
    {
        get
        {
            if (_curArrayIndex >= maxArraySize - 1)
            {
                _curArrayListIndex++;
                if (_curArrayListIndex >= floatArrays.Count) _curArrayListIndex = 0;
                _curArray = floatArrays[_curArrayListIndex];
                _curArrayIndex = -1;
            }
            return _curArray;
        }
    }
}

public class Frame : ScriptableObject
{
    // one frame contains byte information about
    // states of actors and environment

    byte[] stateData = new byte[0];
}