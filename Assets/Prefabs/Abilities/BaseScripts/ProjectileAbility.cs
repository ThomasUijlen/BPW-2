using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : Ability
{
    public string canHit = "";
    public float projectileSpeed = 1.0f;
    public float damage = 10.0f;
    public int maxFlyDistance = 10;

    private Vector2 startCoord;
    private Vector2 targetCoord;
    private GridCharacter characterToHit;


    private DungeonGenerator dungeonGenerator;

    public void Start() {
        dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();    
    }

    internal override void PerformAbility() {
        FindTargetCoord();
        FinishAbility();
    }

    private void FindTargetCoord() {
        Vector2 moveDirection = new Vector2(castedBy.transform.forward.x,castedBy.transform.forward.z);

        for(int i = 0; i < maxFlyDistance; i++) {
            Vector2 coordToCheck = startCoord + moveDirection*i;

            if(dungeonGenerator.IsActive(coordToCheck) && !dungeonGenerator.IsEmpty(coordToCheck)) {
                GridCharacter character = dungeonGenerator.GetOccupyingCharacter(coordToCheck);

                if(character.tag == canHit) {
                    characterToHit = character;
                    targetCoord = coordToCheck;
                    return;
                }
            }

            targetCoord = coordToCheck;
        }
    }

    public Vector2 GetCoord() {
        return new Vector2(transform.position.x, transform.position.z) / DungeonGenerator.TILE_WIDTH;
    }

    public void SetCoord(Vector2 newCoord) {
        transform.position = new Vector3(newCoord.x * DungeonGenerator.TILE_WIDTH,transform.position.y,newCoord.y * DungeonGenerator.TILE_WIDTH);
    }

    float moveProgress = 0.0f;
    private void Update() {
        moveProgress += Time.deltaTime*projectileSpeed;
        SetCoord(Vector2.Lerp(startCoord,targetCoord,moveProgress));

        if(moveProgress > 1.0f) {
            moveProgress = 1.0f;
            
            if(characterToHit != null) characterToHit.GetComponent<CharacterDetails>().ChangeHealth(-damage);
            FinishAbility();
        }
    }
}
