
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
	[Header("Prefabs")]
	public Entity _entity;
	public GameObject[] _ballPrefabs;
	public Transform[] _spawnPoints;

	[Header("Definitions")]
	public float h = 25;
	public float gravity = -18;

	[Header("Path")]
	public LineRenderer _line;
	public bool debugPath;


	void Start()
	{
		
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var _target = InputManager.Instance._hitPreviewTransform;
			var _spawnPos = _spawnPoints[(int)_entity._entitySport + 1];
			var _ball = Instantiate(_ballPrefabs[0], _spawnPos.position, Quaternion.identity).GetComponent<Rigidbody>();
			Launch(_ball, _target);
		}

		if (debugPath)
		{
			//DrawPath();
		}
	}

	public void Launch(Rigidbody rb, Transform target)
	{
		rb.useGravity = false;
		Physics.gravity = Vector3.up * gravity;
		rb.useGravity = true;
		rb.velocity = CalculateLaunchData(rb, target).initialVelocity;
	}

	LaunchData CalculateLaunchData(Rigidbody rb, Transform target)
	{
		float displacementY = target.position.y - rb.position.y;
		Vector3 displacementXZ = new Vector3 (target.position.x - rb.position.x, 0, target.position.z - rb.position.z);
		float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
	}

	void DrawPath(Rigidbody rb, Transform target)
	{
		LaunchData launchData = CalculateLaunchData(rb, target);
		Vector3 previousDrawPoint = rb.position;

		int resolution = 30;
		for (int i = 1; i <= resolution; i++) {
			float simulationTime = i / (float)resolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = rb.position + displacement;
			Debug.DrawLine (previousDrawPoint, drawPoint, Color.green);
			previousDrawPoint = drawPoint;
		}
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
}
	