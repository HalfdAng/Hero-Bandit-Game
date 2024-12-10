using UnityEngine;

public class level_end : MonoBehaviour
{
    public GameObject player;
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
            player.GetComponent<PlayerController>().enabled = false;
            Debug.Log("ay");
        }
    }
}
