using System;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

#if ENABLE_INPUT_SYSTEM

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

#endif

namespace WinterboltGames.TouchInput.Scripts
{
	public static class TouchInput
	{
		private const string TouchUnavailableExceptionMessage = "Cannot get touches without a touchscreen. Consider using the Device Simulator.";

		public static int GetTouchCount()
		{

#if ENABLE_INPUT_SYSTEM

			return EnhancedTouchSupport.enabled ? Touch.activeTouches.Count : Touchscreen.current?.touches.Count ?? 0;

#else

			return Input.touchCount;

#endif

		}

#if ENABLE_INPUT_SYSTEM

		public static SimpleTouch ToSimpleTouch(Touch touch)
		{
			int id = touch.touchId;

			Vector2 position = touch.screenPosition;

			SimpleTouchPhase phase = touch.phase switch
			{
				TouchPhase.None => SimpleTouchPhase.None,
				TouchPhase.Began => SimpleTouchPhase.Began,
				TouchPhase.Stationary => SimpleTouchPhase.Stationary,
				TouchPhase.Moved => SimpleTouchPhase.Moved,
				TouchPhase.Ended => SimpleTouchPhase.Ended,
				TouchPhase.Canceled => SimpleTouchPhase.Canceled,

				_ => throw new InvalidEnumArgumentException(nameof(touch.phase), (int)touch.phase, typeof(TouchPhase)),
			};

			return new SimpleTouch(id, position, phase);
		}

		public static SimpleTouch ToSimpleTouch(TouchState touch)
		{
			int id = touch.touchId;

			Vector2 position = touch.position;

			SimpleTouchPhase phase = touch.phase switch
			{
				TouchPhase.None => SimpleTouchPhase.None,
				TouchPhase.Began => SimpleTouchPhase.Began,
				TouchPhase.Stationary => SimpleTouchPhase.Stationary,
				TouchPhase.Moved => SimpleTouchPhase.Moved,
				TouchPhase.Ended => SimpleTouchPhase.Ended,
				TouchPhase.Canceled => SimpleTouchPhase.Canceled,

				_ => throw new InvalidEnumArgumentException(nameof(touch.phase), (int)touch.phase, typeof(TouchPhase)),
			};

			return new SimpleTouch(id, position, phase);
		}

		public static SimpleTouch ToSimpleTouch(TouchControl touch)
		{
			return ToSimpleTouch(touch.ReadValue());
		}

#else

		public static SimpleTouch ToSimpleTouch(Touch touch)
		{
			int id = touch.fingerId;

			Vector2 position = touch.position;

			SimpleTouchPhase phase = touch.phase switch
			{
				TouchPhase.Began => SimpleTouchPhase.Began,
				TouchPhase.Stationary => SimpleTouchPhase.Stationary,
				TouchPhase.Moved => SimpleTouchPhase.Moved,
				TouchPhase.Ended => SimpleTouchPhase.Ended,
				TouchPhase.Canceled => SimpleTouchPhase.Canceled,

				_ => throw new InvalidEnumArgumentException(nameof(touch.phase), (int)touch.phase, typeof(TouchPhase)),
			};

			return new SimpleTouch(id, position, phase);
		}

#endif

		public static SimpleTouch GetTouchByIndex(int index)
		{
			if (index < 0 || index > GetTouchCount())
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			int id = -1;

			Vector2 position = Vector2.zero;

			SimpleTouchPhase phase = SimpleTouchPhase.None;

#if ENABLE_INPUT_SYSTEM

			if (EnhancedTouchSupport.enabled)
			{
				Touch touch = Touch.activeTouches[index];

				id = touch.touchId;

				position = touch.screenPosition;

				phase = touch.phase switch
				{
					TouchPhase.None => SimpleTouchPhase.None,
					TouchPhase.Began => SimpleTouchPhase.Began,
					TouchPhase.Stationary => SimpleTouchPhase.Stationary,
					TouchPhase.Moved => SimpleTouchPhase.Moved,
					TouchPhase.Ended => SimpleTouchPhase.Ended,
					TouchPhase.Canceled => SimpleTouchPhase.Canceled,

					_ => throw new InvalidEnumArgumentException(nameof(touch.phase), (int)touch.phase, typeof(TouchPhase)),
				};
			}
			else
			{
				TouchState touch = Touchscreen.current?.touches[index].ReadValue() ?? throw new InvalidOperationException(TouchUnavailableExceptionMessage);

				id = touch.touchId;

				position = touch.position;

				phase = touch.phase switch
				{
					TouchPhase.None => SimpleTouchPhase.None,
					TouchPhase.Began => SimpleTouchPhase.Began,
					TouchPhase.Stationary => SimpleTouchPhase.Stationary,
					TouchPhase.Moved => SimpleTouchPhase.Moved,
					TouchPhase.Ended => SimpleTouchPhase.Ended,
					TouchPhase.Canceled => SimpleTouchPhase.Canceled,

					_ => throw new InvalidEnumArgumentException(nameof(touch.phase), (int)touch.phase, typeof(TouchPhase)),
				};
			}

#else

			Touch touch = Input.GetTouch(index);

			id = touch.fingerId;

			position = touch.position;

			phase = touch.phase switch
			{
				TouchPhase.Began => SimpleTouchPhase.Began,
				TouchPhase.Stationary => SimpleTouchPhase.Stationary,
				TouchPhase.Moved => SimpleTouchPhase.Moved,
				TouchPhase.Ended => SimpleTouchPhase.Ended,
				TouchPhase.Canceled => SimpleTouchPhase.Canceled,

				_ => throw new InvalidEnumArgumentException(nameof(touch.phase), (int)touch.phase, typeof(TouchPhase)),
			};

#endif

			return new SimpleTouch(id, position, phase);

		}

		public static IEnumerable<(int, SimpleTouch)> NonAllocatingIndexedTouchesIterator()
		{
			for (int i = 0; i < GetTouchCount(); i++)
			{

#if ENABLE_INPUT_SYSTEM

				if (EnhancedTouchSupport.enabled)
				{
					yield return (i, ToSimpleTouch(Touch.activeTouches[i]));
				}
				else
				{
					yield return (i, ToSimpleTouch(Touchscreen.current?.touches[i] ?? throw new InvalidOperationException(TouchUnavailableExceptionMessage)));
				}

#else

				yield return (i, ToSimpleTouch(Input.GetTouch(i)));

#endif

			}
		}
	}
}
