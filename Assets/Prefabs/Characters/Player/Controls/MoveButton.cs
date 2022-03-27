using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveButton : Button
{
    public UnityEvent<Vector2> pressedDirection;
    public Vector2 direction;
    public bool hideWhenCharacter = true;
    


    private DungeonGenerator dungeonGenerator;
    private MeshRenderer meshRenderer;

    private new void Start() {
        base.Start();
        dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();    
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime*10);

        if(Input.GetButtonDown("LeftClick") && moused) {
            pressedDirection.Invoke(direction);
            pressed.Invoke();
        }

        if(hideWhenCharacter) {
            meshRenderer.enabled = dungeonGenerator.IsEmpty(GetCoord());
        } else {
            meshRenderer.enabled = dungeonGenerator.IsActive(GetCoord());
        }
    }

    public Vector2 GetCoord() {
        return new Vector2(transform.position.x, transform.position.z) / DungeonGenerator.TILE_WIDTH;
    }
}
