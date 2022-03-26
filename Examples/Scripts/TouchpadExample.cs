using UnityEngine;
using WinterboltGames.TouchInput.Controls;

namespace WinterboltGames.TouchInput.Examples
{
	public sealed class TouchpadExample : MonoBehaviour
	{
		[SerializeField]
		private Touchpad touchpad;

		[SerializeField]
		private Transform target;

		[SerializeField]
		private float xSensitivity;

		[SerializeField]
		private float ySensitivity;

		[SerializeField]
		private float xMin;

		[SerializeField]
		private float xMax;

		private Vector3 eulerAngles;

		private void Update()
		{
			eulerAngles.x -= touchpad.Delta.y * Time.deltaTime * ySensitivity;

			eulerAngles.x = Mathf.Clamp(eulerAngles.x, xMin, xMax);
			
			eulerAngles.y += touchpad.Delta.x * Time.deltaTime * xSensitivity;

			eulerAngles.z = 0.0f;

			target.eulerAngles = eulerAngles;
		}
	}
}
