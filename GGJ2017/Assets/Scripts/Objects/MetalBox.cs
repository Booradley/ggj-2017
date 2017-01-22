using UnityEngine;
using System.Collections;

public class MetalBox : MonoBehaviour
{
    [SerializeField]
    private GameObject _door;

    public void OpenDoor()
    {
        _door.SetActive(false);
    }
}