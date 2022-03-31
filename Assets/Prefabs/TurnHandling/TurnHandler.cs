using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurnHandler
{
    private static LinkedList<TurnDependentObject> turnDependentObjects = new LinkedList<TurnDependentObject>();

    public static TurnDependentObject objectTakingTurn;

    public static void Refresh() {
        turnDependentObjects = new LinkedList<TurnDependentObject>();
        objectTakingTurn = null;
    }

    public static void RegisterTurnDependentObject(TurnDependentObject turnDependentObject) {
        turnDependentObjects.AddLast(turnDependentObject);

        if(turnDependentObjects.Count == 1) StartTurnFor(turnDependentObject);
    }

    public static void DeregisterTurnDependentObject(TurnDependentObject turnDependentObject) {
        if(turnDependentObject == objectTakingTurn) RegisterTurnEnd(turnDependentObject);
        if(turnDependentObjects.Contains(turnDependentObject)) turnDependentObjects.Remove(turnDependentObject);
    }

    public static void RegisterTurnEnd(TurnDependentObject turnDependentObject) {
        LinkedListNode<TurnDependentObject> linkedListNode = turnDependentObjects.Find(turnDependentObject);

        if(linkedListNode.Next != null) {
            StartTurnFor(linkedListNode.Next.Value);
        } else {
            StartTurnFor(turnDependentObjects.First.Value);
        }
    }

    private static void StartTurnFor(TurnDependentObject turnDependentObject) {
        objectTakingTurn = turnDependentObject;
        turnDependentObject.StartTurn();
    }
}
