using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulatableObject : MonoBehaviour
{
    private Renderer _renderer;
    public Material selectedMat;
    private Material _standardMat;

    void Start() {
        _renderer = this.gameObject.GetComponent<Renderer>();
        _standardMat = _renderer.material;
    }

    public void Select() {
        _renderer.material = selectedMat;
    }

    public void Release() {
        _renderer.material = _standardMat;
    }
}
