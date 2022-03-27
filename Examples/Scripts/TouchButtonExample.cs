using UnityEngine;
using UnityEngine.UI;
using WinterboltGames.TouchInput.Scripts.Controls;

namespace WinterboltGames.TouchInput.Examples.Scripts
{
	public sealed class TouchButtonExample : MonoBehaviour
	{
		[SerializeField]
		private TouchButton touchButton;

		[SerializeField]
		private Image image;

		[SerializeField]
		private Color normalColor;

		[SerializeField]
		private Color pressedColor;

		private void Start()
		{
			touchButton.OnPressed.AddListener(() => image.color = pressedColor);

			touchButton.OnReleased.AddListener(() => image.color = normalColor);

			image.color = normalColor;
		}
	}
}
