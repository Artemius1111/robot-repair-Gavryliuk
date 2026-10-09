using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public int healthAmount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.ChangeHealth(1);
            Destroy(gameObject);
        }
    }
}