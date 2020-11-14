
using System.Collections;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
	[Header("Prefabs")]
	public Entity _entity;
	public GameObject[] _ballPrefabs;
	public Transform[] _spawnPoints;
	public Transform _spawnPos, _target;

	[Header("Basket Definitions")]
	[Range(0, 2)] public float _cooldown = .2f;
	[Range(0, 2)] public float _heightMultiplier = .75f;
	[Range(0, 30)] public float _maxBasketHeight = 10f;
	public float _gravity = -25f;
	public float _basketHeight = 10f;

	[Header("Football Definitions")]
	public float _footballLineLength = 5f;
	public float _footballKickForce = 20f;

	[Header("Baseball Definitions")]
	public float _baseballLineLength = 5f;
	public float _baseballBatForce = 30f;

	[Header("Booleans")]
	public bool _hasBall = true;

	[Header("Path")]
	public LineRenderer _line;
	public int _resolution = 30;

	#region Initialization
	void Start()
	{
		GetSpawnPos();
		ResetBools();
	}

	void ResetBools()
    {
		_hasBall = true;
	}
	#endregion // initialization


	void Update()
	{
		DrawPath(_spawnPos.GetComponent<Rigidbody>());
	}


	#region Shoot
	public void Shoot()
	{
		_target = InputManager.Instance._hitPreviewTransform;

		if (_entity._entitySport == IEntity.entitySport.basketball)
		{
			var _ball = Instantiate(_ballPrefabs[0], _spawnPos.position, Quaternion.identity).GetComponent<Rigidbody>();
			Launch(_ball, _basketHeight);
		}

		else if (_entity._entitySport == IEntity.entitySport.football)
		{
			var _ball = Instantiate(_ballPrefabs[1], _spawnPos.position, Quaternion.identity).GetComponent<Rigidbody>();
			var _dir = (_line.GetPosition(1) - _line.GetPosition(0)).normalized * _footballKickForce;
			_ball.AddForce(_dir, ForceMode.Impulse);
		}

		else if (_entity._entitySport == IEntity.entitySport.baseball)
		{
			var _ball = Instantiate(_ballPrefabs[2], _spawnPos.position, Quaternion.identity).GetComponent<Rigidbody>();
			var _dir = (_line.GetPosition(1) - _line.GetPosition(0)).normalized * _baseballBatForce;
			_ball.AddForce(_dir, ForceMode.Impulse);
		}

		if (_entity._isShooting == false)
        {
			
		}
	}
	#endregion // shoot


	#region Basketball Launch
	public void Launch(Rigidbody rb, float height)
	{
		rb.useGravity = false;
		Physics.gravity = Vector3.up * _gravity;
		rb.useGravity = true;
		rb.velocity = CalculateLaunchData(rb, height).initialVelocity;
	}


	LaunchData CalculateLaunchData(Rigidbody rb, float height)
	{
		float displacementY = _target.position.y - rb.position.y;
		Vector3 displacementXZ = new Vector3(_target.position.x - rb.position.x, 0, _target.position.z - rb.position.z);
		float time = Mathf.Sqrt(-2 * height / _gravity) + Mathf.Sqrt(2 * (displacementY - height) / _gravity);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * _gravity * height);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(_gravity), time);
	}


	struct LaunchData
	{
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public LaunchData (Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}
	}
	#endregion // basket launch


	#region Football
	#endregion // football


	#region Utility
	void DrawPath(Rigidbody rb)
	{
		if (_entity._entitySport == IEntity.entitySport.basketball)
		{
			var _dist = Vector3.Distance(_target.position, _entity.transform.position);
			_basketHeight = _dist * _heightMultiplier;
			_basketHeight = Mathf.Clamp(_basketHeight, 0, _maxBasketHeight);
			LaunchData launchData = CalculateLaunchData(rb, _basketHeight);
			Vector3 previousDrawPoint = rb.position;

			for (int i = 0; i < _resolution; i++)
			{
				float simulationTime = i / (float)_resolution * launchData.timeToTarget;
				Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * _gravity * simulationTime * simulationTime / 2f;
				Vector3 drawPoint = rb.position + displacement;
				Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
				previousDrawPoint = drawPoint;

				if (i < _resolution - 1)
					_line.SetPosition(i, previousDrawPoint);
			}
		}

		else if (_entity._entitySport == IEntity.entitySport.football)
		{
			_line.SetPosition(0, _spawnPos.position);
			var _targetPos = _spawnPos.position + _entity._graphicContainer.forward * _footballLineLength;
			_line.SetPosition(1, _targetPos);
		}

		if (_entity._entitySport == IEntity.entitySport.baseball)
		{
			_line.SetPosition(0, _spawnPos.position);
			var _targetPos = _spawnPos.position + (InputManager.Instance._hitPreviewTransform.position - _spawnPos.position).normalized * _baseballLineLength;
			_line.SetPosition(1, _targetPos);
		}
	}


	void GetSpawnPos()
	{
		_target = InputManager.Instance._hitPreviewTransform;

		if (_entity._entitySport == IEntity.entitySport.basketball)
		{
			_spawnPos = _spawnPoints[0];
			_line.useWorldSpace = true;
			_line.positionCount = _resolution - 1;
		}

		else if (_entity._entitySport == IEntity.entitySport.football)
		{
			_spawnPos = _spawnPoints[1];
			_line.useWorldSpace = true;
			_line.positionCount = 2;
		}

		else if (_entity._entitySport == IEntity.entitySport.baseball)
		{
			_spawnPos = _spawnPoints[2];
			_line.useWorldSpace = true;
			_line.positionCount = 2;
		}
	}
	#endregion // utility
}
