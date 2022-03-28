using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int detectionDistance = 10;
    private CharacterDetails characterDetails;
    private PathFinder pathFinder;
    private GridCharacter gridCharacter;

    private void Awake() {
        characterDetails = GetComponent<CharacterDetails>();
        pathFinder = GetComponent<PathFinder>();
        gridCharacter = GetComponent<GridCharacter>();
    }

    public void StartTurn() {
        if(PlayerInRange()) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            pathFinder.FindPath(Vector2Int.RoundToInt(gridCharacter.GetCoord()),Vector2Int.RoundToInt(player.GetComponent<GridCharacter>().GetCoord()));

            Vector2 moveDirection = (pathFinder.currentPath[1] - gridCharacter.GetCoord());
            if(gridCharacter.SafeMove(moveDirection)) return;
        }

        gridCharacter.EndTurn();
    }

    public void EndTurn() {
        
    }

    private bool PlayerInRange() {
        return (Vector3.Distance(transform.position,GameObject.FindGameObjectWithTag("Player").transform.position) < detectionDistance);
    }
}
