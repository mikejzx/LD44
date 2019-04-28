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
    public Text label_instructionmsg;
    public Image img_healthfillbar;
    public CanvasGroup fadeCg;
    [Space]
    [SerializeField] private Sprite _missingTexture;
    public static Sprite missingTexture { get { return instance._missingTexture; } }
    [Space]
    public GUITowerModule towers;

    private Coroutine fadeCr = null;

    private static readonly string TEXT_READY = "Wave running...",
        TEXT_UNREADY = "Press 'RETURN' to start the next wave.",
        TEXT_REGULARSPEED = "<color='magenta'>1x</color> speed. (Press 'D' to toggle.)",
        TEXT_DOUBLESPEED = "<color='magenta'>2x</color> speed... (Press 'D' to toggle.)";

    private Color colourInstrTextHidden = new Color (1.0f, 0.0f, 0.0f, 0.0f), colourInstrTextInitial;

    private MonoBehaviour behav;

    public void Initialise (MonoBehaviour behav) {
        this.behav = behav;
        instance = this;
        towers.Initialise();

        colourInstrTextInitial = label_instructionmsg.color;
        fadeCr = behav.StartCoroutine(StartingFadeCr());
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
        if (instance.fadeCr != null) {
            success = true;
            instance.fadeCr = instance.behav.StartCoroutine(instance.FadeInstructionMsg());
        }
    }

    private IEnumerator FadeInstructionMsg () {
        Color a = colourInstrTextInitial,
            b = colourInstrTextHidden;

        float t = 0.0f;
        label_instructionmsg.color = colourInstrTextInitial;
        while(t < 1.0f) {
            t = Mathf.Clamp01(t + Time.deltaTime);
            label_instructionmsg.color = Color.Lerp(a, b, t);
            yield return null;
        }
        label_instructionmsg.color = b;
    }

    /// <summary>Fade a little overlay during game startup</summary>
    private IEnumerator StartingFadeCr() {
        yield return new WaitForSeconds(0.2f); // Slight delay
        float t = 0.0f;
        fadeCg.alpha = 1.0f;
        while (t < 1.0f) {
            t = Mathf.Clamp01(t + Time.deltaTime);
            fadeCg.alpha = 1.0f - t;
            yield return null;
        }
        fadeCg.alpha = 0.0f;

        // Fade instruction text in
        Color a = colourInstrTextHidden,
            b = colourInstrTextInitial;
        label_instructionmsg.color = colourInstrTextInitial;
        t = 0.0f;
        while (t < 1.0f) {
            t = Mathf.Clamp01(t + Time.deltaTime);
            label_instructionmsg.color = Color.Lerp(a, b, t);
            yield return null;
        }
        label_instructionmsg.color = b;
    }
}
