using UnityEngine;
using System.Collections;

public class WallPanels : MonoBehaviour
{
    public void PlayWallAnimation()
    {
        GetComponent<Animator>().SetTrigger("Activate");
    }
}