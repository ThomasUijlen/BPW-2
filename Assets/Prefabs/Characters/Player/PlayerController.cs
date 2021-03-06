using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject controls;
    public TextMeshPro healthText;

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

    public void healthChanged() {
        healthText.text = GetComponent<CharacterDetails>().currentHealth.ToString();
    }
}
