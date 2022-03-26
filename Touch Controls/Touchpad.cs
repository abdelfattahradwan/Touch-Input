using UnityEngine;
using UnityEngine.Events;

namespace WinterboltGames.TouchInput.Controls
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

			if (fingerId == -1)
			{
				foreach ((int i, Touch touch) in TouchInput.NonAllocatingIndexedTouchesIterator())
				{
					if (RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, touch.position))
					{
						fingerId = touch.fingerId;

						touchIndex = i;

						break;
					}

					if (touchIndex != -1) break;
				}
			}
			else
			{
				foreach ((int i, Touch touch) in TouchInput.NonAllocatingIndexedTouchesIterator())
				{
					if (touch.fingerId == fingerId)
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
				Touch touch = Input.GetTouch(touchIndex);

				if (touch.phase == TouchPhase.Began)
				{
					IsActive = true;

					_lastPosition = touch.position;
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					_currentPosition = touch.position;

					Delta = _currentPosition - _lastPosition;

					_lastPosition = _currentPosition;
				}
				else if (touch.phase == TouchPhase.Stationary)
				{
					Delta = Vector2.zero;
				}
				else if (touch.phase is (TouchPhase.Canceled or TouchPhase.Ended))
				{
					ResetTouchpad();
				}
			}
		}

		private void ResetTouchpad()
		{
			fingerId = -1;

			IsActive = false;

			_lastPosition = Vector2.zero;
			_currentPosition = Vector2.zero;

			Delta = Vector2.zero;
		}
	}
}
