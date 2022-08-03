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
    private GameObject _shield;
    [SerializeField]
    private bool _isShieldOn;

    // shot from behind
    [SerializeField]
    GameObject _backFireShot;

    [SerializeField]
    private int _lives = 600;

    private int maxHealth;

    private int shieldLives = 20;
    UIManager canvas;
    [SerializeField] private GameObject portal;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private GameObject bossExplode;

    private CameraShake _cameraShake;
    // starting enemy can shoot from behind player

    // Start is called before the first frame update
    void Start()
    {
        _fireRate = 10.0f;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        canvas = GameObject.Find("Canvas").GetComponent<UIManager>();
        _anim = GetComponent<Animator>();
        maxHealth = _lives;
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

        if(_spawnManager != null)
        {
            StartCoroutine(_spawnManager.SpawnPowerupRoutine());
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
        if (transform.position == _movePoints[_randomMovePoint].position)
        {
            _randomMovePoint = Random.Range(0, 5);
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
        _fireRate = Random.Range(.3f, 1f);
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
            shieldLives--;
            _player.AddScore(5);
            canvas.UpdateBossShield(shieldLives, 10);
            if (shieldLives <= 0)
            {
                _isShieldOn = false;
                _shield.SetActive(false);
                canvas.DisplayBossShield(false);
                canvas.DisplayBossHealth(true);
            }
        }
        else
        {
            _lives -= 10;
            _player.AddScore(10);
            canvas.UpdateBossHealth(_lives, maxHealth);
            if (_lives <= 0)
            {
                //_anim.SetTrigger("EnemyExplode");
                _speed = 0;
                GetComponent<Collider2D>().enabled = false;
                _audioSource.Play();
                _spawnManager.EnemyDead();
                if (_player != null)
                {
                    _player.AddScore(50);
                }
                bossExplode.SetActive(true);
                AudioSource.PlayClipAtPoint(explosion, transform.position, 8);
                canvas.DisplayBossHealth(false);
                StartCoroutine(bossDestoryed());
            }
        }
    }
    private IEnumerator bossDestoryed()
    {
        yield return new WaitForSeconds(.8f);
        AudioSource.PlayClipAtPoint(clip, transform.position, 8);
        bossExplode.SetActive(false);
        portal.SetActive(true);
        Destroy(this.gameObject);
        StopCoroutine(bossDestoryed());
    }
}

