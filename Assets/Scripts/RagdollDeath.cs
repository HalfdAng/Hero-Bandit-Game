using Unity.Cinemachine;
using UnityEngine;

public class RagdollDeath : MonoBehaviour
{
    public GameObject player;
    public GameObject playerObject;
    private PlayerController _playerController;
    public GameObject _armature;
    public CinemachineCamera PlayerRagdollCamera;

    public Rigidbody[] _rigidBones;

    private bool _playerHasDied = false;
    private Vector3 _deathVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.isDead && !_playerHasDied)
        {
            _playerHasDied = true;
            PlayerRagdollCamera.enabled = true;
            _deathVelocity = player.GetComponent<Rigidbody>().linearVelocity;

            // Matches own transform and rigidbody
            gameObject.transform.localPosition = player.transform.localPosition;
            gameObject.transform.rotation = player.transform.rotation;

            _armature.SetActive(true);
            foreach (var rigidBone in _rigidBones)
            {
                rigidBone.linearVelocity = _deathVelocity;
            }
            playerObject.SetActive(false);
        }
    }
}
