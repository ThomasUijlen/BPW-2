using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type = "";
    public string itemName = "";
    public GameObject equipPrefab;

    [HideInInspector]
    public bool canBeGrabbed = true;

    [HideInInspector]
    public InventorySlot inventorySlot;
    private InventorySlot newSlot;
    private bool grabbed = false;
    private Vector3 screenPoint;
    private Vector3 offset;

    public void ItemGrabbed() {
        if(!canBeGrabbed) return;
        grabbed = true;

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
 
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    public void ItemReleased() {
        grabbed = false;

        if(newSlot != null) {
            if(inventorySlot != null) inventorySlot.SetItem(null);
            inventorySlot = newSlot;
            newSlot = null;
            inventorySlot.SetItem(this);
        }
    }

    private void Update() {
        if(!grabbed) {
            if(inventorySlot != null) transform.position = Vector3.Lerp(transform.position,inventorySlot.transform.position,Time.deltaTime*20f);
        }
    }

    void OnMouseDrag() {
        if(!grabbed) return;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
    
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;    
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<InventorySlot>() != null) {
            InventorySlot slot = other.gameObject.GetComponent<InventorySlot>();
            if(slot != inventorySlot && !slot.hasItem() && slot.accepts(type)) newSlot = slot;
        }
    }

    private void OnTriggerExit(Collider other) {
        newSlot = null;
    }

    private void OnEnable() {
        if(inventorySlot) transform.position = inventorySlot.transform.position;
    }
}
