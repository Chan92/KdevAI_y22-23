public class BtInverter : BtNode {
	private BtNode child;

	public BtInverter(BtNode _child) {
		child = _child;
	}

	public override BtResult Run() {
		BtResult result = child.Run();

		switch(result) {
			case BtResult.success:
				return BtResult.failed;
			case BtResult.failed:
				return BtResult.success;
			case BtResult.running:
				break;
		}

		return BtResult.running;
	}
}