using UnityEngine;

public class FireSpeedupScript : MonoBehaviour
{
    public FirewallScript Firewall;
    public float FastFireMinSpeed = 6f;
    public float FastFireMaxSpeed = 20f;

    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasTriggered)
        {
            _hasTriggered = true;

            Firewall.MinSpeed = FastFireMinSpeed;
            Firewall.MaxSpeed = FastFireMaxSpeed;
            Debug.Log("Fire sped up!");
        }
        
    }
}
