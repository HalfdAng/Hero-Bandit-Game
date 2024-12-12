using UnityEngine;

public class FireKillMesh : MonoBehaviour
{
    public GameObject player;

    private PlayerController _playerController;
    private MeshCollider _collider;

    void Start()
    {
        _playerController = player.GetComponent<PlayerController>();
        _collider = player.GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        _playerController.isDead = true;
        _playerController.CharacterActive = false;
    }
}
