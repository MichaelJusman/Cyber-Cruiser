using UnityEngine;
using System;
public class Enemy : GameBehaviour, IDamageable
{
    public enum movementDirection
    {
        Up, Down, Left, Right, DownLeft, None

    }

    public enum movementTypes
    {
        Forward, BackForth, None
    }

    public static event Action<GameObject> OnEnemyDied = null;

    [SerializeField] private movementDirection moveDirection;
    [SerializeField] private movementTypes moveType;

    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float speed;
    private Vector2 direction;
    [SerializeField] private bool copyPlayerY;

    [SerializeField] private bool explodeOnDeath;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDamage;
    [SerializeField] private GameObject explosionGraphic;
    private Transform player;

    [SerializeField] private bool bossEnemy;
    [SerializeField] private GameObject goalPoint;
    private Vector3 startPosition;
    //if true backforth will move up, otherwise move down
    [SerializeField] private bool backForthUp;


    [SerializeField] private int backForthMoveDistance;
    private Vector2 UpPosition;
    private Vector2 DownPosition;
    private bool goalPositionReached;


    protected void Awake()
    {
        player = PM.player.transform;
        startPosition = transform.position;
        goalPoint = ESM.bossGoalPosition;
    }

    protected void Start()
    {     
        goalPositionReached = false;
        switch (moveDirection)
        {
            case movementDirection.Up:
                direction = Vector2.up;
                break;
            case movementDirection.Down:
                direction = Vector2.down;
                break;
            case movementDirection.Left:
                direction = Vector2.left;
                break;
            case movementDirection.Right:
                direction = Vector2.right;
                break;
            case movementDirection.DownLeft:
                direction = new Vector2(-1, -1);
                break;
            case movementDirection.None:
                direction = Vector2.zero;
                break;
        }

        switch (moveType)
        {
            default:
                break;

            case movementTypes.BackForth:
                UpPosition = new Vector2(goalPoint.transform.position.x, startPosition.y + backForthMoveDistance);
                DownPosition = new Vector2(goalPoint.transform.position.x, startPosition.y - backForthMoveDistance);
                break;
        }

        currentHealth = maxHealth;
    }

    protected void Update()
    {
        if (bossEnemy)
        {
            if (!goalPositionReached)
            {
                transform.position = Vector2.MoveTowards(transform.position, goalPoint.transform.position, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, goalPoint.transform.position) <= 0.1)
                {
                    goalPositionReached = true;
                }
                return;
            }
        }

        //goal position has been reached if it exits. Move on to normal movement patterns
        switch (moveType)
        {
            case movementTypes.None:
                break;

            case movementTypes.Forward:

                transform.position += (Vector3)direction * speed * Time.deltaTime;

                if (copyPlayerY)
                {
                    transform.position = new Vector2(transform.position.x, player.position.y);
                }
                break;

            case movementTypes.BackForth:

                if (backForthUp)
                {
                    transform.position = Vector2.MoveTowards(transform.position, UpPosition, speed * Time.deltaTime);

                    if (Vector2.Distance(transform.position, UpPosition) <= 0.1f)
                    {
                        backForthUp = false;
                    }
                }

                if (!backForthUp)
                {
                    transform.position = Vector2.MoveTowards(transform.position, DownPosition, speed * Time.deltaTime);

                    if (Vector2.Distance(transform.position, DownPosition) <= 0.1f)
                    {
                        backForthUp = true;
                    }
                }

                break;
        }

        if (moveType == movementTypes.None)
        {
            return;
        }
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if (explodeOnDeath)
            {
                Explode();
            }
            else
            {
                Destroy();
            }
        }
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            GameObject explosionEffect = Instantiate(explosionGraphic, transform);
            explosionEffect.GetComponent<ExplosionGraphic>().explosionRadius = explosionRadius;
            explosionEffect.transform.SetParent(null);

            if (!collider.TryGetComponent<PlayerManager>(out var player))
            {
                continue;
            }
            else
            {
                player.Damage(explosionDamage);
            }
        }
        Destroy();
    }

    public void Destroy()
    {
        if (ESM.enemiesAlive.Contains(gameObject))
        {
            OnEnemyDied(gameObject);
        }
        Destroy(gameObject);
    }
}
