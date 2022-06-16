using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace WinterboltGames.TouchInput.Scripts.Controls
{
	public sealed class TouchButton : TouchControl
	{
		private bool _isPressed;

		public bool IsPressed
		{
			get => _isPressed;

			private set
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
				Touch touch = Touch.activeTouches[touchIndex];

				if (touch.phase == TouchPhase.Began)
				{
					IsPressed = true;
				}
				else if (touch.phase is TouchPhase.Ended or TouchPhase.Canceled)
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
