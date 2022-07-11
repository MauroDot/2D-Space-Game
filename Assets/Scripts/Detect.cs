using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            transform.GetComponentInParent<Enemy>().Dodge();
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
