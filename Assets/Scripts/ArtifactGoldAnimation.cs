using UnityEngine;

public class ArtifactGoldAnimation : MonoBehaviour
{
    public GameObject Artifact;
    public float minScale;
    public float maxScale;
    public float rotationAnimationSpeed;
    public float sizeAnimationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(Time.time * rotationAnimationSpeed * 20, Time.time * rotationAnimationSpeed, Time.time * rotationAnimationSpeed);
        transform.localScale = Vector3.one * (minScale + ((Mathf.Sin(Time.time * sizeAnimationSpeed) +1)*0.5f) * (maxScale - minScale));
    }
}
