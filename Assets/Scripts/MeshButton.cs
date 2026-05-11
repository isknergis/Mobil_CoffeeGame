using UnityEngine;
using UnityEngine.Events;

public class MeshButton : MonoBehaviour
{
    [Header("Ayarlar")]
    public UnityEvent onMeshClick; // Müfettiþten (Inspector) fonksiyon atayabileceksin
    public Color hoverColor = Color.gray; // Fare üzerine gelince renk deðiþimi
    private Color originalColor;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null) originalColor = meshRenderer.material.color;
    }

    // Dokunmatik veya Fare týklamasý
    void OnMouseDown()
    {
        // Týpký normal butonun OnClick'i gibi çalýþýr
        if (onMeshClick != null)
            onMeshClick.Invoke();

        // Küçük bir basýlma efekti (Opsiyonel)
        transform.localScale *= 0.9f;
    }

    void OnMouseUp()
    {
        transform.localScale /= 0.9f; // Býrakýnca eski boyuta döner
    }
}