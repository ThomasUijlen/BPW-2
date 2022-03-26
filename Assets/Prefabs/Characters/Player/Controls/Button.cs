using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent pressed;
    public UnityEvent release;
    internal bool moused = false;
    internal Vector3 targetScale = Vector3.one;
    internal Vector3 originalScale = Vector3.zero;

    private bool justPressed = false;
    private bool justReleased = true;

    private bool justInstanced = true;

    internal void Start() {
        originalScale = transform.localScale;
        targetScale = originalScale;
        transform.localScale = Vector3.zero;

        justInstanced = false;
    }


    private void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime*10);

        if(Input.GetButtonDown("LeftClick")) {
            if(moused) {
                if(!justPressed) {
                    justPressed = true;
                    justReleased = false;
                    pressed.Invoke();
                }
            }
        }

        if(!Input.GetButton("LeftClick")) {
            justPressed = false;

            if(!justReleased) {
                justReleased = true;
                release.Invoke();
            }
        }
    }

    private void OnMouseEnter() {
        moused = true;
        targetScale = new Vector3(1.1f,1.1f,1.1f);
        targetScale.Scale(originalScale);
    }

    private void OnMouseExit() {
        moused = false;
        targetScale = originalScale;
    }

    private void OnDisable() {
        OnMouseExit();
    }

    private void OnEnable() {
        if(justInstanced) return;
        transform.localScale = Vector3.zero;
    }
}
