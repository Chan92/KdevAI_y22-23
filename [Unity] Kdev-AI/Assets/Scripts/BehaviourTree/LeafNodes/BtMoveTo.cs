using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BtMoveTo : BtNode {
	private NavMeshAgent agent;
	private Transform target;
	private float distanceOffset;

	public BtMoveTo(Blackboard blackBoard, Transform _target) {
		Transform obj = blackBoard.GetData<Transform>("ThisTransform");
		agent = obj.GetComponent<NavMeshAgent>();
		target = _target;
		distanceOffset = blackBoard.GetData<float>("AnyOffset");
	}

	public BtMoveTo(Blackboard blackBoard, string _targetName) {
		Transform obj = blackBoard.GetData<Transform>("ThisTransform");
		agent = obj.GetComponent<NavMeshAgent>();
		target = blackBoard.GetData<Transform>(_targetName);
		distanceOffset = blackBoard.GetData<float>("AnyOffset");
	}

	public override BtResult Run() {
		float distance = Vector3.Distance(agent.transform.position, target.position);

		if(distance > distanceOffset) {
			agent.isStopped = false;
			agent.SetDestination(target.position);
			return BtResult.running;
		} else {
			agent.isStopped = true;
			return BtResult.success;
		}
	}

}
