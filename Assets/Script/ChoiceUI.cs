using UnityEngine;

public class ChoiceUI : MonoBehaviour
{
    public GameObject choicePanel;
    public GameObject policeGoodPrefab;
    public GameObject policeBadPrefab;
    public Transform policeSpawnPoint;

    public void ShowChoices()
    {
        choicePanel.SetActive(true);
    }

    public void OnStopBullying()
    {
        choicePanel.SetActive(false);
        SpawnPolice(true);
    }

    public void OnSupportBullying()
    {
        choicePanel.SetActive(false);
        SpawnPolice(false);
    }

    void SpawnPolice(bool isGoodChoice)
    {
        GameObject police = Instantiate(
            isGoodChoice ? policeGoodPrefab : policeBadPrefab,
            policeSpawnPoint.position,
            Quaternion.identity
        );

        // Polisi görünür yap
        police.SetActive(true);
    }
}