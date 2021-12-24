using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioJump : MonoBehaviour
{
    private Rigidbody2D playerRigidbody2D;

    public float jumpPower;
    public float speed;
    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }

  
    void Update()
    {
        MovePlayer();
        PlayerJump();
    }
    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * (x * speed * Time.deltaTime));
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            playerRigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag != "Floor")
        {
            Up_Down(col);
        }
        
    }

    void Up_Down(Collision2D col)
    {
        Vector3 distVec = transform.position - col.transform.position;
        if (Vector3.Cross(col.transform.right, distVec).z > 0)
        {
            Debug.Log("Up : º® À§¿¡ ºÎµúÇû½À´Ï´Ù!");
            return;
        }
        Debug.Log("Down :  º® ¾Æ·¡¿¡ ºÎµúÇû½À´Ï´Ù!");
    }
}
