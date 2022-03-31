using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    DungeonGenerator dungeonGenerator;
    public GridCharacter gridCharacter;
    private void Start() {
        dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
    }

    public void DropItem(Item item) {
        if(item == null) return;

        item.inventorySlot.item = null;
        item.inventorySlot = null;

        dungeonGenerator.PlaceItem(gridCharacter.GetCoord(),item);
    }
}
