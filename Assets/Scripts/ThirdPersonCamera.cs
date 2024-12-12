using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("References")]
    public Transform Orientation;
    public GameObject Player;
    public Transform PlayerObject;
    public Rigidbody Rigidbody;

    public float RotationSpeed;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!Player.GetComponent<PlayerController>().CharacterActive) return;
        // rotate orientation
        Vector3 viewDir = Player.transform.position - new Vector3(transform.position.x, Player.transform.position.y, transform.position.z);
        Orientation.forward = viewDir.normalized;

        // rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = Orientation.forward * verticalInput + Orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            PlayerObject.forward = Vector3.Slerp(PlayerObject.forward, inputDir.normalized, Time.deltaTime * RotationSpeed);
        }
    }
}
