using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class level_end : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip endingClip;
    public GameObject timer;
    public GameObject player;
    public FirewallScript FireWall;

    public CinemachineCamera _camera;

    public Animator fade;
    public Animator coin;
    public Animator gem;
    public Animator crystal;
    public Animator time;

    public Animator endButtons;

    public bool stopMainSong;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            stopMainSong = true;
            audioSource.PlayOneShot(endingClip);

            player.GetComponent<PlayerController>().CharacterActive = false;
            player.GetComponent<pause>().NoPause = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            FireWall.isActive = false;

            fade.GetComponent<Animator>().SetBool("fade", true);
            coin.GetComponent<Animator>().SetBool("fade", true);
            gem.GetComponent<Animator>().SetBool("fade", true);
            crystal.GetComponent<Animator>().SetBool("fade", true);
            time.GetComponent<Animator>().SetBool("fade", true);
            timer.GetComponent<timer>().timerActice = false;
            endButtons.GetComponent<Animator>().SetBool("fade", true);

            _camera.GetComponent<CinemachineCamera>().enabled = false;
        }
    }
}
