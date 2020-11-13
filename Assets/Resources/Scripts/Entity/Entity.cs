
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum entityType { none, player, enemy }

    [Header("Entity Type")]
    public int _entityId;
    public entityType _entityType;

    [Header("Entity Movement")]
    public float _moveSpeed = 5;
    public float _lookSpeed = 10;

    [Header("Components")]
    public Rigidbody _rigidbody;

    private void Update()
    {
        UpdateEntity();
    }


    public virtual void UpdateEntity()
    {
        
    }


    #region Utility
    public void SmoothLookAt(Transform rotator, Vector3 direction, float speed)
    {
        if (direction != Vector3.zero)
        {
            rotator.rotation = Quaternion.Slerp(rotator.rotation, Quaternion.LookRotation(direction), speed * Time.deltaTime);
        }
    }

    public void SmoothLookAt(Transform rotator, Quaternion rotation, float speed)
    {
        rotator.rotation = Quaternion.Slerp(rotator.rotation, rotation, speed * Time.deltaTime);
    }
    #endregion
}