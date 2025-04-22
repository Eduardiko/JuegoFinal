using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIManager.Instance.AddRoll();
            AudioManager.Instance.PlaySFX(0, 1f, Random.Range(0.5f, 1.5f));
            Destroy(gameObject);
        }
    }
}
