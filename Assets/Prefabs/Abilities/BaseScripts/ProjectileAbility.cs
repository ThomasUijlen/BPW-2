using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : Ability
{
    public string canHit = "";
    public float projectileSpeed = 1.0f;
    public float damage = 10.0f;
    public int maxFlyDistance = 10;

    public GameObject hitEffect;

    private Vector2 startCoord;
    private Vector2 targetCoord;
    private int steps = 0;
    private GridCharacter characterToHit;


    private DungeonGenerator dungeonGenerator;

    internal override void PerformAbility() {
        dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();

        transform.position = castedBy.transform.position;
        startCoord = GetCoord();

        FindTargetCoord();
    }

    private void FindTargetCoord() {
        Vector2 moveDirection = new Vector2(body.transform.forward.x,body.transform.forward.z);

        for(int i = 0; i < maxFlyDistance; i++) {
            Vector2 coordToCheck = startCoord + moveDirection*i;

            if(dungeonGenerator.IsActive(coordToCheck) && !dungeonGenerator.IsEmpty(coordToCheck)) {
                GridCharacter character = dungeonGenerator.GetOccupyingCharacter(coordToCheck);

                if(character.tag == canHit) {
                    characterToHit = character;
                    targetCoord = coordToCheck;
                    steps = i;
                    return;
                }
            }

            if(!dungeonGenerator.IsActive(coordToCheck)) {
                targetCoord = coordToCheck;
                steps = i;
                return;
            }

            steps = i;
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
        if(moveProgress < 0) return;

        moveProgress += Time.deltaTime*projectileSpeed/steps;
        SetCoord(Vector2.Lerp(startCoord,targetCoord,moveProgress));

        if(moveProgress > 1.0f) {
            moveProgress = -1.0f;
            
            if(characterToHit != null) characterToHit.GetComponent<CharacterDetails>().ChangeHealth(-damage);
            if(hitEffect) Instantiate(hitEffect,transform.position,transform.rotation);
            FinishAbility();
        }
    }
}
