using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTransform : MonoBehaviour {

	private string MoveInputAxis = "Vertical";
	private string TurnInputAxis = "Horizontal";

    // rotation that occurs in angles per second holding down input
    public float rotationRate = 360;

    // units moved per second holding down move input
    public float moveSpeed = 2;

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
		transform.Translate(Vector3.forward * input * moveSpeed);
	}

    private void Turn(float input)
    {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
    }
}