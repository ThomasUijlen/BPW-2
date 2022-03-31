using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    private LinkedList<TurnDependentObject> turnDependentObjects = new LinkedList<TurnDependentObject>();

    public TurnDependentObject objectTakingTurn;

    public void RegisterTurnDependentObject(TurnDependentObject turnDependentObject) {
        turnDependentObjects.AddLast(turnDependentObject);

        if(turnDependentObjects.Count == 1) StartTurnFor(turnDependentObject);
    }

    public void DeregisterTurnDependentObject(TurnDependentObject turnDependentObject) {
        if(turnDependentObject == objectTakingTurn) RegisterTurnEnd(turnDependentObject);
        if(turnDependentObjects.Contains(turnDependentObject)) turnDependentObjects.Remove(turnDependentObject);
    }

    public void RegisterTurnEnd(TurnDependentObject turnDependentObject) {
        LinkedListNode<TurnDependentObject> linkedListNode = turnDependentObjects.Find(turnDependentObject);

        if(linkedListNode.Next != null) {
            StartTurnFor(linkedListNode.Next.Value);
        } else {
            //Start after delay to prevent endless loops of 1 object starting and ending its own turn.
            if(turnDependentObjects.Count > 0 && this != null) StartCoroutine(StartAfterDelay(0.1f,turnDependentObjects.First.Value));
        }
    }

    private void StartTurnFor(TurnDependentObject turnDependentObject) {
        Debug.Log(turnDependentObject);
        objectTakingTurn = turnDependentObject;
        turnDependentObject.StartTurn();
    }

    private IEnumerator StartAfterDelay(float time, TurnDependentObject turnDependentObject)
    {
        yield return new WaitForSeconds(time);
        StartTurnFor(turnDependentObject);
    }
}
