using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    private TurnDependentObject castedBy;

    public void Cast(TurnDependentObject by) {
        castedBy = by;
        PerformAbility();
    }

    internal void PerformAbility() {
        //Ability logic
        FinishAbility();
    }

    internal void FinishAbility() {
        castedBy.EndTurn();
    }
}
