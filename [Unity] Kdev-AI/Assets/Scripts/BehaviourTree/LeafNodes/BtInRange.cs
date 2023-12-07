using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BtInRange : BtNode {
	private Blackboard blackboard;
	private string rangeType;
	private string targetName;
	private Transform targetTransform;

	private bool inRange;

	public BtInRange(Blackboard _blackboard, Transform _target, string _rangeType) {
		blackboard = _blackboard;
        targetTransform = _target;
        rangeType = _rangeType;

        CalculateRange();
    }

	public BtInRange(Blackboard _blackboard, string _targetName, string _rangeType) {
        blackboard = _blackboard;
		targetName = _targetName;
        rangeType = _rangeType;

        CalculateRange();
    }

    private void CalculateRange() {
        Transform obj = blackboard.GetData<Transform>(StringNames.Transform_BBowner);

        if (!targetTransform) {
            targetTransform = blackboard.GetData<Transform>(targetName);
        }

        float distance = Vector3.Distance(obj.position, targetTransform.position);
        float range = blackboard.GetData<float>(rangeType);

        blackboard.SetData<Transform>(StringNames.Transform_CurrentTarget, targetTransform);
        inRange = distance <= range;
    }

	public override BtResult Run() {
		if(inRange) {			
			return BtResult.success;
		} else {
			return BtResult.failed;
		}
	}
}
