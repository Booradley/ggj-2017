using UnityEngine;
using System.Collections;

public class LockedDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Hammer")
        {

        }
    }
}