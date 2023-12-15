using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtAttack : BtNode {
	private Blackboard blackboard;
	private Transform obj;
	private Transform target;
	private Health targetHealth;
	private SoAttack attackMethod;
	private int attackCounts;
	private float strength;
	private float range;
	private float cooldown;
	private Coroutine attackDelay;


	public BtAttack(Blackboard _blackboard, string _target, SoAttack _attackMethod) {
		blackboard = _blackboard;
		attackMethod = _attackMethod;
		
		//negative number will make it attack untill the target dies or gets out of range
		attackCounts = -1;

		target = blackboard.GetData<Transform>(_target);
		GetInfo();
	}

	public BtAttack(Blackboard _blackboard, string _target, SoAttack _attackMethod, int _attackCounts) {
		blackboard = _blackboard;
		attackMethod = _attackMethod;
		attackCounts = _attackCounts;

        target = blackboard.GetData<Transform>(_target);
		GetInfo();
	}

	private void GetInfo() {
		obj = blackboard.GetData<Transform>(StringNames.Transform_BBowner);
		targetHealth = target.GetComponent<Health>();
		strength = blackboard.GetData<float>(StringNames.Float_AttackStrength);
		range = blackboard.GetData<float>(StringNames.Float_AttackRange);
		cooldown = blackboard.GetData<float>(StringNames.Float_AttackCooldown);
	}

	public override BtResult Run() {		
		float distance = Vector3.Distance(obj.position, target.position);

		if(!target || !targetHealth) {
			Debug.Log("Attack failed: failed to find target or target health.");
			return BtResult.failed;
		} else if(CheckCancelEffects()) {
			return BtResult.failed;
		}
		
		if(!targetHealth.Alive || attackCounts == 0) {
			return BtResult.success;
		} else if(distance <= range) {
			if (attackDelay == null) {
				attackDelay = MonoInstance.instance.StartCoroutine(AttackDelay(cooldown));
			}
			return BtResult.running;
		} else {
			return BtResult.failed;
		}
	}

	//effects that cancel out attacks
	private bool CheckCancelEffects() {
		StatusEffects se = obj.GetComponent<StatusEffects>();
		if(!se) {
			return false;
		}

		if(se.HasEffect(StatusEffects.Effects.InSmoke)) {
			return true;
		}

		return false;
	}

	private IEnumerator AttackDelay(float delay) {
		Debug.Log($"Attacking {target.name}, count:{attackCounts}");
		attackMethod.Attack(target, strength);
		attackCounts--;
		yield return new WaitForSeconds(delay);

		attackDelay = null;
	}
}
