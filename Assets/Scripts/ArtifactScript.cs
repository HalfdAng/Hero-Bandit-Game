using UnityEngine;

public class ArtifactScript : MonoBehaviour
{
    [Header("Important")]
    public Cutscene1Script Cutscene1Script;

    [Header("Animation")]
    public float bobSpeed = 3f;
    public float bobPower = 0.25f;
    public float cutsceneTime = 1f;
    public float scaleMultiplier = 2f;
    public float upwardsMovement = 3f;

    private Vector3 _startPosition;
    public bool _started = false;
    private float _timePassed = 0f;
    private Color _color;

    private void Start()
    {
        _startPosition = transform.position;
        _color = gameObject.GetComponent<MeshRenderer>().material.color;
    }

    private void Update()
    {
        if (_started)
        {
            _timePassed += Time.deltaTime;
            transform.position += new Vector3(0, upwardsMovement * Time.deltaTime / cutsceneTime);
            transform.localScale *= Mathf.Pow(scaleMultiplier, Time.deltaTime/cutsceneTime);

            gameObject.GetComponent<MeshRenderer>().material.color = new Color(_color.r, _color.g, _color.b, 1 - _timePassed / cutsceneTime);

            if (_timePassed > cutsceneTime)
            {
                Cutscene1Script.StartCutscene();
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position = _startPosition + new Vector3(0, Mathf.Sin(Time.time * bobSpeed) * bobPower, 0);
        }
    }


    public void PickUp()
    {
        _started = true;
    }
}
