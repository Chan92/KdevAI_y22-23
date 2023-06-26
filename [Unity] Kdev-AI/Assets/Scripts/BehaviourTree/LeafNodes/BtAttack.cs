using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BtAttack : BtNode {
	private Blackboard blackboard;
	private Transform target;

	public BtAttack(Blackboard _blackBoard, string _target) {
		blackboard = _blackBoard;
		target = blackboard.GetData<Transform>(_target);
	}

	public override BtResult Run() {
		if(true) {
			//attack target
			//return BtResult.running;
		//} else if (){
			//when target dead 
			return BtResult.success;
		} else {
			//when target is lost/far
			return BtResult.failed;
		}
	}
}
