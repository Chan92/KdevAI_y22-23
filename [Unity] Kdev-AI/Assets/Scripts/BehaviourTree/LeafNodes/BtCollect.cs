using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BtCollect : BtNode {
	private Blackboard blackboard;
	private bool collected;
	private string objectName;
	public BtCollect(Blackboard _blackBoard, string _object) {
		blackboard = _blackBoard;
		objectName = _object;

		collected = true;		
	}

	public override BtResult Run() {
		if(collected) {
			Debug.Log("collected");
			blackboard.SetData<bool>($"Has{objectName}", collected);
			return BtResult.success;
		} else {
			Debug.Log("failed");
			return BtResult.failed;
		}
	}
}
