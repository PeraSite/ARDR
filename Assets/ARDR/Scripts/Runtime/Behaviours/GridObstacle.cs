namespace ARDR {
	public class GridObstacle : GridObjectBase, ITouchListener {

		public void OnTouch() {
			Destroy(gameObject);
		}
	}
}
