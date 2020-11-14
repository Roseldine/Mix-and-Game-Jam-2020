
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float _lifeTime = 10f;
    public bool _destroy = true;

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
