using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int detectionDistance = 10;
    private CharacterDetails characterDetails;
    private PathFinder pathFinder;
    private GridCharacter gridCharacter;
    private DungeonGenerator dungeonGenerator;

    public Ability ability;

    private GridCharacter characterToHit = null;

    private void Awake() {
        characterDetails = GetComponent<CharacterDetails>();
        pathFinder = GetComponent<PathFinder>();
        gridCharacter = GetComponent<GridCharacter>();
        dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
    }

    public void StartTurn() {
        if(PlayerInRange()) {
            CheckForCharacterToHit();
            if(characterToHit) {
                gridCharacter.LookAt((characterToHit.GetCoord() - gridCharacter.GetCoord()).normalized);
                Ability castAbility = Instantiate(ability,transform.position,transform.rotation);
                castAbility.Cast(gridCharacter,gridCharacter.body);
                return;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            pathFinder.FindPath(Vector2Int.RoundToInt(gridCharacter.GetCoord()),Vector2Int.RoundToInt(player.GetComponent<GridCharacter>().GetCoord()));

            Vector2 moveDirection = (pathFinder.currentPath[1] - gridCharacter.GetCoord());
            if(gridCharacter.SafeMove(moveDirection)) return;
        }

        gridCharacter.EndTurn();
    }

    public void EndTurn() {
        
    }

    public void Dead() {
        gridCharacter.OnDestroy();
        GameObject.Destroy(gameObject);
    }

    private bool PlayerInRange() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null) return false;
        return (Vector3.Distance(transform.position,player.transform.position) < detectionDistance);
    }

    private void CheckForCharacterToHit() {
        characterToHit = null;
        ScanForTarget(new Vector2(1,0), ability.aiCastDistance);
        ScanForTarget(new Vector2(-1,0), ability.aiCastDistance);
        ScanForTarget(new Vector2(0,1), ability.aiCastDistance);
        ScanForTarget(new Vector2(0,-1), ability.aiCastDistance);
    }

    private void ScanForTarget(Vector2 scanDirection, int distance) {
        Vector2 currentCoord = Vector2.zero;

        for(int i = 1; i < distance; i++) {
            Vector2 coordToCheck = gridCharacter.GetCoord() + scanDirection*i;

            if(!dungeonGenerator.IsActive(coordToCheck)) return;

            if(!dungeonGenerator.IsEmpty(coordToCheck)) {
                GridCharacter character = dungeonGenerator.GetOccupyingCharacter(coordToCheck);

                if(character.tag == "Player") {
                    characterToHit = character;
                    return;
                }
            }
        }
    }
}
