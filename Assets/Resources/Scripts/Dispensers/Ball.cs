
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public IEntity.entitySport _ballSport;
    public float _lifeTime = 10f;
    public bool _destroy = true;
    public bool _hasTriggered;

    private void OnEnable()
    {
        StartCoroutine(Lifetime());
    }

    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(_lifeTime);
        if (_destroy)
        {
            //Debug.Log("Destroyed: " + gameObject);
            Destroy(gameObject);
        }
    }
}
