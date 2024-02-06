using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Material offMaterial;
    [SerializeField] Material onMaterial;
    MeshRenderer renderer;
    bool activated = false;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.material = offMaterial;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            activated = true;
            renderer.material = onMaterial;
            door.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            activated = false;
            renderer.material = offMaterial;
            door.SetActive(true);
        }
    }

}
