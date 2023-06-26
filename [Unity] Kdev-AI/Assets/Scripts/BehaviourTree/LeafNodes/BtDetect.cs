using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BtDetect : BtNode {
	private Blackboard blackBoard;
	private Transform obj;
	private float radiusFar;
	private float radiusNear;
	private float sightAngle;
	private string targetTag;


	public BtDetect(Blackboard _blackBoard, string _target, string _angle) {
		blackBoard = _blackBoard;
		obj = _blackBoard.GetData<Transform>("ThisTransform");
		radiusFar = _blackBoard.GetData<float>("DetectRadiusFar");
		radiusNear = _blackBoard.GetData<float>("DetectRadiusNear");
		sightAngle = _blackBoard.GetData<float>("_angle");
		targetTag = _target;
	}

	public override BtResult Run() {
		Collider[] hitColliders = Physics.OverlapSphere(obj.transform.position, radiusFar);

		foreach(Collider hit in hitColliders) {
			if(hit.transform.tag == targetTag) {
				if(CheckDetection(hit.transform)) {
					blackBoard.SetData<Transform>("CurrentTarget", hit.transform);
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
}
