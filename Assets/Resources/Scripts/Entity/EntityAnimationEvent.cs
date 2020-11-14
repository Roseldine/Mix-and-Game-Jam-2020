
using System.Collections;
using UnityEngine;

public class EntityAnimationEvent : MonoBehaviour
{
    public Entity _entity;

    public void Shoot(int id)
    {
        Debug.Log(id);
        _entity._ballLauncher.Shoot();
    }

    public void StopShooting()
    {
        _entity._isShooting = false;
    }
}