
using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Trophy : MonoBehaviour
{
    public static Trophy Instance;

    [Header("Health")]
    public Transform _trophyTransform;
    public Transform _enemyTarget;
    public HealthBar _healthbar;
    public int _maxHealth;
    public int _health;
    public bool _isDead;

    [Header("Collisions")]
    public LayerMask _detectionMask;
    public Transform _triggerPos;
    public float _triggerRadius;

    [Header("Particles")]
    public GameObject _damageParticle;
    public GameObject _destructionParticle;
    public Transform _particleSpawn;

    [Header("Audio")]
    public AudioSource _source;
    public AudioClip _damageClip;
    public AudioClip _destructionClip;
    public float _volume = .75f;

    private void Awake()
    {
        Instance = this;
        _trophyTransform = transform;
        _healthbar._maxHealth = _maxHealth;
        _healthbar._currentHealth = _maxHealth;
        _health = _maxHealth;
    }

    private void Update()
    {
        CheckCollisions();
    }

    public void CheckCollisions()
    {
        var _collisions = Physics.OverlapSphere(_triggerPos.position, _triggerRadius, _detectionMask);

        if (_collisions != null && _collisions.Length > 0)
        {
            for (int i = 0; i < _collisions.Length; i++)
            {
                var _ball = _collisions[i].GetComponent<Ball>();
                if (_ball._hasTriggered == false)
                {
                    TakeDamage(1);
                    _ball._hasTriggered = true;
                }
            }
        }
    }


    public void TakeDamage(int ammount)
    {
        if (_health > 0)
        {
            _health -= ammount;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            PlayAudio(0);
            if (_damageParticle != null)
                Instantiate(_damageParticle, _particleSpawn.position, Quaternion.identity);

            if (_health <= 0)
            {
                if (_destructionParticle != null)
                    Instantiate(_destructionParticle, _particleSpawn.position, Quaternion.identity);

                // some end method
                _isDead = true;
                StartCoroutine(End());


            }

            _healthbar._currentHealth = _health;
        }
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    /// <summary>
    /// 0-damage, 1-destruction
    /// </summary>
    /// <param name="id"></param>
    void PlayAudio(int id)
    {
        if (_source != null && _damageClip != null && _destructionClip != null)
        {
            if (id == 0)
                _source.PlayOneShot(_damageClip, _volume);
            if (id == 1)
                _source.PlayOneShot(_destructionClip, _volume);
        }
    }

    private void OnDrawGizmos()
    {
        if (_triggerPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_triggerPos.position, _triggerRadius);
        }
    }
}