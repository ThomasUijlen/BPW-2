using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public bool isDirectional = false;
    internal TurnDependentObject castedBy;

    public void Cast(TurnDependentObject by) {
        castedBy = by;
        PerformAbility();
    }

    internal virtual void PerformAbility() {
        //Ability logic
        FinishAbility();
    }

    internal void FinishAbility() {
        castedBy.EndTurn();
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }
}
