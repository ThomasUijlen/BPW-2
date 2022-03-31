using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHandler : MonoBehaviour
{
    public GameObject startButtons;
    public GameObject moveButtons;
    public GameObject abilityBar;
    public GameObject castDirectionButtons;
    public GameObject inventory;
    public GameObject cancelButton;

    // private void OnEnable() {
    //     ShowAll();
    //     HideAll();
    //     ShowStartButtons();
    // }

    // private void OnDisable() {
    //     HideAll();
    // }

    public void HideAll() {
        startButtons.SetActive(false);
        moveButtons.SetActive(false);
        castDirectionButtons.SetActive(false);
        inventory.SetActive(false);
        cancelButton.SetActive(false);
        abilityBar.SetActive(false);
    }

    public void ShowAll() {
        startButtons.SetActive(true);
        moveButtons.SetActive(true);
        castDirectionButtons.SetActive(true);
        inventory.SetActive(true);
        cancelButton.SetActive(true);
        abilityBar.SetActive(true);
    }

    public void ShowMoveButtons() {
        startButtons.SetActive(false);
        moveButtons.SetActive(true);
        cancelButton.SetActive(true);
    }

    public void showAbilityBar() {
        startButtons.SetActive(false);
        abilityBar.SetActive(true);
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
