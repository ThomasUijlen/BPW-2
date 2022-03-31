using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    private GridCharacter gridCharacter;
    private DungeonGenerator dungeonGenerator;

    private void Start() {
        gridCharacter = GetComponent<GridCharacter>();
        dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
    }

    public void CheckForPickup() {
        Vector2 coord = gridCharacter.GetCoord();
        Debug.Log(dungeonGenerator.HasItem(coord));
        Debug.Log(coord);
        if(dungeonGenerator.HasItem(coord)) {
            Item item = dungeonGenerator.GetItem(coord);
            InventorySlot slot = GetEmptySlot();

            Debug.Log(item);

            if(slot) {
                dungeonGenerator.ClearItem(coord);
                slot.SetItem(item);
                item.canBeGrabbed = true;
            }
        }
    }

    private InventorySlot GetEmptySlot() {
        foreach(InventorySlot slot in inventorySlots) {
            Debug.Log(slot.hasItem());
            if(!slot.hasItem()) return slot;
        }

        return null;
    }
}
