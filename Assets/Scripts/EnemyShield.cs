using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    public void EnemyShieldOn()
    {
        gameObject.SetActive(true);
    }

    public void EnemyShieldOff()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            EnemyShieldOff();
        }

        if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            EnemyShieldOff();
        }
    }
}
