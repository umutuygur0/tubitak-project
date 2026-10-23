using UnityEngine;

public class AddCollidersToAll : MonoBehaviour
{
    [ContextMenu("Tüm Nesnelere Collider Ekle")]
    void AddColliders()
    {
        // Sahnedeki tüm MeshRenderer'lı nesneleri bul
        MeshRenderer[] allObjects = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);

        foreach (MeshRenderer obj in allObjects)
        {
            // Zaten collider varsa atla
            if (obj.GetComponent<Collider>() != null)
                continue;

            // Box Collider ekle (çoğu nesne için yeterli)
            obj.gameObject.AddComponent<BoxCollider>();

            Debug.Log($"Collider eklendi: {obj.name}");
        }
    }
}