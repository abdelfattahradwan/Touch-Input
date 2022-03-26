using UnityEngine;
using WinterboltGames.TouchInput.Controls;

namespace WinterboltGames.TouchInput.Examples
{
	public sealed class ThumbstickExample : MonoBehaviour
    {
        [SerializeField]
        private Thumbstick thumbstick;

		[SerializeField]
		private Transform target;

		private void Update()
		{
			target.rotation = Quaternion.FromToRotation(transform.up, thumbstick.Input);
		}
	}
}
