using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.InputSystem; // 1. Yeni Input System namespace'ini ekledik

[Serializable]
public class DecisionData
{
    public int childId;
    public string scenarioId;
    public string questionId;
    public string selectedOption;
    public float responseTimeSeconds;
}

public class AnalyticsManager : MonoBehaviour
{
    [SerializeField] private string apiUrl = "https://localhost:7154/api/Decisions/save";

    private void Update()
    {
        // 2. Yeni Input System ile Keyboard kontrolü
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            SendDecision(1, "unity_test_scenario", "q_unity_01", "option_c", 2.4f);
        }
    }

    public void SendDecision(int childId, string scenarioId, string questionId, string option, float responseTime)
    {
        DecisionData data = new DecisionData
        {
            childId = childId,
            scenarioId = scenarioId,
            questionId = questionId,
            selectedOption = option,
            responseTimeSeconds = responseTime
        };

        StartCoroutine(PostDecisionCoroutine(data));
    }

    private IEnumerator PostDecisionCoroutine(DecisionData data)
    {
        string jsonPayload = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.certificateHandler = new BypassCertificate();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("<color=green>[AnalyticsManager] Karar PostgreSQL'e başarıyla gönderildi!</color> Yanıt: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("[AnalyticsManager] İstek hatası: " + request.error + " | " + request.downloadHandler.text);
            }
        }
    }
}

public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}