using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour
{
    private Item item = null;

    public List<string> filters = new List<string>();
    public bool fake = false;
    public InventorySaver inventorySaver;

    public ItemChangedEvent itemChanged;
    public UnityEvent slotFilled;

    private bool initialLoad = true;

    private void Awake() {
        if(fake) return;
        if(!initialLoad) return;
        initialLoad = false;

        GameObject item = inventorySaver.GetSavedItem(this);

        if(item != null) {
            this.item = item.GetComponent<Item>();
            this.item.inventorySlot = this;
            item.transform.eulerAngles = transform.eulerAngles;
            item.transform.position = transform.position;

            slotFilled.Invoke();
            itemChanged.Invoke(this.item);
        }
    }

    public void SetItem(Item item) {
        if(fake) Debug.Log(this.item);
        if(fake && this.item != null) GameObject.Destroy(this.item.gameObject);

        if(item != null) {
            slotFilled.Invoke();

            if(fake) {
                item = Instantiate(item);
                item.inventorySlot = this;
                item.transform.eulerAngles = transform.eulerAngles;
                item.transform.position = transform.position;
                item.canBeGrabbed = false;
            }
        }

        this.item = item;
        itemChanged.Invoke(this.item);
        if(this.item) item.gameObject.SetActive(isActiveAndEnabled);

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

[System.Serializable]
 public class ItemChangedEvent : UnityEvent<Item> {}