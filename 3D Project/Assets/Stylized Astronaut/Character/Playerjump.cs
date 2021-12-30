using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerjump : MonoBehaviour
{
    private Rigidbody playerRigidbody;

    public float jumpPower;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerJump();
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}
