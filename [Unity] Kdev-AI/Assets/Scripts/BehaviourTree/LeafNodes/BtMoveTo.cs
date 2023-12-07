using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BtMoveTo : BtNode {
	private NavMeshAgent agent;
	private Transform target;
	private float distanceOffset;

	public BtMoveTo(Blackboard _blackboard, Transform _target) {
		Transform obj = _blackboard.GetData<Transform>(StringNames.Transform_BBowner);
		agent = obj.GetComponent<NavMeshAgent>();
		target = _target;
		distanceOffset = _blackboard.GetData<float>(StringNames.Float_GeneralOffset);
		//Debug.Log($"Move to target:{target.name} ({target})");
	}

	public BtMoveTo(Blackboard _blackboard, string _targetName) {
		Transform obj = _blackboard.GetData<Transform>(StringNames.Transform_BBowner);
		agent = obj.GetComponent<NavMeshAgent>();
		target = _blackboard.GetData<Transform>(_targetName);
		distanceOffset = _blackboard.GetData<float>(StringNames.Float_GeneralOffset);
        //Debug.Log($"Move to target:{target.name} ({target})");
    }

	private float CalculateDistance() {
        Vector3 agentPosition = agent.transform.position;
		Vector3 targetPosition = target.position;

		agentPosition.y = 0;
		targetPosition.y = 0;

		float distance = Vector3.Distance(agentPosition, targetPosition);

		return distance;
    }

	public override BtResult Run() {
		if(CalculateDistance() > distanceOffset) {
			agent.isStopped = false;
			agent.SetDestination(target.position);
			return BtResult.running;
		} else {
			agent.isStopped = true;
            return BtResult.success;
		}
	}

}
