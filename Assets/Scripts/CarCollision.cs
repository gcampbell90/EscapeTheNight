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
        if (triggerCollider.gameObject.name == "Player")
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

        var _originPos = transform.localPosition;
        var _originRot = transform.localRotation;
        var _targetPos = _originPos + new Vector3(Random.Range(5, 20), Random.Range(5, 20), Random.Range(1,5));
        var _targetRot = Quaternion.Euler(20, 20, 20);

        while (timer < 1f)
        {
            var _rot = Quaternion.Slerp(Quaternion.identity, _targetRot, timer);
            var _pos = Vector3.Slerp(_originPos, _targetPos, timer);

            transform.SetLocalPositionAndRotation(_pos, _rot);
            timer += Time.deltaTime / dur;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        transform.SetLocalPositionAndRotation(_originPos, _originRot);

        _collider.enabled = true;
    }

}
