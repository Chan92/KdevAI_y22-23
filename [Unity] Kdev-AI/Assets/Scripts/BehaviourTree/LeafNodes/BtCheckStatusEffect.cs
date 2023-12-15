using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtCheckStatusEffect:BtNode {
	private Blackboard blackboard;
	private StatusEffects se;
	private StatusEffects.Effects effects;
	private bool statusOn;

	public BtCheckStatusEffect(Blackboard _blackboard, string _target, StatusEffects.Effects _status, bool _statusOn) {
		blackboard = _blackboard;
		effects = _status;
		statusOn = _statusOn;

		se = blackboard.GetData<Transform>(_target).GetComponent<StatusEffects>();
	}

	public override BtResult Run() {
		if(!se) {
			return BtResult.failed;
		}

		if(se.HasEffect(effects) == statusOn) {
			return BtResult.success;
		} else {		
			return BtResult.failed;
		}
	}
}