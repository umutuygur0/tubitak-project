using System.Collections;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    public GameObject[] speechBubbles;
    public float visibleTime = 5f;
    public float delayBetweenBubbles = 1f;
    public GameObject choiceUI;

    private bool hasStarted = false;

    public void StartDialogue()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(PlayDialogues());
        }
    }

    IEnumerator PlayDialogues()
    {
        foreach (var bubble in speechBubbles)
        {
            bubble.SetActive(true);
            yield return new WaitForSeconds(visibleTime);
            bubble.SetActive(false);
            yield return new WaitForSeconds(delayBetweenBubbles);
        }

        FindFirstObjectByType<ChoiceUI>().ShowChoices(); // Butonları göster
    }
}
