using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {
	[SerializeField]
	private float maxHealth = 100f;
	private float currentHealth;

	public bool Alive {
		get;
		private set;
	}

	private void Start() {
		GetHealed(maxHealth);
		Alive = true;
	}

	public void TakeDamage(float damage) {
		currentHealth -= damage;

		if (currentHealth < 0) {
			currentHealth = 0;
			Alive = false;
			//TODO: add death
			transform.gameObject.SetActive(false);
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
