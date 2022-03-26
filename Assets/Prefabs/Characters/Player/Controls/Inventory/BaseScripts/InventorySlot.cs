using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour
{
    private Item item = null;

    public List<string> filters = new List<string>();
    public InventorySaver inventorySaver;

    public UnityEvent<Item> itemChanged;
    public UnityEvent slotFilled;

    private void Start() {
        GameObject item = inventorySaver.GetSavedItem(this);

        if(item != null) {
            this.item = item.GetComponent<Item>();
            this.item.inventorySlot = this;
            item.transform.eulerAngles = transform.eulerAngles;
            item.transform.position = transform.position;
        }
    }

    public void SetItem(Item item) {
        this.item = item;
        if(item != null) slotFilled.Invoke();
        itemChanged.Invoke(item);

        inventorySaver.SaveInventorySlot(this);
    }

    public Item GetItem() {
        return item;
    }

    public bool hasItem() {
        return item != null;
    }

    public bool accepts(string type) {
        if(filters.Count == 0) return true;

        foreach(string filterType in filters) {
            if(filterType == type) return true;
        }

        return false;
    }

    private void OnEnable() {
        if(item != null) item.gameObject.SetActive(true);
    }

    private void OnDisable() {
        if(item != null) item.gameObject.SetActive(false);
    }
}
