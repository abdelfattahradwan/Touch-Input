using UnityEngine;

namespace WinterboltGames.TouchInput.Scripts
{
	public readonly struct SimpleTouch
    {
        public int Id { get; }

        public Vector2 Position { get; }

        public SimpleTouchPhase Phase { get; }

		public SimpleTouch(int Id, Vector2 position, SimpleTouchPhase phase)
		{
			this.Id = Id;
			
			Position = position;

			Phase = phase;
		}
	}
}
