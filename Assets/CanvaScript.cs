using UnityEngine;


public class ShowCanvasAfterDelay : MonoBehaviour
{
    public GameObject targetCanvas; // Inspector’dan atayacağın Canvas
    public float delay = 45f; // Kaç saniye sonra gözüksün

    void Start()
    {
        if (targetCanvas == null)
        {
            Debug.LogWarning($"{name}: targetCanvas atanmadı, ShowCanvasAfterDelay devre dışı.", this);
            return;
        }

        // Başlangıçta Canvas kapalı olsun
        targetCanvas.SetActive(false);
        // Belirtilen süre sonra ShowCanvas metodunu çağır
        Invoke(nameof(ShowCanvas), delay);
    }

    void ShowCanvas()
    {
        targetCanvas.SetActive(true);
    }
}
