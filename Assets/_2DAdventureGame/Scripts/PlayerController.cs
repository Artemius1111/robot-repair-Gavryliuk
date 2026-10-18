using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3.0f;

    [Header("Health Settings")]
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float timeBetweenShots = 0.6f;

    private int currentHealth;
    private bool isInvincible;
    private float invincibleTimer;

    private Rigidbody2D rigidbody2d;
    private float horizontal;
    private float vertical;

    private Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);

    private float shotTimer;

    public int health => currentHealth;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            horizontal = 0f;
            vertical = 0f;

            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) vertical += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) vertical -= 1f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) horizontal -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) horizontal += 1f;

            shotTimer -= Time.deltaTime;

            if (keyboard.cKey.wasPressedThisFrame && shotTimer <= 0)
            {
                Launch();
                shotTimer = timeBetweenShots;
            }
        }

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
                isInvincible = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log("Поточне здоров'я: " + currentHealth + "/" + maxHealth);
    }

    private void Launch()
    {
        GameObject projectileObject = Instantiate(
            projectilePrefab,
            rigidbody2d.position + Vector2.up * 0.5f,
            Quaternion.identity
        );

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
}