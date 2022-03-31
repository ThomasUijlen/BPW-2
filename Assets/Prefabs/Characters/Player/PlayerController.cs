using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject controls;

    private void Awake() {
        TurnHandler.Refresh();
    }

    public void ShowControls() {
        controls.SetActive(true);
    }

    public void HideControls() {
        controls.SetActive(false);
    }

    public void Dead() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
