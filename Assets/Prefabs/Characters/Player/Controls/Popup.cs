using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    internal Vector3 targetScale = Vector3.one;
    internal Vector3 originalScale = Vector3.zero;

    internal void Start() {
        originalScale = transform.localScale;
        targetScale = originalScale;
        transform.localScale = Vector3.zero;
    }


    private void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime*10);
    }

    private void OnEnable() {
        if(originalScale == Vector3.zero) return;
        transform.localScale = Vector3.zero;
    }
}
