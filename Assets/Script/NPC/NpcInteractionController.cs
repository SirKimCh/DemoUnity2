using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;

[System.Serializable]
public class ChatMessage
{
    public string type; 
    public string message;
}

[System.Serializable]
public class ChatPayload
{
    public string input;
    public List<ChatMessage> history;
}

[System.Serializable]
public class ChatResponse
{
    public string answer;
}

public class NpcInteractionController : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Transform playerTransform; 
    public float interactionDistance = 3.0f;
    public KeyCode interactionKey = KeyCode.E;
    public KeyCode exitKey = KeyCode.Escape;

    [Header("UI Elements")]
    public GameObject chatPanel; 
    public TextMeshProUGUI chatLogText;
    public TMP_InputField playerInputField; 

    [Header("Backend Settings")]
    public string serverUrl = "http://localhost:3000/api/chat";

    [Header("Player Movement")]
    public MonoBehaviour playerMovementScript; 

    private bool isPlayerInRange = false;
    private bool isChatting = false;
    private List<ChatMessage> chatHistory = new List<ChatMessage>();

    void Start()
    {
        if (chatPanel != null)
        {
            chatPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerTransform == null) return;
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        isPlayerInRange = (distance <= interactionDistance);
        if (isPlayerInRange && Input.GetKeyDown(interactionKey) && !isChatting)
        {
            StartChat();
        }
        else if (isChatting && Input.GetKeyDown(exitKey))
        {
            EndChat();
        }
    }

    public void StartChat()
    {
        isChatting = true;
        Debug.Log("Starting chat with NPC.");
        chatPanel.SetActive(true);
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
        chatLogText.text = "<b>NPC:</b> Xin chào! Tôi có thể giúp gì cho bạn?\n";
        chatHistory.Clear();
        playerInputField.ActivateInputField();
        playerInputField.Select();
        playerInputField.onEndEdit.AddListener(OnPlayerSendMessage);
    }

    public void EndChat()
    {
        isChatting = false;
        Debug.Log("Ending chat with NPC.");
        chatPanel.SetActive(false);
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
        playerInputField.onEndEdit.RemoveListener(OnPlayerSendMessage);
    }
    private void OnPlayerSendMessage(string message)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                AddMessageToLog("<b>You:</b> " + message);
                chatHistory.Add(new ChatMessage { type = "player", message = message });
                StartCoroutine(SendRequestToServer(message));
                playerInputField.text = "";
                playerInputField.ActivateInputField();
            }
        }
    }

    private IEnumerator SendRequestToServer(string playerInput)
    {
        AddMessageToLog("<b>NPC:</b> <i>...đang suy nghĩ...</i>");
        ChatPayload payload = new ChatPayload
        {
            input = playerInput,
            history = chatHistory
        };
        string jsonPayload = JsonUtility.ToJson(payload);
        using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            RemoveLastLineFromLog();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                ChatResponse response = JsonUtility.FromJson<ChatResponse>(jsonResponse);
                AddMessageToLog("<b>NPC:</b> " + response.answer);
                chatHistory.Add(new ChatMessage { type = "npc", message = response.answer });
            }
            else
            {
                Debug.LogError("Error from server: " + request.error);
                AddMessageToLog("<b>NPC:</b> <i>(Xin lỗi, tôi đang gặp sự cố kết nối.)</i>");
            }
        }
    }

    private void AddMessageToLog(string message)
    {
        chatLogText.text += message + "\n";
    }

    private void RemoveLastLineFromLog()
    {
        if (chatLogText.text.Contains("\n"))
        {
            int lastNewLine = chatLogText.text.LastIndexOf("\n");
            int secondToLastNewLine = chatLogText.text.LastIndexOf("\n", lastNewLine - 1);
            if(secondToLastNewLine != -1)
            {
                 chatLogText.text = chatLogText.text.Substring(0, secondToLastNewLine + 1);
            }
        }
    }
}
