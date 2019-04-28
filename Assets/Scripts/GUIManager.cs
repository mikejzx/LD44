using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GUIManager {
    public static GUIManager instance;

    public Text label_health;
    public Text label_wave;
    public Text label_ready;
    public Text label_doublespeed;
    public Text label_instructionmsg, label_instr2;
    public Text label_losescr_kills;
    public Image img_healthfillbar;
    public CanvasGroup cgFade;
    public CanvasGroup cgLoseScreen;
    public CanvasGroup cgSidebar;
    [Space]
    public Button btnQuit;
    public Button btnRestart;
    [Space]
    [SerializeField] private Sprite _missingTexture;
    public static Sprite missingTexture { get { return instance._missingTexture; } }
    [Space]
    public GUITowerModule towers;

    private Coroutine fadeCr = null;
    private Coroutine loseScreenCr = null;
    private GameObject goLoseScreen;

    private static readonly string TEXT_READY = "Wave running...",
        TEXT_UNREADY = "Press 'RETURN' to start the next wave.",
        TEXT_REGULARSPEED = "<color='magenta'>1x</color> speed. (Press 'D' to toggle.)",
        TEXT_DOUBLESPEED = "<color='magenta'>2x</color> speed... (Press 'D' to toggle.)";

    private Color colourInstrTextHidden = new Color (1.0f, 0.0f, 0.0f, 0.0f), 
        colourInstrTextInitial = new Color(0.0f, 1.0f, 0.0f, 1.0f);

    private MonoBehaviour behav;

    public void Initialise (MonoBehaviour behav) {
        // Add button click listeners
        btnQuit.onClick.AddListener(Fn_QuitGame);
        btnRestart.onClick.AddListener(Fn_RestartGame);

        this.behav = behav;
        instance = this;
        towers.Initialise();

        fadeCr = behav.StartCoroutine(StartingFadeCr());
        cgLoseScreen.alpha = 0.0f;
        goLoseScreen = cgLoseScreen.gameObject;
        goLoseScreen.SetActive(false);

        cgSidebar.alpha = 1.0f;
    }

    public static void SetHealth(int newhealth) {
        // Change health label
        instance.label_health.text = "Life: " + newhealth;
        instance.img_healthfillbar.fillAmount = Mathf.Clamp01(newhealth / 300.0f);

        // Set availability based on new health
        instance.towers.UpdateHealth(newhealth);
    }

    public static void SetWave(int newwave) => instance.label_wave.text = "Wave: " + newwave;

    public static void SetReady (bool ready) => instance.label_ready.text = ready ? TEXT_READY : TEXT_UNREADY;

    public static void SetDoubleSpeed (bool doubleS) => instance.label_doublespeed.text = doubleS ? TEXT_DOUBLESPEED : TEXT_REGULARSPEED;

    public static void RemoveInstructionMsg (out bool success) {
        success = false;
        if (instance.fadeCr == null) {
            success = true;
            instance.fadeCr = instance.behav.StartCoroutine(instance.FadeInstructionMsg());
        }
    }

    /// <summary>Display the lose screen</summary>
    public static void LoseScreen () => instance._LoseScreen();

    private void _LoseScreen () {
        if (loseScreenCr != null) { behav.StopCoroutine(loseScreenCr); }
        loseScreenCr = behav.StartCoroutine(LoseScreenCr());
    }

    private IEnumerator LoseScreenCr () {
        string stats = string.Format("You lasted until wave {0}\n\nTotal aliens killed: {1}", Spawner.wave, Player.kills);
        label_losescr_kills.text = stats;

        float startTime = Time.timeScale;
        float t = 0.0f;
        goLoseScreen.SetActive(true);
        cgLoseScreen.alpha = 0.0f;
        cgSidebar.alpha = 1.0f;
        while (t < 1.0f) {
            t = Mathf.Clamp01(t + Time.deltaTime * 4.0f);
            cgLoseScreen.alpha = t;
            cgSidebar.alpha = 1.0f - t;
            Time.timeScale = startTime * (1.0f - t);
            yield return null;
        }
        Time.timeScale = 0.0f;
        cgLoseScreen.alpha = 1.0f;
        cgSidebar.alpha = 0.0f;
    }

    private IEnumerator FadeInstructionMsg () {
        Color a = colourInstrTextInitial,
            b = colourInstrTextHidden;

        float t = 0.0f;
        label_instructionmsg.color = colourInstrTextInitial;
        label_instr2.color = colourInstrTextInitial;
        while (t < 1.0f) {
            t = Mathf.Clamp01(t + Time.deltaTime);
            label_instructionmsg.color = Color.Lerp(a, b, t);
            label_instr2.color = label_instructionmsg.color;
            yield return null;
        }
        label_instructionmsg.color = b;
        label_instr2.color = b;
    }

    /// <summary>Fade a little overlay during game startup</summary>
    private IEnumerator StartingFadeCr() {
        cgFade.alpha = 1.0f;
        yield return new WaitForSeconds(0.2f); // Slight delay
        float t = 0.0f;
        cgFade.alpha = 1.0f;
        while (t < 1.0f) {
            t = Mathf.Clamp01(t + Time.deltaTime);
            cgFade.alpha = 1.0f - t;
            yield return null;
        }
        cgFade.alpha = 0.0f;

        // Fade instruction text in
        Color a = colourInstrTextHidden,
            b = colourInstrTextInitial;
        label_instructionmsg.color = colourInstrTextHidden;
        label_instr2.color = colourInstrTextHidden;
        t = 0.0f;
        while (t < 1.0f) {
            t = Mathf.Clamp01(t + Time.deltaTime * 2.0f);
            label_instructionmsg.color = Color.Lerp(a, b, t);
            label_instr2.color = label_instructionmsg.color;
            yield return null;
        }
        label_instructionmsg.color = b;
        label_instructionmsg.color = b;

       fadeCr = null;
    }

#region BUTTON_FUNCTIONS
    
    /// <summary>To be run from </summary>
    public void Fn_QuitGame () {
        AudioHandler.ClickSound();
        Debug.Log("Quitting");
        Application.Quit();
    }

    /// <summary>Restarts the game</summary>
    public void Fn_RestartGame () {
        // Restart the game...
        AudioHandler.ClickSound();
        Debug.Log("Restarting");
        behav.StartCoroutine(RestartCr());
    }

    private IEnumerator RestartCr () {
        float t = 0.0f;
        while (t < 1.0f) {
            t = Mathf.Clamp01(t + 3.0f * Time.unscaledDeltaTime);
            cgFade.alpha = t;
            cgLoseScreen.alpha = 1.0f - t;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.1f);

        // Was running out of time :P
        // This seems like a *decent* solution.
        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("dummyscene");
    }
    #endregion
}
