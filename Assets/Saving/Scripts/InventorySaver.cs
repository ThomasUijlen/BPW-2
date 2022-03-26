using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "InventorySaver", menuName = "Items/InventorySaver", order = 100)]
public class InventorySaver : ScriptableObject
{
    public GameObject[] itemPrefabs;

    List<ItemData> slotData = new List<ItemData>();

    public void SaveInventorySlot(InventorySlot slot) {
        if(slot.hasItem()) {
            string slotPath = GetGameObjectPath(slot.gameObject);
            Debug.Log(slot.GetItem().itemName);
            slotData.Add(new ItemData(slotPath,slot.GetItem().itemName));
        } else {
            string slotPath = GetGameObjectPath(slot.gameObject);
            if(ContainsKey(slotPath)) slotData.Remove(GetItemData(slotPath));
        }
    }

    public GameObject GetSavedItem(InventorySlot slot) {
        string slotPath = GetGameObjectPath(slot.gameObject);
        ItemData itemData = GetItemData(slotPath);
        if(itemData == null) return null;

        foreach(GameObject item in itemPrefabs) {
            if(item.GetComponent<Item>().itemName == itemData.itemName) return Instantiate(item);
        }

        return null;
    }

    private GameObject LoadPrefabFromFile(string filename)
    {
        var loadedObject = Resources.Load<GameObject>(filename);
        return loadedObject;
    }

    private string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }

    private bool ContainsKey(string key) {
        foreach(ItemData itemData in slotData) {
            if(key == itemData.slotPath) return true;
        }
        return false;
    }

    private ItemData GetItemData(string key) {
        foreach(ItemData itemData in slotData) {
            if(key == itemData.slotPath) return itemData;
        }
        return null;
    }
}

[System.Serializable]
public class ItemData {
    public string slotPath;
    public string itemName;

    public ItemData(string slotPath, string itemName) {
        this.slotPath = slotPath;
        this.itemName = itemName;
    }
}