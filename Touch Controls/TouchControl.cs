using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WinterboltGames.TouchInput.Controls
{
	public abstract class TouchControl : MonoBehaviour
	{
		protected GraphicRaycaster graphicRaycaster;

		protected EventSystem eventSystem;

		protected int fingerId = -1;

		protected int touchIndex = -1;

		protected virtual void Start()
		{
			graphicRaycaster = FindObjectOfType<GraphicRaycaster>();

			eventSystem = FindObjectOfType<EventSystem>();
		}

		protected virtual void Update()
		{
			touchIndex = -1;

			if (fingerId == -1)
			{
				foreach ((int i, Touch touch) in TouchInput.NonAllocatingIndexedTouchesIterator())
				{
					PointerEventData eventData = new(eventSystem)
					{
						position = touch.position,
					};

					List<RaycastResult> results = new();

					graphicRaycaster.Raycast(eventData, results);

					foreach (RaycastResult result in results)
					{
						if (result.gameObject == gameObject)
						{
							fingerId = touch.fingerId;

							touchIndex = i;

							break;
						}
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
		}
	}
}
