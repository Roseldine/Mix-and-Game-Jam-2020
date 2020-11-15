
using System.Collections;
using UnityEngine;

public class Trophy : MonoBehaviour
{
    public static Trophy Instance;

    [Header("Health")]
    public Transform _trophyTransform;
    public Transform _enemyTarget;
    public int _maxHealth;
    public int _health;
    public bool _isDead;

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
            }
        }
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
}