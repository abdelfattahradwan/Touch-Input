using UnityEngine;
using UnityEngine.Events;

namespace WinterboltGames.TouchInput.Scripts.Controls
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
				SimpleTouch touch = TouchInput.GetTouchByIndex(touchIndex);

				if (touch.Phase == SimpleTouchPhase.Began)
				{
					IsPressed = true;
				}
				else if (touch.Phase is SimpleTouchPhase.Ended or SimpleTouchPhase.Canceled)
				{
					ResetTouchButton();
				}
			}
		}

		private void ResetTouchButton()
		{
			touchId = -1;

			IsPressed = false;
		}
	}
}
