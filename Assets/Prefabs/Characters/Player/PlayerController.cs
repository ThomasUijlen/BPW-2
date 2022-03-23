using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject controls;


    public void ShowControls() {
        controls.SetActive(true);
    }

    public void HideControls() {
        controls.SetActive(false);
    }
}
