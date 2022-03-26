using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHandler : MonoBehaviour
{
    public GameObject startButtons;
    public GameObject moveButtons;
    public GameObject castDirectionButtons;
    public GameObject inventory;
    public GameObject cancelButton;

    public void HideAll() {
        startButtons.SetActive(false);
        moveButtons.SetActive(false);
        castDirectionButtons.SetActive(false);
        inventory.SetActive(false);
        cancelButton.SetActive(false);
    }

    public void ShowMoveButtons() {
        startButtons.SetActive(false);
        moveButtons.SetActive(true);
        cancelButton.SetActive(true);
    }

    public void ShowCastDirectionButtons() {
        startButtons.SetActive(false);
        castDirectionButtons.SetActive(true);
        cancelButton.SetActive(true);
    }

    public void ShowInventoryButtons() {
        startButtons.SetActive(false);
        inventory.SetActive(true);
        cancelButton.SetActive(true);
    }

    public void ShowStartButtons() {
        HideAll();
        startButtons.SetActive(true);
    }
}
