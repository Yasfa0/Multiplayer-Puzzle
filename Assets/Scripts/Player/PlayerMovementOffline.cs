using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovementOffline : MonoBehaviour
{
    [SerializeField] List<string> inputAxes = new List<string>();
    [SerializeField] KeyCode grabKey;
    [SerializeField] KeyCode runKey;
    [SerializeField] KeyCode jumpKey;
    private GameObject currentItem;
    List<GameObject> captureFollow = new List<GameObject>();
    float horizontalInput, verticalInput;
    Vector3 moveDir;
    [SerializeField] private PlayerGrabArea grabArea;
    [SerializeField] private GameObject front;
    [SerializeField] private float speed = 10f;
    private float baseSpeed;
    CharacterController charaController;
    float turnSmooth;

    //bool isIdle = true;
    //bool idleSetup = false;

    float lastIdle;
    float tickDuration = 2f;

    bool isGrounded;
    Vector3 playerVelocity;
    float gravityValue = -9.81f; 

    [SerializeField] Animator anim;
    bool canMove = true;

    private void Awake()
    {
        baseSpeed = speed;
        //anim = GetComponentInChildren<Animator>();
        charaController = GetComponent<CharacterController>();
    }

    private void Update()
    {
            MovementControl();   
    }

    public void MovementControl()
    {
        isGrounded = charaController.isGrounded;
        horizontalInput = Input.GetAxisRaw(inputAxes[0]);
        verticalInput = Input.GetAxisRaw(inputAxes[1]);
        moveDir = new Vector3(horizontalInput, playerVelocity.y, verticalInput).normalized;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if(isGrounded && playerVelocity.y < 0)
        {
            //Reset gravity effect to zero
            playerVelocity.y = 0;
        }

        //Still buggy
        /*if (isGrounded &&  Input.GetKey(jumpKey))
        {
            playerVelocity.y = Mathf.Sqrt(5f * -3f * gravityValue);
           //rb.velocity = Vector3.up * 100f;
        }*/

        playerVelocity.y += gravityValue * Time.deltaTime;

        /*if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (2.5f - 1) * Time.deltaTime; 
        }else if (rb.velocity.y > 0 && !Input.GetKey(jumpKey))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (2f - 1) * Time.deltaTime;
        }*/

        if (Input.GetKeyDown(runKey))
        {
            speed = baseSpeed * 2;
            Debug.Log("Current Speed" + speed);
        }else if (Input.GetKeyUp(runKey))
        {
            speed = baseSpeed;
            Debug.Log("Current Speed" + speed);
        }

        if (moveDir.magnitude == 0)
        {
            //Debug.Log("Player Idling");
            //anim.SetInteger("animState", 0);
            if (Time.time - lastIdle >= tickDuration)
            {
                //Debug.Log("Idling Heal");
                lastIdle = Time.time;
            }
        }
        else
        {
            //anim.SetInteger("animState", 1);
            //idleSetup = false;
            //Debug.Log("Player not Idling");
        }

        if (horizontalInput == 0 && verticalInput == 0)
        {
            //Debug.Log("Character not moving");
            anim.SetFloat("moveSpeed", 0);
        }else if (moveDir.magnitude >= 0.1f && canMove)
        {
            //if (!playerWeapon.GetIsAiming())
            //{
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, 0.1f);
            transform.rotation = Quaternion.Euler(transform.rotation.x, smoothAngle, transform.rotation.z);
            //}


            //transform.Translate(moveDir * speed * Time.deltaTime);
            //rb.velocity = moveDir * speed * Time.deltaTime;
            charaController.Move(moveDir * speed * Time.deltaTime);
            anim.SetFloat("moveSpeed", speed);
        }

        if (Input.GetKeyDown(grabKey))
        {

            Debug.Log("E Pressed");
            GrabNDrop();
        }
    }

    private IEnumerator WaitMove(float delay)
    {
        canMove = false;
        speed = 0;
        yield return new WaitForSeconds(delay);
        canMove = true;
        speed = baseSpeed;
    }

    public void GrabNDrop()
    {

        if (currentItem != null)
        {
            anim.SetBool("isGrabbing", false);
            StartCoroutine(WaitMove(2f));
            Debug.Log("Dropping");
            //Currently holding item, if pressed, drop off.
            currentItem.gameObject.transform.parent = null;
            currentItem = null;
        }
        else
        {
            //Not holding item, if pressed, pick up
            if (grabArea.GetInGrabArea() != null)
            {
                anim.SetBool("isGrabbing", true);
                StartCoroutine(WaitMove(2f));
                Debug.Log("Grabbing");
                GameObject tempItem = grabArea.GetInGrabArea();
                currentItem = tempItem;
                currentItem.transform.position = front.transform.position;
                currentItem.transform.parent = transform;
            }
        }
    }

    public void AdjustSpeed(float val)
    {
        speed += val;
    }

}
