using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BtAttack : BtNode {
	private Blackboard blackboard;
	private string targetString;

	public BtAttack(Blackboard _blackboard, string _target) {
		blackboard = _blackboard;
        targetString = _target;
    }

	public override BtResult Run() {
        Transform target = blackboard.GetData<Transform>(targetString);

        if (true) {
			//attack target
			Debug.Log($"Attacking {target.name}");
			return BtResult.running;
		//} else if (){
			//when target dead 
			//return BtResult.success;
		} else {
			//when target is lost/far
			return BtResult.failed;
		}
	}
}
