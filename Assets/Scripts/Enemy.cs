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
    private int _moveID;
    [SerializeField]
    private GameObject _shield;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _moveID = Random.Range(1, 4);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

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
            _fireRate = Random.Range(15f, 20f);
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

        if (transform.position.y < -15f)
        {
            float randomX = Random.Range(-2f, 15f);
            transform.position = new Vector3(randomX, 15, 1);
        }

        switch(_moveID)
        {
            case 1:
                transform.Translate((Vector3.down + Vector3.left) * Time.deltaTime);
                break;
            case 2:
                transform.Translate((Vector3.down + Vector3.right) * Time.deltaTime);
                break;
            case 3:
                transform.Translate(Vector3.down * Time.deltaTime);
                break;
            default:
                break;
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
            Debug.Log("Laser Hit");
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(1);
            }
            _anim.SetTrigger("EnemyExplode");
            _speed = 0;
            GetComponent<Collider2D>().enabled = false;
            _audioSource.Play();
            _spawnManager.EnemyDead();
            Destroy(this.gameObject, .8f);
        }

    }
}
