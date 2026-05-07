using UnityEngine;

public class LanternInteractable : MonoBehaviour
{
    [SerializeField] private string LanternID = "Lantern_01";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        LanternSystem.Instance?.Rest(LanternID, transform.position);
    }
}
