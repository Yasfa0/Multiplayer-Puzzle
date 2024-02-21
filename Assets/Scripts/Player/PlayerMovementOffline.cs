using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovementOffline : MonoBehaviour
{
    [SerializeField] List<string> inputAxes = new List<string>();
    [SerializeField] KeyCode grabKey;
    private GameObject currentItem;
    List<GameObject> captureFollow = new List<GameObject>();
    float horizontalInput, verticalInput;
    Vector3 moveDir;
    [SerializeField] private PlayerGrabArea grabArea;
    [SerializeField] private GameObject front;
    [SerializeField] private float speed = 10f;
    CharacterController charaController;
    float turnSmooth;

    //bool isIdle = true;
    //bool idleSetup = false;

    float lastIdle;
    float tickDuration = 2f;

    //Animator anim;

    private void Awake()
    {
        //anim = GetComponentInChildren<Animator>();
        charaController = GetComponent<CharacterController>();
    }

    private void Update()
    {
            MovementControl();   
    }

    public void MovementControl()
    {
        horizontalInput = Input.GetAxisRaw(inputAxes[0]);
        verticalInput = Input.GetAxisRaw(inputAxes[1]);
        moveDir = new Vector3(horizontalInput, 0, verticalInput).normalized;

        transform.position = new Vector3(transform.position.x, 1, transform.position.z);

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

        if (moveDir.magnitude >= 0.1f)
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
        }

        if (Input.GetKeyDown(grabKey))
        {

            Debug.Log("E Pressed");
            GrabNDrop();
        }
    }

    public void GrabNDrop()
    {

        if (currentItem != null)
        {
            Debug.Log("Dropping");
            //Currently holding item, if pressed, drop off.
            currentItem.gameObject.transform.parent = null;
            currentItem = null;
        }
        else
        {
            Debug.Log("Grabbing");
            //Not holding item, if pressed, pick up
            if (grabArea.GetInGrabArea() != null)
            {
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
