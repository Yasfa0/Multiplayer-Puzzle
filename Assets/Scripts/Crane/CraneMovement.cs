using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneMovement : MonoBehaviour
{
    [SerializeField] Transform crane;
    [SerializeField] private float speed = 10f;
    float horizontalInput, verticalInput;
    Vector3 moveDir;
    CharacterController charaController;

    private void Awake()
    {
        charaController = crane.GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovementControl();
    }

    private void MovementControl()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        moveDir = new Vector3(horizontalInput, 0, verticalInput).normalized;

        //crane.position = new Vector3(crane.position.x, 1, crane.position.z);

        if (moveDir.magnitude == 0)
        {
        }
        else
        {
            //anim.SetInteger("animState", 1);
            //idleSetup = false;
            //Debug.Log("Player not Idling");
        }

        if (moveDir.magnitude >= 0.1f)
        {
            //if (!playerWeapon.GetIsAiming())
            //{
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            
            //Do not rotate yet
            //float smoothAngle = Mathf.SmoothDampAngle(crane.eulerAngles.y, targetAngle, ref turnSmooth, 0.1f);
            //crane.rotation = Quaternion.Euler(crane.rotation.x, smoothAngle, crane.rotation.z);
            
            //}


            //crane.Translate(moveDir * speed * Time.deltaTime);
            //rb.velocity = moveDir * speed * Time.deltaTime;
            charaController.Move(moveDir * speed * Time.deltaTime);
        }
    }
}
