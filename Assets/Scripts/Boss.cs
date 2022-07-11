using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static Boss instance;

    public BossAction[] actions;
    public int currentAction;
    private float actionCounter;
    private float shotCounter;
    private Vector2 moveDirection;
    public Rigidbody2D theRB;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        actionCounter = actions[currentAction].actionLength;
    }

    void Update()
    {
        if(actionCounter > 0)
        {
            actionCounter-= Time.deltaTime;

            moveDirection = Vector2.zero;

            if(actions[currentAction].shouldMove)
            {
                if(actions[currentAction].shouldChasePlayer)
                {
                    moveDirection = Player.instance.transform.position - transform.position;
                    moveDirection.Normalize();
                }

                if(actions[currentAction].moveToPoints)
                {
                    moveDirection = actions[currentAction].pointToMoveTo.position - transform.position;
                }
            }

            theRB.velocity = moveDirection * actions[currentAction].moveSpeed;
        }
        else
        {
            currentAction++;
            if(currentAction >= actions.Length)
            {
                currentAction = 0;
            }

            actionCounter = actions[currentAction].actionLength;
        }
    }
}

[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength;

    public bool shouldMove;
    public bool shouldChasePlayer;
    public float moveSpeed;
    public bool moveToPoints;
    public Transform pointToMoveTo;

    public bool shouldShoot;
    public GameObject itemToShoot;
    public float timeBetweenShots;
    public Transform[] shotPoints;
}

