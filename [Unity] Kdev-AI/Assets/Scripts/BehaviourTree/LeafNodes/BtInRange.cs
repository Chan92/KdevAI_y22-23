using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BtInRange : BtNode {
	private bool inRange;

	public BtInRange(Blackboard _blackBoard, Transform _target, string _rangeType) {
		Transform obj = _blackBoard.GetData<Transform>("ThisTransform");

		float distance = Vector3.Distance(obj.position, _target.position);
		float range = _blackBoard.GetData<float>(_rangeType);

		_blackBoard.SetData<Transform>("CurrentTarget", _target);
		inRange = distance <= range;
	}

	public BtInRange(Blackboard _blackBoard, string _targetName, string _rangeType) {
		Transform obj = _blackBoard.GetData<Transform>("ThisTransform");
		Transform target = _blackBoard.GetData<Transform>(_targetName);

		float distance = Vector3.Distance(obj.position, target.position);
		float range = _blackBoard.GetData<float>(_rangeType);

		_blackBoard.SetData<Transform>("CurrentTarget", target);
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
