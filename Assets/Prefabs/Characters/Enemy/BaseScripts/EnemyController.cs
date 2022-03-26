using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private CharacterDetails characterDetails;

    public void StartTurn() {
        characterDetails = GetComponentInChildren<CharacterDetails>();
    }

    public void EndTurn() {
        
    }
}
