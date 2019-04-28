using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This is used in the dummy scene for restarting the game.
    It just immediately loads the main scene.
*/

public class DummyScene : MonoBehaviour {

	private void Start () => UnityEngine.SceneManagement.SceneManager.LoadScene("main");
}
