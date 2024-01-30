using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
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

        if (Input.GetKeyDown(KeyCode.E))
        {

            if (currentItem != null)
            {
                //Currently holding item, if pressed, drop off.
                currentItem.gameObject.transform.parent = null;
                currentItem = null;
            }
            else
            {
                //Not holding item, if pressed, pick up
                //RaycastHit[] hits =  Physics.RaycastAll(gameObject.transform.position, gameObject.transform.forward, 5f);

                
                //RaycastHit[] hits = Physics.SphereCastAll(gameObject.transform.position, 0.5f, gameObject.transform.forward);
                //List<GameObject> validItems = new List<GameObject>();

                //Debug.Log("Hits Length: " + hits.Length);

                //foreach (RaycastHit item in hits)
                //{
                 //   Debug.Log(item.transform.gameObject.name);
                //}

                //for (int i = 0; i < hits.Length; i++)
                //{
                  //  if (hits[i].transform.gameObject.tag == "Item")
                   // {
                     //   validItems.Add(hits[i].transform.gameObject);
                    //}
                    //i++;
                //}

                //float closestDistance = Mathf.Infinity;
                //GameObject closestItem = null;

                //for (int i = 0; i < validItems.Count; i++)
                //{
                  //  float magDistance = Vector3.Magnitude(validItems[i].transform.position - gameObject.transform.position);
                    //if (magDistance < closestDistance)
                    //{
                      //  closestDistance = magDistance;
                       // closestItem = validItems[i];
                    //}
                    //i++;
                //}

                //if (closestItem != null)
                //{
                 //   closestItem.transform.position = front.transform.position;
                 //   closestItem.transform.parent = front.transform.parent;
                 //   currentItem = closestItem;
                //}

                if(grabArea.GetInGrabArea() != null)
                {
                    GameObject tempItem = grabArea.GetInGrabArea();
                    currentItem = tempItem;
                    currentItem.transform.position = front.transform.position;
                    currentItem.transform.parent = front.transform.parent;
                }
            }

            

        }
        
    }

    public void AdjustSpeed(float val)
    {
        speed += val;
    }

}
