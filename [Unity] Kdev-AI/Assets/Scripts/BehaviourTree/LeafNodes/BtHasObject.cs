using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtHasObject : BtNode {
	private bool hasObject;

	public BtHasObject(Blackboard blackBoard, string _object) {
		hasObject = blackBoard.GetData<bool>($"Has{_object}");
	}

	public override BtResult Run() {
		if(hasObject) {
			return BtResult.success;
		} else {
			return BtResult.failed;
		}
	}
}
