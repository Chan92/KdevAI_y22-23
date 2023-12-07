using UnityEngine;

public class BtDebug : BtNode {
	private string debugMsg;

	public BtDebug(string _debugMsg, string _color) {
        debugMsg = $"<color={_color}>{_debugMsg}</color>";
    }

    public override BtResult Run() {
		Debug.Log(debugMsg);
		return BtResult.success;
	}
}