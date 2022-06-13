using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingLaserPlayer : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 14f;
    [SerializeField]
    private float _rotateSpeed = 350f;
    private GameObject _closeEnemy;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_closeEnemy == null)
        {
            _closeEnemy = FindCloseEnemy();
        }
        if (_closeEnemy != null)
        {
            MoveToEnemy();
        }
        else
        {
            transform.Translate(Vector3.up * (_laserSpeed / 3) * Time.deltaTime);
        }

        if (transform.position.y > 13)
        {
            Destroy(gameObject);
        }

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private GameObject FindCloseEnemy()
    {
        try
        {
            GameObject[] enemies;
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject close = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;

            foreach (GameObject enemy in enemies)
            {
                Vector3 other = enemy.transform.position - position;
                float curDistance = other.sqrMagnitude;
                if(curDistance < distance)
                {
                    close = enemy;
                    distance = curDistance;
                }
            }
            return close;
        }
        catch
        {
            return null;
        }
    }

    private void MoveToEnemy()
    {
        Vector2 direction = (Vector2)_closeEnemy.transform.position - _rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = -rotateAmount * _rotateSpeed;
        _rb.velocity = transform.up * _laserSpeed;
    }
}
        
