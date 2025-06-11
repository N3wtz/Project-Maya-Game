using UnityEngine;
using System.Collections;

public class NPCWithIcon : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Animator untuk ikon interaksi (objek terpisah)")]
    public Animator interactAnim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>(); // pastikan animator NPC ada di child
    }

    private void OnEnable()
    {
        Debug.Log("NPC diaktifkan");

        // Hentikan pergerakan
        if (rb != null)
            rb.velocity = Vector2.zero;

        // Jalankan animasi idle
        if (anim != null)
            anim.Play("Idle");

        // Jalankan animasi buka icon interaksi
        if (interactAnim != null)
            interactAnim.Play("Open");
        else
            Debug.LogWarning("interactAnim belum di-assign di Inspector!");
    }

    // Jangan pakai OnDisable karena saat ini animasi tidak bisa jalan di GameObject yang sudah mati
    public void DeactivateNPC()
    {
        Debug.Log("Memulai proses menonaktifkan NPC...");
        StartCoroutine(PlayCloseAndDeactivate());
    }

    private IEnumerator PlayCloseAndDeactivate()
    {
        // Jalankan animasi Close terlebih dahulu
        if (interactAnim != null)
            interactAnim.Play("Close");

        // Tunggu 0.3 detik agar animasi Close selesai diputar
        yield return new WaitForSeconds(0.3f);

        // Lalu matikan NPC
        gameObject.SetActive(false);
    }
}
