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

    [SerializeField]
    private GameObject _toPlayer;
    private Vector3 _toPlayerDirection;

    //rare system
    [Header("0 = Common, 1 = Uncommon, 2 = Rare")]
    [SerializeField]
    private int _rare;
    void Start()
    {
        if(GameObject.Find("Player") !=null)
        {
            _toPlayer = GameObject.Find("Player");
        }
        else
        {
            Debug.Log("Powerup:: Start() - _toPlayer is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal;
        float moveVertical;

        if(Input.GetKey(KeyCode.H))
        {
            if (transform.position.x < _toPlayer.transform.position.x)
            {
                //move to right
                moveHorizontal = 3;
            }
            else
            {
                //move to left
                moveHorizontal = -3;
            }

            if(transform.position.y < _toPlayer.transform.position.y)
            {
                //move up
                moveVertical = 3;
            }
            else
            {
                //move down
                moveVertical= -3;
            }

            _toPlayerDirection = new Vector3(moveHorizontal, moveVertical, 0);
            transform.Translate(_toPlayerDirection * (_speed - 1) * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        

        if(transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    //rare system
    public int GetRare()
    {
        return _rare;
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
                        player.FlankCannonsActive();
                        break;
                    case 8:
                        player.SlowSpeedActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;

                }
            }
            Destroy(this.gameObject);
        }
        else
        {
            other.gameObject.TryGetComponent<Laser>(out Laser _laser);

            if(_laser !=null)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

