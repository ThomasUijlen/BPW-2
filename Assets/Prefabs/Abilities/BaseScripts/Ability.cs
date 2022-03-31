using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public bool isDirectional = false;
    internal TurnDependentObject castedBy;
    internal GameObject body;
    public GameObject startCastEffect;
    public int aiCastDistance = 1;

    public void Cast(TurnDependentObject by, GameObject body) {
        castedBy = by;
        this.body = body;
        PerformAbility();

        if(startCastEffect) Instantiate(startCastEffect,transform.position,transform.rotation);
    }

    internal virtual void PerformAbility() {
        //Ability logic
        FinishAbility();
    }

    internal void FinishAbility() {
        castedBy.EndTurn();
        GameObject.Destroy(gameObject,0.5f);
    }
}
