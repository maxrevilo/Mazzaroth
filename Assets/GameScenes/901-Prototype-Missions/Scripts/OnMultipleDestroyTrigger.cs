namespace Mazzaroth {
	public class OnMultipleDestroyTrigger : BaseTrigger {
		public ShipState[] DestructiblesToWatch;

		int destroyedCount;

		void Start () {
			foreach (ShipState destructible in DestructiblesToWatch) {
				destructible.OnShipDestroyed += destroyed;
			}
		}

		void destroyed(ShipState destructible) {
			destroyedCount++;

			if (destroyedCount == DestructiblesToWatch.Length) {
				Trigger();
			}
		}
	}
}

