using System.Collections.Generic;
using UnityEngine;

namespace WinterboltGames.TouchInput.Scripts
{
	public sealed class TouchInputManager : MonoBehaviour
	{
		public static IEnumerable<(int, Touch)> NonAllocatingIndexedTouchesIterator()
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				yield return (i, Input.GetTouch(i));
			}
		}
	}
}
