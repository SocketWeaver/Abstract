using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float Lerp = 0.5f;
    public Vector3 Offset;

    private void FixedUpdate()
    {
        if (Target == null)
        {
            return;
        }

        Vector3 desiredPosition = Target.position + Offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Lerp);
    }
}