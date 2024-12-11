using UnityEngine;

public class ArtifactScript : MonoBehaviour
{
    [Header("Important")]
    public Cutscene1Script Cutscene1Script;

    [Header("Animation")]
    public float bobSpeed = 3f;
    public float bobPower = 0.25f;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = _startPosition + new Vector3(0, Mathf.Sin(Time.time * bobSpeed) * bobPower, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Cutscene1Script.StartCutscene();
    }
}
