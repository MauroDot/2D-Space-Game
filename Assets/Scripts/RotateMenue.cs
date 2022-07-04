using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMenue : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime);
    }
}
