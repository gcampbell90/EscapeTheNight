using UnityEngine;

[RequireComponent(typeof(PlayerMovementBehaviour))]
[RequireComponent(typeof(BoostBehaviour))]

public class PlayerCollisionBehaviour : MonoBehaviour
{
    PlayerMovementBehaviour movementBehaviour;
    float penaltySpeed;
    private void Awake()
    {
        movementBehaviour = GetComponent<PlayerMovementBehaviour>();
    }

    public void Start()
    {
        //penaltySpeed = GameController.Instance.PenaltySpeed;
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Player Collider " + collision.name);

        //GameController.onSpeedChange?.Invoke(penaltySpeed);
        movementBehaviour.ResetPos();
    }
}
