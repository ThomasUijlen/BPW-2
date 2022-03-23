using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent pressed;
    internal bool moused = false;
    internal Vector3 targetScale = Vector3.one;


    private void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime*10);

        if(Input.GetButtonDown("LeftClick") && moused) {
            pressed.Invoke();
        }
    }

    private void OnMouseEnter() {
        moused = true;
        targetScale = new Vector3(1.1f,1.1f,1.1f);
    }

    private void OnMouseExit() {
        moused = false;
        targetScale = Vector3.one;
    }

    private void OnDisable() {
        OnMouseExit();
    }
}
