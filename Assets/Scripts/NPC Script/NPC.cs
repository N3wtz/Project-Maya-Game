using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCTalk talk;

    private void Start()
    {
        // Nonaktifkan talk di awal
        talk.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            talk.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            talk.enabled = false;
        }
    }
}
