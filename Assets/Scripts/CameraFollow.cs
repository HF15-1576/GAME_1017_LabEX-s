using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;     
    [SerializeField] private float xOffset = 6f;   // x axis for camera
    [SerializeField] private float yOffset = 0f;   // y axis for camera
    [SerializeField] private float smooth = 8f;   // how quickly the camera catches up to the target

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = new Vector3(
            target.position.x + xOffset,
            target.position.y + yOffset,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
    }
    public void SetTarget(Transform t) => target = t;

    public void SnapToTarget()
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.position.x + xOffset,
            target.position.y + yOffset,
            transform.position.z
        );
    }

    }