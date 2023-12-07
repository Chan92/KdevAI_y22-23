using UnityEngine;

public class BtWait : BtNode {
	private float waitTime;

	public BtWait(float _waitTime) {
		waitTime = _waitTime;
    }

	public override BtResult Run() {
		waitTime -= Time.deltaTime;

		if (waitTime > 0) {
			return BtResult.running;
		} else {
            return BtResult.success;
		}
    }
}