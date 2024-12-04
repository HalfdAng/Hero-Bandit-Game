using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    public LayerMask detectionLayers; // Select layers to be detected
    public bool IsColliding {  get; private set; }
    public Vector3 rayDirection = Vector3.forward; // Default direction, otherwise set
    private Vector3 _finalRayDirection;
    public float rayLength = 1.0f; // Default length
    public bool rotateWithTransform = true; // If true, ray will rotate with the Transform of the object

    public delegate void CollisionStateChangedAction(bool state);
    public event CollisionStateChangedAction OnCollisionStateChanged;

    [HideInInspector] public RaycastHit outHit;

    private void Update()
    {
        _finalRayDirection = rotateWithTransform ? transform.TransformDirection(rayDirection) : rayDirection;

        RaycastHit hit;
        bool hitDetected = Physics.Raycast(transform.position, _finalRayDirection.normalized * rayLength, out hit, rayLength, detectionLayers); // Start, Direction * Length, Output, Max Distance, LayerMask
        outHit = hit;

        if (hitDetected && !IsColliding) SetCollisionState(true);
    }

    void SetCollisionState(bool state)
    {
        if (IsColliding != state)
        {
            IsColliding = state;
            OnCollisionStateChanged?.Invoke(IsColliding);
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the ray in the scene view for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _finalRayDirection.normalized * rayLength);
    }
}
