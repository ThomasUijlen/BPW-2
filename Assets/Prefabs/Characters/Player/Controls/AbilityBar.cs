using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityBar : MonoBehaviour
{
    private Ability selectedAbilityPrefab;
    public TurnDependentObject player;
    public UnityEvent directionalAbilityPressed;
    public UnityEvent abilityCasted;

    public void AbilityEquipped(Item item) {
        if(item == null) return;
        Button button = item.GetComponent<Button>();
        button.pressedFrom.AddListener(AbilityPressed);
    }

    public void AbilityPressed(GameObject obj) {
        selectedAbilityPrefab = obj.GetComponent<Item>().equipPrefab.GetComponent<Ability>();
        
        if(selectedAbilityPrefab.isDirectional) {
            directionalAbilityPressed.Invoke();
        } else {
            CastAbility();
        }
    }

    public void CastAbility() {
        Ability ability = Instantiate(selectedAbilityPrefab);
        ability.Cast(player,player.GetComponent<GridCharacter>().body);
        abilityCasted.Invoke();
    }
}
