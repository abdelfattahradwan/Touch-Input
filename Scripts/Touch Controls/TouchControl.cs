using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

			UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
		}

		protected virtual void Update()
		{
			touchIndex = -1;

			if (touchId == -1)
			{
				foreach ((int i, SimpleTouch touch) in TouchInput.NonAllocatingIndexedTouchesIterator())
				{
					PointerEventData eventData = new(eventSystem)
					{
						position = touch.Position,
					};

					List<RaycastResult> results = new();

					graphicRaycaster.Raycast(eventData, results);

					foreach (RaycastResult result in results)
					{
						if (result.gameObject == gameObject)
						{
							touchId = touch.Id;

							touchIndex = i;

							break;
						}
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
		}
	}
}
