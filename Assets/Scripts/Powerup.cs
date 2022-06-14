using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
   
    [SerializeField] 
    private int powerupID;
    [SerializeField]
    private AudioClip _audioClip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_audioClip, transform.position, 2f);

            if(player != null)
            {
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.BoosterShotActive();
                        break;
                    case 4:
                        player.HealPickupActive();
                        break;
                    case 5:
                        player.RefillAmmo();
                        break;
                    case 6:
                        player.PlayerHomingLaser();
                        break;
                    case 7:
                        player.FlankCannonsACtive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;

                }
            }
            Destroy(this.gameObject);
        }
    }
}
