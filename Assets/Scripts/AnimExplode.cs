using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimExplode : MonoBehaviour
{
    [SerializeField] private float animSpeed;
    void Start()
    {
        StartCoroutine(destoryAnim());
    }
    private IEnumerator destoryAnim(){
        yield return new WaitForSeconds(animSpeed);
        Destroy(this.gameObject);
    }
    
}
