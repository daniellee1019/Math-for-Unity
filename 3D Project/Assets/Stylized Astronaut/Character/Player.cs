using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
		JoyStick joyStick;
		private Animator anim;
		private CharacterController controller;

		public float speed = 600.0f;
		public float turnSpeed = 400.0f;
		private Vector3 moveDirection = Vector3.zero;
		public float gravity = 20.0f;


		public void Start () 
			{
				controller = GetComponent <CharacterController>();
				anim = gameObject.GetComponentInChildren<Animator>();
			
			}

	public void Update()
	{
		if (joyStick.MoveFlag)
		{
			transform.Translate(transform.forward * Time.deltaTime * speed);
			anim.SetInteger("AnimationPar", 1);
		}
		else
		{
			anim.SetInteger("AnimationPar", 0);
		}

		if (controller.isGrounded)
		{
			moveDirection = transform.forward * Time.deltaTime * speed;
		}



	}
}
