using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIManager.Instance.AddRoll();
            Destroy(gameObject);
        }
    }
}
