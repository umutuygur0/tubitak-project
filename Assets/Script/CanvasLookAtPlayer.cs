using UnityEngine;


public class CanvasLookAtPlayer : MonoBehaviour
{
    public Transform playerCamera; // MobilePlayer içindeki Camera (MainCamera)

    void Update()
    {
        if (playerCamera == null)
        {
            // Eğer kamera atanmadıysa otomatik bul
            Camera main = Camera.main;
            if (main == null) return;
            playerCamera = main.transform;
        }

        // Canvas’ı her frame’de kameraya döndür
        transform.LookAt(playerCamera);

        // Canvas’ın yönünü ters çevirmek gerekebilir (yazılar ters görünüyorsa)
        transform.Rotate(0, 180, 0);
    }
}
