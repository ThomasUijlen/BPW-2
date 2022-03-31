using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnDependentObject : MonoBehaviour
{
    public UnityEvent turnStarted;
    public UnityEvent turnEnded;

    private TurnHandler turnHandler;

    public void Start() {
        turnHandler = GameObject.FindGameObjectWithTag("TurnHandler").GetComponent<TurnHandler>();
        turnHandler.RegisterTurnDependentObject(this);
    }

    public void StartTurn() {
        turnStarted.Invoke();
    }

    public void EndTurn() {
        turnEnded.Invoke();
        turnHandler.RegisterTurnEnd(this);
    }

    public void OnDestroy() {
        turnHandler.DeregisterTurnDependentObject(this);
    }
}
