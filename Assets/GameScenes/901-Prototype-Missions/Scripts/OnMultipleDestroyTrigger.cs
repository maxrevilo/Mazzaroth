using Mazzaroth.Ships;

namespace Mazzaroth {
	public class OnMultipleDestroyTrigger : BaseTrigger {
		public Ship[] DestructiblesToWatch;

		int destroyedCount = 0;

		void Start () {
			foreach (Ship destructible in DestructiblesToWatch) {
				destructible.OnShipDestroyed += destroyed;
			}
		}

		void destroyed(Ship destructible) {
			destroyedCount++;

			if (destroyedCount == DestructiblesToWatch.Length) {
				Trigger();
			}
		}
	}
}

