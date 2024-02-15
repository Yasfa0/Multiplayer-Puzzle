using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneVerticality : MonoBehaviour
{
    [SerializeField] Transform crane;
    [SerializeField] private float speed = 10f;
    float verticalInput;
    Vector3 moveDir;
    CharacterController charaController;

    private void Awake()
    {
        charaController = crane.GetComponent<CharacterController>();
    }

    private void Update()
    {
        ControlVerticality();
    }

    public void ControlVerticality()
    {
        verticalInput = Input.GetAxisRaw("Vertical2");
        moveDir = new Vector3(0, verticalInput, 0).normalized;

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
