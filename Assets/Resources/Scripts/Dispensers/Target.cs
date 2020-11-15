

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Target : MonoBehaviour
{
    public enum triggerShape { sphere, cube }

    [Header("Trigger Position")]
    public triggerShape _shape;
    public LayerMask _detectionLayers;
    public IEntity.entitySport _sportDetect;
    public Transform _targetTrigger;
    public float _sphereRadius = .2f;
    public Vector3 _cubeDimensions;
    public Color _triggerColor = Color.cyan;

    [Header("Hit Particle")]
    public GameObject _triggerParticle;

    [Header("Audio")]
    public AudioSource _source;
    public AudioClip _clip;
    public float _volume = .75f;

    private void Start()
    {
        if (_source == null)
            _source = GetComponent<AudioSource>();
    }


    private void Update()
    {
        DetectCollisions();
    }

    void DetectCollisions()
    {
        Collider[] _collisions = null;

        if (_shape == triggerShape.sphere)
            _collisions = Physics.OverlapSphere(_targetTrigger.position, _sphereRadius, _detectionLayers);

        else if (_shape == triggerShape.cube)
            _collisions = Physics.OverlapBox(_targetTrigger.position, _cubeDimensions, _targetTrigger.rotation, _detectionLayers);

        if (_collisions != null && _collisions.Length > 0)
        {
            for (int i = 0; i < _collisions.Length; i++)
            {
                var _ball = _collisions[i].GetComponent<Ball>();

                if (_ball._ballSport == _sportDetect && _ball._hasTriggered == false)
                {
                    // ball hit add some code here
                    if (_triggerParticle != null)
                        Instantiate(_triggerParticle, _targetTrigger.position, Quaternion.identity);

                    PlayAudio();
                    _ball._hasTriggered = true;
                }
            }
        }
    }

    public void PlayAudio()
    {
        if (_source != null && _clip != null)
        {
            _source.PlayOneShot(_clip, _volume);
        }
    }


    private void OnDrawGizmos()
    {
        if (_targetTrigger != null)
        {
            Gizmos.color = _triggerColor;
            if (_shape == triggerShape.sphere)
                Gizmos.DrawSphere(_targetTrigger.position, _sphereRadius);

            else if (_shape == triggerShape.cube)
                DrawCube(_targetTrigger.position, _targetTrigger.rotation, _cubeDimensions);
        }
    }

    void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
        Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

        Gizmos.matrix *= cubeTransform;

        Gizmos.DrawCube(Vector3.zero, Vector3.one);

        Gizmos.matrix = oldGizmosMatrix;
    }
}
