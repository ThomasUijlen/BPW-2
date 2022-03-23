using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnDependentObject : MonoBehaviour
{
    public UnityEvent turnStarted;
    public UnityEvent turnEnded;


    private LinkedListNode<TurnDependentObject> linkedListNode;

    public void Start() {
        linkedListNode = TurnHandler.RegisterTurnDependentObject(this);
    }

    public void StartTurn() {
        turnStarted.Invoke();
    }

    public void EndTurn() {
        turnEnded.Invoke();
        TurnHandler.RegisterTurnEnd(linkedListNode);
    }
}
