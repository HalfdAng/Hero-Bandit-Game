using UnityEngine;
using UnityEngine.UI;

public class FirewallScript : MonoBehaviour
{
    [Header("Important")]
    public bool isActive = false;
    public GameObject player;
    public GameObject fireVignetteRawImage;

    [Header("Stats")]
    public float MinSpeed = 4;
    public float MaxSpeed = 16;
    public float fireVignetteCoefficient = 1;
    public float fireVignetteDisplacement = 0;

    private PlayerController _playerController;
    private Transform _playerTransform;
    private RawImage _rawImage;

    void Start()
    {
        _playerController = player.GetComponent<PlayerController>();
        _playerTransform = player.transform;
        _rawImage = fireVignetteRawImage.GetComponent<RawImage>();
    }

    private void FixedUpdate()
    {
        _rawImage.color = new Color(1, 1, 1, 1 - Mathf.Clamp((transform.position - _playerTransform.position).magnitude / fireVignetteCoefficient + fireVignetteDisplacement, 0, 1));
        if (isActive && !_playerController.isDead)
        {
            transform.position -= new Vector3(transform.position.x - _playerTransform.position.x, 0, transform.position.z - _playerTransform.position.z).normalized * Mathf.Clamp((transform.position - _playerTransform.position).magnitude, MinSpeed, MaxSpeed) * Time.fixedDeltaTime;
            transform.LookAt(new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z));
        }
    }
}