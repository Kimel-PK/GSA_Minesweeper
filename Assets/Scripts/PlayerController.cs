using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

	public float walkingSpeed = 4f;
	public float sprintingSpeed = 7f;
	public float speed;
	public float mouseSensitivity = 0.1f;
	Rigidbody rb;
	Vector2 move;
	float cameraY;
	public Transform cameraTransform;
    AudioSource explosionAudio;

    void Awake()
	{
		// get reference to Rigidbody component
		rb = GetComponent<Rigidbody>();
        explosionAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
		speed = walkingSpeed;
    }

    void OnEnable()
    {
		GameManager.Singleton.onMineExplosion += PlayerDeath;
    }

    void OnDisable()
    {
        GameManager.Singleton.onMineExplosion -= PlayerDeath;
    }

    void Update()
	{
		ApplyMovement();
		MakeStep();
	}

	void ApplyMovement()
	{
		// update player position every frame
		if (!GameManager.Singleton.paused)
			rb.velocity = (transform.forward * move.y + transform.right * move.x) * speed + new Vector3(0, rb.velocity.y, 0);
	}

	void MakeStep()
	{
		Debug.DrawRay(transform.position, -transform.up, Color.blue, 0.1f);

		RaycastHit rHit;
		if (!Physics.Raycast(transform.position, -transform.up, out rHit, 1f) || rHit.collider.tag != "Tile")
			return;

		rHit.collider.GetComponent<Tile>().Dig();
	}

	public void Walk(InputAction.CallbackContext context)
	{
		if (GameManager.Singleton.gameOver)
		{
			move = Vector2.zero;
			return;
		}

		// if move keys are pressed, read and store new input value in variable
		if (context.performed)
			move = context.ReadValue<Vector2>();
		if (context.canceled)
			move = Vector2.zero;
	}

	public void Sprint(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			speed = sprintingSpeed;
		}
        if (context.canceled)
        {
			speed = walkingSpeed;
		}
	}

	public void Look(InputAction.CallbackContext context)
	{
		if (!context.performed || GameManager.Singleton.paused || GameManager.Singleton.gameOver)
			return;

		// read mouse input from current frame
		Vector2 mouseDelta = context.ReadValue<Vector2>();

		// apply mouse sensitivity
		mouseDelta *= mouseSensitivity;

		// rotate player along Y axis
		transform.Rotate(0, mouseDelta.x, 0);

		// the rotation of the camera is the opposite axis to the axis of the mouse so we subtract
		cameraY -= mouseDelta.y;
		// make sure that player cannot raise and lower camera beyond given range
		cameraY = Mathf.Clamp(cameraY, -60f, 50f);
		// apply camera rotation
		cameraTransform.transform.localRotation = Quaternion.Euler(cameraY, 0, 0);
	}

	public void Dig(InputAction.CallbackContext context)
	{
		if (!context.performed || GameManager.Singleton.gameOver)
			return;

		Debug.DrawRay(cameraTransform.position, cameraTransform.forward, Color.red, 0.1f);

		RaycastHit rHit;
		if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out rHit, 3f) || rHit.collider.tag != "Tile")
			return;

		rHit.collider.GetComponent<Tile>().Dig();
	}

	public void PlaceFlag(InputAction.CallbackContext context)
	{
		if (!context.performed || GameManager.Singleton.gameOver)
			return;

		RaycastHit rHit;
		if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out rHit, 3f) || rHit.collider.tag != "Tile")
			return;

        // Cannot place a flag on a tile that was already digged.
        if (rHit.collider.GetComponent<Tile>().isChecked)
			return;

		if (rHit.collider.GetComponent<Tile>().ToggleFlag())
			GameUI.Singleton.PlacedFlags++;
		else
			GameUI.Singleton.PlacedFlags--;

		GameManager.Singleton.CheckIfGameIsWon();
	}

	void PlayerDeath()
    {
		explosionAudio.Play();

		// Make player less stable.
        rb.centerOfMass = new Vector3(0, 1, -0.2f);
        rb.constraints = RigidbodyConstraints.None;

        Vector3 toExplosion = new Vector3(GameManager.Singleton.mineExplosionPosition.x - transform.position.x, 0, GameManager.Singleton.mineExplosionPosition.z - transform.position.z);
        toExplosion = toExplosion.normalized;
        rb.AddExplosionForce(300, transform.position + toExplosion, 10);
    }

	public void Pause(InputAction.CallbackContext context)
	{
		GameUI.Singleton.Pause();
	}
}