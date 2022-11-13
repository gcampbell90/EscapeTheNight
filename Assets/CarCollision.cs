using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarCollision : MonoBehaviour
{
    BoxCollider _collider;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
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
            StartCoroutine(CarCollisionEffect());
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

    private IEnumerator CarCollisionEffect()
    {
        _collider.enabled = false;

        var timer = 0f;
        var dur = 0.5f;

        var originPosition = transform.localPosition;
        var originRotation = transform.localRotation;
        var targetPos = new Vector3(0,0, Random.Range(5,10));
        var targetRot = Quaternion.Euler(20, 20, 20);

        while (timer < 1f)
        {
            transform.localPosition = Vector3.Lerp(originPosition, targetPos, timer);
            //Quaternion.Slerp(Quaternion.identity, targetRot, timer));
            timer += Time.deltaTime / dur;
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        transform.localPosition = originPosition;
        _collider.enabled = true;
    }

}
