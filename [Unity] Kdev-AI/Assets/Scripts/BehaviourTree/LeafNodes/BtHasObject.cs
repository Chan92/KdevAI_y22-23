using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtHasObject : BtNode {
	private Blackboard blackboard;
	private string objectType;

	public BtHasObject(Blackboard _blackboard, string _hasObject) {
		blackboard = _blackboard;
		objectType = _hasObject;
	}

	public override BtResult Run() {
		if(blackboard.GetData<bool>(objectType)) {
			return BtResult.success;
		} else {
			return BtResult.failed;
		}
	}
}
