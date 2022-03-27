using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurnHandler
{
    private static LinkedList<TurnDependentObject> turnDependentObjects = new LinkedList<TurnDependentObject>();

    public static void RegisterTurnDependentObject(TurnDependentObject turnDependentObject) {
        turnDependentObjects.AddLast(turnDependentObject);

        if(turnDependentObjects.Count == 1) turnDependentObject.StartTurn();
    }

    public static void RegisterTurnEnd(TurnDependentObject turnDependentObject) {
        LinkedListNode<TurnDependentObject> linkedListNode = turnDependentObjects.Find(turnDependentObject);

        if(linkedListNode.Next != null) {
            linkedListNode.Next.Value.StartTurn();
        } else {
            turnDependentObjects.First.Value.StartTurn();
        }
    }
}
