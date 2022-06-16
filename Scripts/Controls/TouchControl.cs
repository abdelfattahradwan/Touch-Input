using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace WinterboltGames.TouchInput.Scripts.Controls
{
	public abstract class TouchControl : MonoBehaviour
	{
		protected GraphicRaycaster graphicRaycaster;

		protected EventSystem eventSystem;

		protected int touchId = -1;

		protected int touchIndex = -1;

		protected virtual void Start()
		{
			graphicRaycaster = FindObjectOfType<GraphicRaycaster>();

			eventSystem = FindObjectOfType<EventSystem>();
		}

		protected virtual void Update()
		{
			touchIndex = -1;

			if (touchId == -1)
			{
				for (int i = 0; i < Touch.activeTouches.Count; i++)
				{
					Touch touch = Touch.activeTouches[i];

					PointerEventData eventData = new(eventSystem)
					{
						position = touch.screenPosition,
					};

					List<RaycastResult> results = new();

					graphicRaycaster.Raycast(eventData, results);

					foreach (RaycastResult result in results)
					{
						if (result.gameObject == gameObject)
						{
							touchId = touch.touchId;

							touchIndex = i;

							break;
						}
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
		}
	}
}
