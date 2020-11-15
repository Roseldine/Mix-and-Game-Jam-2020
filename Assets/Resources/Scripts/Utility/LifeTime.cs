
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] float _lifetime = .5f;

    private void OnEnable()
    {
        Invoke("DestroyThis", _lifetime);
    }

    public void DestroyThis() => Destroy(gameObject);
}
