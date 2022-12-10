using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

	public float speed = 4f;
	public float mouseSensitivity = 0.1f;
	Rigidbody rb;
	Vector2 move;
    float cameraY;
	public Transform cameraTransform;

	void Awake()
	{
        // get reference to Rigidbody component
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
        // update player position every frame
		rb.velocity = (transform.forward * move.y + transform.right * move.x) * speed + new Vector3 (0, rb.velocity.y, 0);
	}

	public void Walk(InputAction.CallbackContext context)
	{
        // if move keys are pressed, read and store new input value in variable
		if (context.performed)
			move = context.ReadValue<Vector2>();
		if (context.canceled)
			move = Vector2.zero;
	}

	public void Look(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
            // read mouse input from current frame
            Vector2 mouseDelta = context.ReadValue<Vector2>();
            
            // apply mouse sensitivity
            mouseDelta *= mouseSensitivity;
            
            // rotate player along Y axis
			transform.Rotate(0, mouseDelta.x, 0);
            
            // the rotation of the camera is the opposite axis to the axis of the mouse so we subtract
            cameraY -= mouseDelta.y;
            // make sure that player cannot raise and lower camera beyond given range
            cameraY = Mathf.Clamp (cameraY, -60f, 50f);
            // apply camera rotation
			cameraTransform.transform.localRotation = Quaternion.Euler(cameraY, 0, 0);
		}
	}

}
