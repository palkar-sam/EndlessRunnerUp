using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCircle : MonoBehaviour
{
    [SerializeField] private Rigidbody myBody;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float speed;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float jumpSpeed;


    

    // Update is called once per frame
    void Update()
    {
        var inputVal = Input.GetAxisRaw("Horizontal");
        
        if(Input.GetButtonDown("Jump") && IsGrounded())
            myBody.velocity = new Vector3(myBody.velocity.x, jumpSpeed * Time.deltaTime, myBody.velocity.z);


        var xVal = dodgeSpeed * inputVal * Time.deltaTime;
        myBody.velocity = new Vector3(xVal, myBody.velocity.y, speed * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        //RaycastHit raycastHit = Physics.SphereCast(sphereCollider.bounds.center, sphereCollider.bounds.size.x / 2,100.0f, playerLayer, QueryTriggerInteraction.Collide);

        return transform.position.y <= 0.5;
    }
}
