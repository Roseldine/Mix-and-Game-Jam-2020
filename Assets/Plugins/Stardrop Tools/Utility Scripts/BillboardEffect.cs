
using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
	public Transform camTransform;
	Quaternion originalRotation;

    private void OnEnable()
    {
        camTransform = Camera.main.transform;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        LookAtCamera();
    }

    public void LookAtCamera()
    {
        if (camTransform != null)
            transform.rotation = camTransform.rotation * originalRotation;
    }
}
