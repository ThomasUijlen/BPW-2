using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridCharacter : TurnDependentObject
{
    public UnityEvent characterMoved;
    public UnityEvent finishedMoving;
    public GameObject body;

    private DungeonGenerator dungeonGenerator;

    public new void Start() {
        base.Start();
        dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();    
    }

    public void Move(Vector2 direction) {
        if(!dungeonGenerator.IsActive(GetCoord()+direction)) return;
        
        startCoord = GetCoord();
        dungeonGenerator.OccupyTile(startCoord,null);
        targetCoord = GetCoord() + direction;
        dungeonGenerator.OccupyTile(targetCoord,this);

        moving = true;
        moveProgress = 0.0f;
        characterMoved.Invoke();
    }

    public Vector2 GetCoord() {
        return new Vector2(transform.position.x, transform.position.z) / DungeonGenerator.TILE_WIDTH;
    }

    public void SetCoord(Vector2 newCoord) {
        transform.position = new Vector3(newCoord.x * DungeonGenerator.TILE_WIDTH,transform.position.y,newCoord.y * DungeonGenerator.TILE_WIDTH);
    }

    public void LookAt(Vector2 direction) {
        Vector3 lookDirection = body.transform.position + new Vector3(direction.x,0,direction.y);
        body.transform.LookAt(lookDirection);
    }





    //Movement
    private float moveProgress = 0.0f;
    private Vector2 startCoord = Vector2.zero;
    private Vector2 targetCoord = Vector2.zero;
    private bool moving = false;

    private void Update() {
        if(moving) {
            moveProgress += Time.deltaTime;
            if(moveProgress > 1.0f) {
                moveProgress = 1.0f;
                moving = false;
                finishedMoving.Invoke();
            }

            SetCoord(Vector2.Lerp(startCoord,targetCoord,moveProgress));
        }
    }
}
