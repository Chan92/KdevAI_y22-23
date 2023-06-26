public class BtSelector : BtNode {
	private BtNode[] children;
	private int currentIndex = 0;

	public BtSelector(params BtNode[] _children) {
		children = _children;
	}

	public override BtResult Run() {
		for(; currentIndex < children.Length; currentIndex++) {
			BtResult result = children[currentIndex].Run();

			switch(result) {
				case BtResult.success:
					currentIndex = 0;
					return BtResult.success;
				case BtResult.running:
					return BtResult.running;
				case BtResult.failed:
					break;
			}
		}

		currentIndex = 0;
		return BtResult.failed;
	}
}