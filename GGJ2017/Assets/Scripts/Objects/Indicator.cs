using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour
{
    [SerializeField]
    private Material _offMaterial;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }

    public void Display(Material material)
    {
        _renderer.material = material;
    }

    public void Clear()
    {
        _renderer.material = _offMaterial;
    }
}