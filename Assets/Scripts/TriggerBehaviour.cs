using UnityEngine;

internal class TriggerBehaviour : MonoBehaviour, ITriggerBehaviour
{
    CarCollisionBehaviour CarCollision { get; set; }
    public void SetEvent(CarCollisionBehaviour _carCollisionScript)
    {
        CarCollision = _carCollisionScript;
    }

    public void OnTriggerEnter(Collider triggerCollider)
    {
        //Cameron Edit
        //--
        //Added Rigibodies and colliders to the cars, with is trigger checked. Mesh Collider also applied to Batmobile.
        //Check below stops false positives (collisions) with the curved world floor/road.
        //
        //Gary Edit
        //--
        //Im doing stupid things and forgot how events work and this is a mad workaround so I can put the car collision script
        //on the parent but can send the trigger event to the parent script...
        //
        if (triggerCollider.gameObject.name == "Player")
        {
            Debug.Log("Car Collision with "+ triggerCollider.name);
            CarCollision.OnCollision?.Invoke();
            //StartCoroutine(CarCollisionEffect());
        }
        else
        {
            return;
        }

    }
}