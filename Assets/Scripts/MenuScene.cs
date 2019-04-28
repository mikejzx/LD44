using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour {

    public Button proceed;

    private void Start () {
        proceed.onClick.AddListener(Continue);
    }

    private void Continue() => UnityEngine.SceneManagement.SceneManager.LoadScene("main");
}
