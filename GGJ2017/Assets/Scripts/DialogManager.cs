using UnityEngine;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    private static DialogManager _instance;
    public static DialogManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DialogManager>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        // 
    }
}