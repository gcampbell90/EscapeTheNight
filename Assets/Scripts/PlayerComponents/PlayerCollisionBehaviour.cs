using UnityEngine;

public class PlayerCollisionBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Player Collider " + collision.name);
        PlayerMovementBehaviour.onPlayerHit?.Invoke();
    }
}
