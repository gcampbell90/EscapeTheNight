using UnityEngine;


namespace AmazingAssets.CurvedWorld.Example
{
    public class RunnerCar : MonoBehaviour
    {                
        public Vector3 moveDirection = new Vector3(1, 0, 0);    //Set by spawner after instantiating
        public float movingSpeed = 1;                           //Set by spawner after instantiating
        
        Rigidbody rigidBody;


        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            rigidBody.MovePosition(transform.position + moveDirection * movingSpeed * Time.deltaTime * movingSpeed);

            if (transform.position.y < -300)
            {
                Destroy(this.gameObject);
            }
        }

        void OnTriggerEnter(Collider triggerCollider)
        {
            //Cameron Edit
            //--
            //Added Rigibodies and colliders to the cars, with is trigger checked. Mesh Collider also applied to Batmobile.
            //Check below stops false positives (collisions) with the curved world floor/road.
            //
            if (triggerCollider.gameObject.name == "Batmobile")
            {
                Debug.Log("COLLISION");
            }
            else
            {
                return;
            }

            // if (collision.rigidbody)
            //{
            //Vector3 force = (Vector3.up * 2 + Random.insideUnitSphere).normalized * Random.Range(100, 150);
            //collision.rigidbody.AddForce(force, ForceMode.Impulse);
            // }
        }
    }
}
