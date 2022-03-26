using UnityEngine;
using UnityEngine.Events;

namespace WinterboltGames.TouchInput.Controls
{
	public sealed class TouchButton : TouchControl
	{
		private bool _isPressed;

		public bool IsPressed
		{
			get => _isPressed;

			set
			{
				if (_isPressed == value) return;

				_isPressed = value;

				if (_isPressed)
				{
					OnPressed?.Invoke();
				}
				else
				{
					OnReleased?.Invoke();
				}
			}
		}

		public UnityEvent OnPressed;
		public UnityEvent OnReleased;

		protected override void Update()
		{
			base.Update();

			if (touchIndex == -1)
			{
				ResetTouchButton();
			}
			else
			{
				Touch touch = Input.GetTouch(touchIndex);

				if (touch.phase == TouchPhase.Began)
				{
					IsPressed = true;
				}
				else if (touch.phase is (TouchPhase.Canceled or TouchPhase.Ended))
				{
					ResetTouchButton();
				}
			}
		}

		private void ResetTouchButton()
		{
			fingerId = -1;

			IsPressed = false;
		}
	}
}
