using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour
{
    public Item item = null;

    public List<string> filters = new List<string>();
    public bool fake = false;

    public ItemChangedEvent itemChanged;
    public UnityEvent slotFilled;

    private bool initialLoad = true;

    public void SetItem(Item item) {
        if(fake && this.item != null) GameObject.Destroy(this.item.gameObject);

        if(item != null) {
            slotFilled.Invoke();

            if(fake) {
                item = Instantiate(item);
                item.canBeGrabbed = false;
            }

            item.inventorySlot = this;
            item.transform.eulerAngles = transform.eulerAngles;
            item.transform.position = transform.position;
        }

        this.item = item;
        if(this.item) this.item.gameObject.SetActive(isActiveAndEnabled);
        itemChanged.Invoke(this.item);
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

[System.Serializable]
 public class ItemChangedEvent : UnityEvent<Item> {}