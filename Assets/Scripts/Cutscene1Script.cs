using UnityEngine;
using UnityEngine.Playables;

public class Cutscene1Script : MonoBehaviour
{
    [Header("Important")]
    public GameObject MainCamera;
    public GameObject CutsceneCamera;
    public FirewallScript Firewall;
    public PlayerController PlayerController;
    public PlayableDirector playableDirector;

    [Header("Firewall")]
    public float FirewallCooldown = 1f;

    private float _timeSinceCutsceneEnd = 0f;
    private bool _cutsceneHasEnded = false;

    public void StartCutscene()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        MainCamera.GetComponent<AudioListener>().enabled = false;

        CutsceneCamera.GetComponent<Camera>().enabled = true;
        CutsceneCamera.GetComponent<AudioListener>().enabled = true;

        playableDirector.Play();

        PlayerController.CharacterActive = false;
    }

    void OnEnable()
    {
        playableDirector.stopped += OnPlayableDirectorStopped;
    }

    private void Update()
    {
        if (_cutsceneHasEnded)
        {
            _timeSinceCutsceneEnd += Time.deltaTime;
            if (_timeSinceCutsceneEnd > FirewallCooldown)
            {
                _cutsceneHasEnded = false;
                Firewall.isActive = true;
            }
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        MainCamera.GetComponent<Camera>().enabled = true;
        MainCamera.GetComponent<AudioListener>().enabled = true;

        CutsceneCamera.GetComponent<Camera>().enabled = false;
        CutsceneCamera.GetComponent<AudioListener>().enabled = false;

        PlayerController.CharacterActive = true;

        _cutsceneHasEnded = true;
    }

}
