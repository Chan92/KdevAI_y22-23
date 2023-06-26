using UnityEngine;

public class BtWait : BtNode {
	private float waitTime;
	private float currentTime;

	public BtWait(float _waitTime) {
		waitTime = _waitTime;
	}

	public override BtResult Run() {
		currentTime += Time.deltaTime;

		if(currentTime >= waitTime) {
			currentTime = 0;
			return BtResult.success;
		}

		return BtResult.running;
	}
}