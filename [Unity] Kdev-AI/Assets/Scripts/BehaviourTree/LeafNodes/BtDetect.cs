using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtDetect : BtNode {
	private Blackboard blackBoard;
	private Transform obj;
	private float radiusFar;
	private float radiusNear;
	private float sightAngle;
	private string targetTag;


	public BtDetect(Blackboard _blackboard, string _targetTag, string _angle) {
		blackBoard = _blackboard;
		obj = _blackboard.GetData<Transform>(StringNames.Transform_BBowner);
		radiusFar = _blackboard.GetData<float>(StringNames.Float_DetectRadiusFar);
		radiusNear = _blackboard.GetData<float>(StringNames.Float_DetectRadiusNear);
		sightAngle = _blackboard.GetData<float>(_angle);
		targetTag = _targetTag;
	}

    public override BtResult Run() {
		Collider[] hitColliders = Physics.OverlapSphere(obj.transform.position, radiusFar);

		foreach(Collider hit in hitColliders) {
			if(hit.transform.tag == targetTag) {
				if(CheckDetection(hit.transform)) {
					blackBoard.SetData<Transform>(StringNames.Transform_CurrentTarget, hit.transform);
					return BtResult.success;
				}
			}
		}

		Debug.Log($"Target {targetTag} not found!");
		return BtResult.failed;
	}

	private bool CheckDetection(Transform target) {
		float distance = Vector3.Distance(obj.position, target.position);
		Vector3 direction = obj.position - target.position;
		float angle = Vector3.Angle(obj.forward, direction);

		//cant detect if agent is blinded
		if(CheckBlindingEffects()) {
			return false;
		}

		RaycastHit hit;
		if(Physics.Linecast(obj.position, target.position, out hit)) {		

			if(hit.transform == target) {
				if(distance < radiusNear) {
					Debug.Log($"Detected {target.tag} in close range.");
					return true;
				} else if(Mathf.Abs(angle) < sightAngle || sightAngle == 0) {
					Debug.Log($"Detected {target.tag} in eye sight.");
					return true;
				} 
			}
		} 

		return false;
	}

	//effects that blinds
	private bool CheckBlindingEffects() {
		StatusEffects se = obj.GetComponent<StatusEffects>();
		if(!se) {
			return false;
		}

		if(se.HasEffect(StatusEffects.Effects.InSmoke)) {
			return true;
		}

		return false;
	}
}
