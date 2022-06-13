using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _anim;

    private AudioSource _audioSource;
    [SerializeField]
    private float _fireRate = 6.0f;
    [SerializeField]
    private float canFire = -1;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player is NUll!");
        }

        _anim = GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("Animator is NULL!");
        }
    }

    // Update is called once per frame
    void Update()
    {
       CalculateMovement();

        if (Time.time > canFire)
        {
            _fireRate = Random.Range(10f, 15f);
            canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            float randomX = Random.Range(-2f, 15f);
            transform.position = new Vector3(randomX, 9, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("EnemyExplode");
            _speed = 0;
            Destroy(this.gameObject, .8f);
            _audioSource.Play();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            
            if (_player != null)
            {
                _player.AddScore(1);
            }
            _anim.SetTrigger("EnemyExplode");
            _speed = 0;
            Destroy(this.gameObject, .8f);
            Destroy(GetComponent<Collider2D>());
            _audioSource.Play();
        }

        if (other.tag == "HomingLaserPlayer")
        {
            if(_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("EnemyExplode");
            _speed = 0;
            Destroy(this.gameObject, .8f);
            Destroy(GetComponent<Collider2D>());
            _audioSource.Play();
        }
    }
}
