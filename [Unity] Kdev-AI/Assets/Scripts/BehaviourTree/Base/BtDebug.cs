using UnityEngine;

public class BtDebug : BtNode {
	private string debugMsg;

	public BtDebug(string _debugMsg) {
		debugMsg = _debugMsg;
	}

	public override BtResult Run() {
		Debug.Log(debugMsg);
		return BtResult.success;
	}
}