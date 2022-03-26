using UnityEngine;
using UnityEngine.Events;

namespace WinterboltGames.TouchInput.Controls
{
	public sealed class Thumbstick : TouchControl
	{
		[SerializeField]
		private RectTransform thumb;

		[SerializeField]
		private float maxThumbDistance;

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

		public Vector2 Input { get; private set; }

		protected override void Update()
		{
			base.Update();

			if (touchIndex == -1)
			{
				ResetThumbstick();
			}
			else
			{
				Touch touch = UnityEngine.Input.GetTouch(touchIndex);

				if (touch.phase == TouchPhase.Began)
				{
					IsActive = true;
				}
				else if (touch.phase is TouchPhase.Canceled or TouchPhase.Ended)
				{
					ResetThumbstick();
				}

				if (_isActive)
				{
					_ = RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, touch.position, null, out Vector2 position);

					position = Vector2.ClampMagnitude(position, maxThumbDistance);

					thumb.anchoredPosition = position;

					Input = new(position.x / maxThumbDistance, position.y / maxThumbDistance);
				}
				else
				{
					thumb.anchoredPosition = Vector2.zero;
				}
			}
		}

		private void ResetThumbstick()
		{
			fingerId = -1;

			IsActive = false;

			thumb.anchoredPosition = Vector2.zero;
		}
	}
}
