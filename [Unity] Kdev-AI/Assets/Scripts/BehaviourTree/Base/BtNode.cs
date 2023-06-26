public abstract class BtNode {
	public enum BtResult {
		success,
		failed,
		running
	};

	public abstract BtResult Run();
}