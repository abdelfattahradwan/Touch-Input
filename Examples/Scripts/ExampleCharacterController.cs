using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WinterboltGames.TouchInput.Scripts.Controls;

namespace WinterboltGames.TouchInput.Examples.Scripts
{
	public sealed class ExampleCharacterController : MonoBehaviour
	{
		[SerializeField]
		private CharacterController characterController;

		[SerializeField]
		private float speed;

		[SerializeField]
		private float jumpSpeed;

		[SerializeField]
		private float gravityScale;

		[SerializeField]
		private GameObject topDownCameraGameObject;

		[SerializeField]
		private GameObject fpsCameraGameObject;

		[SerializeField]
		private float xLimit;

		[SerializeField]
		private Thumbstick movementThumbstick;

		[SerializeField]
		private TouchButton switchCameraButton;

		[SerializeField]
		private TouchButton jumpButton;

		[SerializeField]
		private Touchpad lookTouchpad;

		[SerializeField]
		private Thumbstick lookThumbstick;

		private Vector3 _velocity;

		private Vector3 _eulerAngles;

		private void Start()
		{
			switchCameraButton.OnPressed.AddListener(() =>
			{
				topDownCameraGameObject.SetActive(fpsCameraGameObject.activeSelf);

				fpsCameraGameObject.SetActive(!topDownCameraGameObject.activeSelf);
			});
		}

		private void Update()
		{
			Vector3 right = fpsCameraGameObject.activeSelf ? transform.right : Vector3.right;
			Vector3 forward = fpsCameraGameObject.activeSelf ? transform.forward : Vector3.forward;

			Vector3 desiredVelocity = Vector3.ClampMagnitude(((right * movementThumbstick.Input.x) + (forward * movementThumbstick.Input.y)) * speed, speed);

			_velocity.x = desiredVelocity.x;
			_velocity.z = desiredVelocity.z;

			if (characterController.isGrounded)
			{
				_velocity.y = 0.0f;

				if (jumpButton.IsPressed)
				{
					_velocity.y = jumpSpeed;
				}
			}
			else
			{
				_velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;
			}

			characterController.Move(_velocity * Time.deltaTime);

			if (fpsCameraGameObject.activeSelf)
			{
				_eulerAngles.x -= lookTouchpad.Delta.y * 4.0f * Time.deltaTime;

				_eulerAngles.x = Mathf.Clamp(_eulerAngles.x, -xLimit, xLimit);

				fpsCameraGameObject.transform.localEulerAngles = _eulerAngles;

				transform.Rotate(0.0f, lookTouchpad.Delta.x * 4.0f * Time.deltaTime, 0.0f, Space.World);
			}
			else
			{
				if (lookThumbstick.IsActive)
				{
					transform.rotation = Quaternion.LookRotation(new Vector3(lookThumbstick.Input.x, 0.0f, lookThumbstick.Input.y), Vector2.up);
				}				
			}
		}
	}
}
