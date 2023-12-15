using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BtChangeStatusEffect : BtNode {
	private Blackboard blackboard;
	private StatusEffects se;
	private StatusEffects.Effects effects;
	private bool statusOn;

	public BtChangeStatusEffect(Blackboard _blackboard, StatusEffects.Effects _status, bool _statusOn) {
		blackboard = _blackboard;
		effects = _status;
		statusOn = _statusOn;

		se = blackboard.GetData<Transform>(StringNames.Transform_BBowner).GetComponent<StatusEffects>();
	}

	public override BtResult Run() {				
		if(!se) {
			return BtResult.failed;
		}

		if(statusOn) {
			se.AddStatus(effects);
			return BtResult.success;
		} else {
			se.RemoveEffect(effects);
			return BtResult.success;
		}
	}
}
