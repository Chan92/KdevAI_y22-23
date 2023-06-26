using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {
	[SerializeField]
	private float maxHealth = 100f;
	private float currentHealth;

	private void Start() {
		GetHealed(maxHealth);
	}

	public void TakeDamage(float damage) {
		currentHealth -= damage;

		if (currentHealth < 0) {
			currentHealth = 0;
			//TODO: add death
		} 

		//TODO: update UI
	}

	public void GetHealed(float amount) {
		if(currentHealth + amount >= maxHealth) {
			currentHealth = maxHealth;
		} else {
			currentHealth += amount;
		}

		//TODO: update UI
	}
}
