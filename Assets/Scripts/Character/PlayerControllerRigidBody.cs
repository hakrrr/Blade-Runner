using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRigidBody : MonoBehaviour {

	private string MoveInputAxis = "Vertical";
	private string TurnInputAxis = "Horizontal";

    // rotation that occurs in angles per second holding down input
    public float rotationRate = 360;

    // units moved per second holding down move input
    public float moveRate = 10;

    private Rigidbody rb;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	private void Update () 
    {
		float moveAxis = Input.GetAxis(MoveInputAxis);
		float turnAxis = Input.GetAxis(TurnInputAxis);

        ApplyInput(moveAxis, turnAxis);
	}

    private void ApplyInput(float moveInput,
                            float turnInput) 
    {
		Move(moveInput);
		Turn(turnInput);
    }

    private void Move(float input) 
    {
        // Make sure to set drag high so the sliding effect is very minimal (5 drag is acceptable for now)

        // mention this trash function automatically converts to local space
        rb.AddForce(transform.forward * input * moveRate, ForceMode.Force);
    }

    private void Turn(float input)
    {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
    }
}