using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarCollisionBehaviour : MonoBehaviour
{
    MeshCollider _collider;

    public delegate void CollisionDelegate();
    public CollisionDelegate onCollision;

    private void OnEnable()
    {
        onCollision += delegate { StartCoroutine(CarCollisionEffect()); };
    }
    private void OnDisable()
    {
        onCollision -= delegate { StartCoroutine(CarCollisionEffect()); };
    }
    private void Awake()
    {
        _collider = GetComponentInChildren<MeshCollider>();
        AddTriggerFunctionToMesh();
    }

    private void AddTriggerFunctionToMesh()
    {
        var triggerComponent = _collider.transform.AddComponent<TriggerBehaviour>();
        triggerComponent.SetEvent(this);
    }

    private IEnumerator CarCollisionEffect()
    {
        _collider.enabled = false;

        var timer = 0f;
        var dur = 0.5f;

        var _originPos = transform.localPosition;
        var _originRot = transform.localRotation;

        var _targetPos = _originPos + new Vector3(Random.Range(5, 20), Random.Range(5, 20), Random.Range(1, 5));
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
