using UnityEngine;
using UnityEngine.Events;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace WinterboltGames.TouchInput.Scripts.Controls
{
	public sealed class Touchpad : TouchControl
	{
		private bool _isActive;

		public bool IsActive
		{
			get => _isActive;

			set
			{
				if (_isActive == value) return;

				_isActive = value;

				if (_isActive)
				{
					OnActivated?.Invoke();
				}
				else
				{
					OnDeactivated?.Invoke();
				}
			}
		}

		public UnityEvent OnActivated;
		public UnityEvent OnDeactivated;

		private Vector2 _lastPosition;
		private Vector2 _currentPosition;

		public Vector2 Delta { get; private set; }

		protected override void Update()
		{
			touchIndex = -1;

			if (touchId == -1)
			{
				for (int i = 0; i < Touch.activeTouches.Count; i++)
				{
					Touch touch = Touch.activeTouches[i];

					if (touch.phase == TouchPhase.Began && RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, touch.screenPosition))
					{
						touchId = touch.touchId;

						touchIndex = i;

						break;
					}

					if (touchIndex != -1) break;
				}
			}
			else
			{
				for (int i = 0; i < Touch.activeTouches.Count; i++)
				{
					Touch touch = Touch.activeTouches[i];

					if (touch.touchId == touchId)
					{
						touchIndex = i;

						break;
					}

					if (touchIndex != -1) break;
				}
			}

			if (touchIndex == -1)
			{
				ResetTouchpad();
			}
			else
			{
				Touch touch = Touch.activeTouches[touchIndex];

				if (touch.phase == TouchPhase.Began)
				{
					IsActive = true;

					_lastPosition = touch.screenPosition;
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					_currentPosition = touch.screenPosition;

					Delta = _currentPosition - _lastPosition;

					_lastPosition = _currentPosition;
				}
				else if (touch.phase == TouchPhase.Stationary)
				{
					Delta = Vector2.zero;
				}
				else if (touch.phase is TouchPhase.None or TouchPhase.Ended or TouchPhase.Canceled)
				{
					ResetTouchpad();
				}
			}
		}

		private void ResetTouchpad()
		{
			touchId = -1;

			IsActive = false;

			_lastPosition = Vector2.zero;
			_currentPosition = Vector2.zero;

			Delta = Vector2.zero;
		}
	}
}
