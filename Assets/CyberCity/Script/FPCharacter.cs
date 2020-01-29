using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPCharacter : MonoBehaviour
{
	public float speed = 2.0f;
    public float speedfast = 50.0f;
	public float gravity = -9.8f;
    float _speedfast;

	private CharacterController _charController;
	
	void Start() 
    {
		_charController = GetComponent<CharacterController>();
	}
	
	void Update() 
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speedfast = speedfast;
        }
        else _speedfast = 1;
        float deltaX = Input.GetAxis("Horizontal") * speed * _speedfast;
        float deltaZ = Input.GetAxis("Vertical") * speed * _speedfast;
		Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speedfast);
		movement.y = gravity;
        movement *= Time.deltaTime;
		movement = transform.TransformDirection(movement);
		_charController.Move(movement);
	}
}
