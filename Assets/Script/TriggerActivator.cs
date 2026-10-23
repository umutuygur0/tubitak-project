using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public GameObject exclamationMark;
    public GameObject gameplayRoot;
    public DialogueManager dialogueManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            exclamationMark.SetActive(false);
            gameplayRoot.SetActive(true);
            dialogueManager.StartDialogue();
        }
    }
}
