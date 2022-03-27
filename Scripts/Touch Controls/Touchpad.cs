using UnityEngine;
using UnityEngine.Events;

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
				foreach ((int i, SimpleTouch touch) in TouchInput.NonAllocatingIndexedTouchesIterator())
				{
					if (touch.Phase == SimpleTouchPhase.Began && RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, touch.Position))
					{
						touchId = touch.Id;

						touchIndex = i;

						break;
					}

					if (touchIndex != -1) break;
				}
			}
			else
			{
				foreach ((int i, SimpleTouch touch) in TouchInput.NonAllocatingIndexedTouchesIterator())
				{
					if (touch.Id == touchId)
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
				SimpleTouch touch = TouchInput.GetTouchByIndex(touchIndex);

				if (touch.Phase == SimpleTouchPhase.Began)
				{
					IsActive = true;

					_lastPosition = touch.Position;
				}
				else if (touch.Phase == SimpleTouchPhase.Moved)
				{
					_currentPosition = touch.Position;

					Delta = _currentPosition - _lastPosition;

					_lastPosition = _currentPosition;
				}
				else if (touch.Phase == SimpleTouchPhase.Stationary)
				{
					Delta = Vector2.zero;
				}
				else if (touch.Phase is SimpleTouchPhase.None or SimpleTouchPhase.Ended or SimpleTouchPhase.Canceled)
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
