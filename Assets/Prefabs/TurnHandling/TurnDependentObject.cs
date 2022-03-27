using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnDependentObject : MonoBehaviour
{
    public UnityEvent turnStarted;
    public UnityEvent turnEnded;

    public void Start() {
        TurnHandler.RegisterTurnDependentObject(this);
    }

    public void StartTurn() {
        turnStarted.Invoke();
    }

    public void EndTurn() {
        turnEnded.Invoke();
        TurnHandler.RegisterTurnEnd(this);
    }
}
