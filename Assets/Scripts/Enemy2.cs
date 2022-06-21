using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _anim;

    private AudioSource _audioSource;
    [SerializeField]
    private float _fireRate = 10.0f;
    [SerializeField]
    private float canFire = -1;
    //private int _moveID;
    [SerializeField]
    private GameObject _shield;
    private SpawnManager _spawnManager;

    [SerializeField]
    //private int _enemyType;
    private float _detectRange = 6f;
    [SerializeField]
    private int _ramSpeed = 5;
    [SerializeField]
    private int _enemyID;

    [SerializeField]
    private bool _isEnemyShieldActive;

    // Start is called before the first frame update
    void Start()
    {
        _fireRate = 10.0f;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        //_moveID = Random.Range(1, 4);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        //_enemyType = Random.Range(0, 2);
        _enemyID = Random.Range(0, 3);
        _detectRange = 6f;
        _isEnemyShieldActive = true;

        if (_player == null)
        {
            Debug.LogError("Player is NUll!");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        EnemyID();
        BaseEnemy();
        DeactivateShield();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);

        if (transform.position.x < -15f)
        {
            float randomX = Random.Range(-2f, 15f);
            transform.position = new Vector3(randomX, 15, 1);
        }

    }

    private void DeactivateShield()
    {
        _shield.SetActive(false);

        _isEnemyShieldActive = false;
        return;
    }

    void EnemyID()
    {
        switch (_enemyID)
        {
            case 0:
                BaseEnemy();
                break;
            case 1:
                RamEnemy();
                break;
            default:
                Debug.Log("Default Value");
                break;
        }
    }

    void BaseEnemy()
    {
        if (Time.time > canFire)
        {
            _fireRate = Random.Range(4f, 6f);
            canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void RamEnemy()
    {
        if (_player != null)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < _detectRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _ramSpeed * Time.deltaTime);
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Player")
        {
            if (_isEnemyShieldActive == true)
            {
                DeactivateShield();
            }
            else
            {
                Destroy(this.gameObject);
            }

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
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
            _ramSpeed = 0;
            GetComponent<Collider2D>().enabled = false;
            _audioSource.Play();
            _spawnManager.EnemyDead();
            Destroy(this.gameObject, .8f);

        }
    }
}

