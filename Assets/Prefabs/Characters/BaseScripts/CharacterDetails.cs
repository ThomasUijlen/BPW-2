using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDetails : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float maxEnergy = 100.0f;
    public float energyRegen = 20.0f;
    private float currentHealth;
    private float currentEnergy;

    public List<Ability> abilities = new List<Ability>();

    public UnityEvent healthAmountChanged;
    public UnityEvent energyAmountChanged;
    public UnityEvent dead;

    private void Start() {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    public void TurnStarted() {
        ChangeEnergy(energyRegen);
    }

    public void TurnEnded() {

    }

    public void ChangeHealth(float amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0.0f, maxHealth);

        healthAmountChanged.Invoke();
        if(currentHealth == 0) dead.Invoke();
    }

    public void ChangeEnergy(float amount) {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0.0f, maxEnergy);

        energyAmountChanged.Invoke();
    }
}
