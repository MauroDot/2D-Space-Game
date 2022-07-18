using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBackFire : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.GetComponentInParent<Enemy>().BackFire();
            StartCoroutine(DetectCooldown());
        }
    }

    IEnumerator DetectCooldown()
    {
        transform.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        transform.gameObject.SetActive(true);
    }
}
