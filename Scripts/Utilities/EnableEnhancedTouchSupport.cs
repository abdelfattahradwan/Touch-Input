using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace WinterboltGames.TouchInput.Scripts.Utilities
{
	public static class EnableEnhancedTouchSupport
    {
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
		{
            EnhancedTouchSupport.Enable();
		}
    }
}
