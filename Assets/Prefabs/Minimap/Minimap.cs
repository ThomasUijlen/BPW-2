using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private int minimapSize = 5;
    private int scrollCount = 1;
    public GridCharacter gridCharacter;
    public GameObject activeTile;
    public GameObject blockedTile;
    public GameObject player;
    public GameObject enemy;
    public GameObject item;

    public List<GameObject> mapItems = new List<GameObject>();

    DungeonGenerator dungeonGenerator;

    private bool hover = false;

    private Popup popup;

    private void Awake() {
        dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
        popup = GetComponent<Popup>();
    }

    private void Update() {
        if(hover) {
            if(Input.GetAxisRaw("Mouse ScrollWheel") > 0 && scrollCount > 1)
            {
                scrollCount -= 1;
                minimapSize -= 2*scrollCount;
            }
            else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0 && scrollCount < 7)
            {
                minimapSize += 2*scrollCount;
                scrollCount += 1;
            }

            RefreshMap();
        }
    }

    public void RefreshMap() {
        ClearOldMap();

        Vector2 currentCoord = gridCharacter.GetCoord();
        float mapStep = 1f/minimapSize;
        float mapOffset = mapStep * (Mathf.Ceil(minimapSize/2f));
        int scanOffset = Mathf.FloorToInt(minimapSize/2f);

        for(int x = 0; x < minimapSize; x++) {
            for(int y = 0; y < minimapSize; y++) {
                Vector2 scanCoord = currentCoord-new Vector2(scanOffset,scanOffset)+new Vector2(x,y);
                Vector2 mapPosition = new Vector2(x*mapStep,y*mapStep)-new Vector2(mapOffset,mapOffset);

                if(dungeonGenerator.IsActive(scanCoord)) {
                    PlaceMapItem(activeTile,mapPosition,mapStep);
                } else {
                    PlaceMapItem(blockedTile,mapPosition,mapStep);
                }

                if(dungeonGenerator.IsActive(scanCoord) && !dungeonGenerator.IsEmpty(scanCoord)) {
                    GridCharacter character = dungeonGenerator.GetOccupyingCharacter(scanCoord);

                    if(character.tag == "Player") {
                        PlaceMapItem(player,mapPosition,mapStep);
                    } else {
                        PlaceMapItem(enemy,mapPosition,mapStep);
                    }
                }

                if(dungeonGenerator.HasItem(scanCoord)) PlaceMapItem(item,mapPosition,mapStep);
            }
        }
    }

    private void PlaceMapItem(GameObject prefab, Vector2 mapPosition, float scale) {
        GameObject mapItem = Instantiate(prefab);
        mapItem.transform.parent = transform;
        mapItem.transform.localPosition = new Vector3(mapPosition.x,0,mapPosition.y) + new Vector3(scale,0.1f,scale);
        mapItem.transform.localScale = new Vector3(scale,scale,scale);
        mapItem.transform.localEulerAngles = Vector3.zero;
        mapItems.Add(mapItem);
    }

    private void ClearOldMap() {
        foreach(GameObject mapItem in mapItems) {
            GameObject.Destroy(mapItem);
        }

        mapItems.Clear();
    }

    private void OnDisable() {
        hover = false;
    }

    private void OnMouseEnter() {
        hover = true;
        popup.targetScale = new Vector3(4,1,4);
    }

    private void OnMouseExit() {
        hover = false;
        popup.targetScale = popup.originalScale;
    }
}
