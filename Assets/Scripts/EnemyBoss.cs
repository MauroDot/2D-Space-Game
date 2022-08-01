using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField]
    Transform[] _movePoints;
    [SerializeField]
    int _randomMovePoint;

    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject frozenAsteroid;
    [SerializeField]
    private GameObject bossLaser;

    private Player _player;
    private Animator _anim;

    private AudioSource _audioSource;
    [SerializeField]
    private float _fireRate = 10.0f;
    [SerializeField]
    private float _canFire = -1;

    private SpawnManager _spawnManager;
    [SerializeField]
    private int _moveID;
    [SerializeField]
    private float _detectRange = 6f;
    [SerializeField]
    private int _ramSpeed = 5;
    [SerializeField]
    private int _enemyID;

    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private bool _isShieldOn;

    // shot from behind
    [SerializeField]
    GameObject _backFireShot;

    [SerializeField]
    private int _lives = 100;

    UIManager canvas;

    // starting enemy can shoot from behind player

    // Start is called before the first frame update
    void Start()
    {
        _fireRate = 10.0f;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _moveID = Random.Range(1, 3);
        _enemyID = Random.Range(0, 2);
        _detectRange = 6f;
        canvas = GameObject.Find("Canvas").GetComponent<UIManager>();
        _anim = GetComponent<Animator>();

        NormalFire();

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL!");
        }

        int s = Random.Range(1, 100);

        if (s >= 0)
        {
            _isShieldOn = true;
        }
        else
        {
            _isShieldOn = false;
        }
        _shield.SetActive(_isShieldOn);

        if (_player == null)
        {
            Debug.LogError("Player is NUll!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (_canFire < Time.time)
        {
            ShootSomething();
            NormalFire();
        }
    }
    void CalculateMovement()
    {
        if (transform.position == _movePoints[0].position || transform.position == _movePoints[1].position || transform.position == _movePoints[2].position)
        {
            _randomMovePoint = Random.Range(0, 3);
        }
        transform.position = Vector3.MoveTowards(transform.position, _movePoints[_randomMovePoint].position, _speed * Time.deltaTime);
    }
    private void ShootSomething()
    {
        int fireAsteroid = Random.Range(0, 2);
        switch (fireAsteroid)
        {
            case 0:
                var freezeAsteroid = Instantiate(frozenAsteroid, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                break;

            case 1:
                var laserShot = Instantiate(bossLaser, new Vector3(transform.position.x - 1.2f, transform.position.y - 1.4f, 0), Quaternion.identity);
                var laserShot1 = Instantiate(bossLaser, new Vector3(transform.position.x, transform.position.y - 1.4f, 0), Quaternion.identity);
                var laserShot2 = Instantiate(bossLaser, new Vector3(transform.position.x + 1.2f, transform.position.y - 1.4f, 0), Quaternion.identity);
                Laser laser = laserShot.GetComponent<Laser>();
                Laser laser1 = laserShot1.GetComponent<Laser>();
                Laser laser2 = laserShot2.GetComponent<Laser>();
                laser.AssignEnemyLaser();
                laser1.AssignEnemyLaser();
                laser2.AssignEnemyLaser();
                break;
        }
    }
    public void NormalFire()
    {
        _fireRate = Random.Range(1f, 4f);
        _canFire = Time.time + _fireRate;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            Damage();
        }

        if (other.tag == "Laser" && _backFireShot == true)
        {
            Destroy(other.gameObject);
            Damage();
        }
    }


    void Damage()
    {
        if (_isShieldOn == true)
        {
            _lives++;
            _isShieldOn = false;
            _shield.SetActive(false);

            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject, .28f);
            }
        }
        else
        {
            _lives -= 10;
            canvas.UpdateBossHealth(_lives,100);
            if (_lives <= 0)
            {
                _anim.SetTrigger("EnemyExplode");
                _speed = 0;
                _ramSpeed = 0;
                GetComponent<Collider2D>().enabled = false;
                _audioSource.Play();
                _spawnManager.EnemyDead();
                if (_player != null)
                {
                    _player.AddScore(25);
                }
                canvas.DisplayBossHealth(false);
                Destroy(this.gameObject, .8f);
            }
        }

    }
}

