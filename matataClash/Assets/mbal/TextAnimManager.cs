using UnityEngine;
using System.Collections;

public class TextAnimManager : MonoBehaviour
{

    public static TextAnimManager Instance;
    public GameObject textAnimBase;
    public Animator contentPanel;

    void Awake()
    {
        if (!Instance) Instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public TextAnim SpawnFloatingText(Vector3 pos, string text, int size = 20, Color col = default(Color), float distance = 10f, float fadeTime = 3)
    {
        // spawn a floating text on the scene
        GameObject go = (GameObject)Instantiate(textAnimBase, pos, Quaternion.Euler(0, 45, 0));
        TextAnim ta = go.GetComponent<TextAnim>();
        ta.txt = text;
        ta.txtSize = size;
        ta.txtColor = col;
        ta.offsetY = 50;
        ta.moveDist = distance;
        ta.fadeTime = fadeTime;

        return ta;
    }

    public TextAnim SpawnStaticText(Vector3 pos, string text, int size = 50, Color col = default(Color), float fadeTime = 5)
    {
        // show static text on the screen
        GameObject go = (GameObject)Instantiate(textAnimBase, pos, Quaternion.identity);
        TextAnim ta = go.GetComponent<TextAnim>();
        ta.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        ta.GetComponentInChildren<UnityEngine.UI.Text>().rectTransform.sizeDelta = new Vector2(1000,500);
        ta.txt = text;
        ta.txtSize = size;
        ta.txtColor = col;
        ta.fadeTime = fadeTime;
        ta.fadeDelay = 3;

        return ta;
    }

    public TextAnim SpawnAddGoldText(Vector3 pos, string amt)
    {
        return SpawnFloatingText(pos, "+" + amt, 10, Color.yellow);
    }

    public TextAnim SpawnAddManaText(Vector3 pos, string amt)
    {
        return SpawnFloatingText(pos, "+" + amt, 10, Color.blue);
    }

    public TextAnim WarningNoWorker()
    {
        return SpawnStaticText(Vector2.zero, "Not Enough Workers!", 50, Color.red);
    }

    public TextAnim WarningCampFull()
    {
        return SpawnStaticText(Vector2.zero, "Camp is Full!", 50, Color.red);
    }

    public TextAnim WarningNoMana()
    {
        return SpawnStaticText(Vector2.zero, "Insufficient Mana!", 50, Color.red);
    }

    public TextAnim WarningNoGold()
    {
        return SpawnStaticText(Vector2.zero, "Insufficient Gold!", 50, Color.red);
    }

    public TextAnim CustomWarning(string message, Color col)
    {
        return SpawnStaticText(Vector2.zero, message, 50, col);
    }

    public void ToggleMenu() {
        bool isHidden = contentPanel.GetBool("isHidden");
        contentPanel.SetBool("isHidden", !isHidden);
    }
}