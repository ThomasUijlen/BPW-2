using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurnHandler
{
    private static LinkedList<TurnDependentObject> turnDependentObjects = new LinkedList<TurnDependentObject>();

    public static LinkedListNode<TurnDependentObject> RegisterTurnDependentObject(TurnDependentObject turnDependentObject) {
        LinkedListNode<TurnDependentObject> linkedListNode = new LinkedListNode<TurnDependentObject>(turnDependentObject);
        turnDependentObjects.AddLast(linkedListNode);

        if(turnDependentObjects.Count == 1) turnDependentObject.StartTurn();

        return linkedListNode;
    }

    public static void RegisterTurnEnd(LinkedListNode<TurnDependentObject> linkedListNode) {
        if(linkedListNode.Next != null) {
            linkedListNode.Next.Value.StartTurn();
        } else {
            turnDependentObjects.First.Value.StartTurn();
        }
    }
}
