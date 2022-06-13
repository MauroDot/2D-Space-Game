using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    private CameraShake _cameraShake;
    // Start is called before the first frame update
    void Start()
    {
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        StartCoroutine(_cameraShake.Shake(0.5f, 0.32f));
        Destroy(this.gameObject, 1f);
    }
}
