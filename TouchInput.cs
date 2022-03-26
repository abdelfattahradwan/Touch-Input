using System.Collections.Generic;
using UnityEngine;

namespace WinterboltGames.TouchInput
{
	public static class TouchInput
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
