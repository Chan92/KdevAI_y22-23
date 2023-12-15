using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack")]
public class SoAttack :ScriptableObject {
	public enum SoAttackType {
		Stab,
		Throw
	}

	public SoAttackType type;
	public GameObject attackEffect;
	public float effectDuration;
	public float specialStatusDuration;

	private Transform target;
	private float strength;
	private Coroutine attackCoroutine;
	private Coroutine effectCoroutine;

	public void Attack(Transform _target, float _strength) {
		target = _target;
		strength = _strength;

		switch (type) {
			case SoAttackType.Stab:
				StabAttack();
				break;
			case SoAttackType.Throw:
				ThrowAttack();
				break;
		}
	}
	
	private void StabAttack() {
		Debug.Log("Stab attack");
		SpawnEffect();
		DoDamage();
	}

	private void ThrowAttack() {
		Debug.Log("Throw attack");
		SpawnEffect();
		DoDamage();

		StatusEffects se = target.GetComponent<StatusEffects>();
		if(se && attackCoroutine == null) {
			se.AddStatus(StatusEffects.Effects.InSmoke);
			attackCoroutine = MonoInstance.instance.StartCoroutine(MonoInstance.instance.DoCoroutine(
				SpecialStatusCooldown(StatusEffects.Effects.InSmoke)));
		}
	}

	private void DoDamage() {
		Health targetHealth = target.GetComponent<Health>();
		if(targetHealth) {
			targetHealth.TakeDamage(strength);
		}
	}

	private void SpawnEffect() {
		if(attackEffect && effectCoroutine == null) {
			GameObject effect = Instantiate(attackEffect);
			effect.transform.position = target.transform.position;

			effectCoroutine = MonoInstance.instance.StartCoroutine(MonoInstance.instance.DoCoroutine(
				DespawnEffect(effect)));
		}
	}

	private IEnumerator DespawnEffect(GameObject effect) {
		yield return new WaitForSeconds(effectDuration);
		GameObject.Destroy(effect);
	}

	private IEnumerator SpecialStatusCooldown(StatusEffects.Effects status) {
		yield return new WaitForSeconds(specialStatusDuration);

		StatusEffects se = target.GetComponent<StatusEffects>();
		if(se) {
			se.RemoveEffect(status);
		}

		attackCoroutine = null;
	}
}
