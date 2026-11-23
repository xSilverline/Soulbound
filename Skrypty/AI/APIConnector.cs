using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.IO;
using Controllers;
using InputSystem;

namespace AI
{
    public class APIConnector : MonoBehaviour
    {
        [Header("UI References")]
        public string npcResponseText="...";
        [Header("JSON API Configuration")]
        public TextAsset jsonApi;
        [Header("API Settings")]
        public string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent";
        public string apiKey = "";
        public string contentType = "application/json";
        public TextAsset botInstructions;
        [Header("Chat Settings")]
        public TMP_InputField chatInputField;

        public TMP_Text chatDisplayOutput;

        public Scrollbar chatScrollbar;
        [SerializeField] GameObject chatBox;
        
        [SerializeField] private UIInputReader inputReader;
        
        private TextContent[] _chatHistory;

        void Start()
        {
            ApiKeyJson jsonApiKey = JsonUtility.FromJson<ApiKeyJson>(jsonApi.text);
            apiKey = jsonApiKey.key;   
            _chatHistory = new TextContent[] { };
            inputReader.ReturnEvent += EndChat;
        }

        public void EndChat()
        {
            chatBox.SetActive(false);
            GameManager.Instance.SetMode(InputMode.Gameplay);
        }
        void OnEnable()
        {
            inputReader.ReturnEvent += EndChat;
            chatInputField.ActivateInputField();
            chatInputField.onSubmit.AddListener(AddToChatOutput);
        }

        void OnDisable()
        {
            inputReader.ReturnEvent -= EndChat;
            chatInputField.onSubmit.RemoveListener(AddToChatOutput);
        }

        void OnDestroy()
        {
            inputReader.ReturnEvent -= EndChat;
            chatInputField.onSubmit.RemoveAllListeners();
        }

        void AddToChatOutput(string txt)
        {
            chatInputField.text = string.Empty;
            if (string.IsNullOrWhiteSpace(txt))
            {
                Debug.LogWarning("Input is empty.");
                return;
            }
            txt = "<#027DF0>" + txt + "</color>";
            PrintMessage(txt);
            npcResponseText = "…"; 
            StartCoroutine(SendMessageToAPI(txt));
        }

        private void PrintMessage(string message)
        {
            var timeNow = System.DateTime.Now;
            string formattedInput = "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + message;

            if (chatDisplayOutput)
            {
                if (chatDisplayOutput.text == string.Empty)
                    chatDisplayOutput.text = formattedInput;
                else
                    chatDisplayOutput.text += "\n" + formattedInput;
            }
            chatInputField.ActivateInputField();
            chatScrollbar.value = 0;
        }

        private IEnumerator SendMessageToAPI(string message)
        {
            TextContent userContent = new TextContent
            {
                role = "user",
                parts = new TextPart[]
                {
                    new TextPart { text = message }
                }
            };

            TextContent instruction = new TextContent
            {
                parts = new TextPart[]
                {
                    new TextPart {text = botInstructions.text}
                }
            }; 
            
            List<TextContent> contentsList = new List<TextContent>(_chatHistory);
            contentsList.Add(userContent);
            _chatHistory = contentsList.ToArray(); 
            ChatRequest chatRequest = new ChatRequest { contents = _chatHistory, systemInstruction = instruction };
            string jsonData = JsonUtility.ToJson(chatRequest);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            using (UnityWebRequest request = new UnityWebRequest(apiEndpoint, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", contentType);
                request.SetRequestHeader("x-goog-api-key", apiKey); 
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    npcResponseText = "Przepraszam, nie jestem w stanie teraz rozmawiać.";
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    File.WriteAllText(Application.dataPath + "/APIResponse.json", jsonResponse);
                    APIResponse response = JsonUtility.FromJson<APIResponse>(jsonResponse);
                    
                    if (response != null && 
                        response.candidates.Length > 0 && 
                        response.candidates[0].content.parts.Length > 0)
                    {
                        string npcText = response.candidates[0].content.parts[0].text;
                        TextContent botContent = new TextContent
                        {
                            role = "model",
                            parts = new TextPart[]
                            {
                                new TextPart { text = npcText }
                            }
                        };
                        npcResponseText = npcText;
                        contentsList.Add(botContent);
                        _chatHistory = contentsList.ToArray();
                        PrintMessage(npcText);
                    }
                    else
                    {
                        npcResponseText = "Przepraszam, nie dam teraz rady porozmawiać, jestem dalej w szoku.";
                    }
                }
            }
            
        }
        
        [System.Serializable]
        private class Part
        {
            public string text;
        }

        [System.Serializable]
        public class APIResponse
        {
            public Candidates[] candidates;
        }
        
        [System.Serializable]
        public class TextContent
        {
            public string role;
            public TextPart[] parts;
        }

        [System.Serializable]
        public class Candidates
        {
            public TextContent content;
        }

        [System.Serializable]
        public class TextResponse
        {
            public TextCandidate[] candidates;
        }
        
        [System.Serializable]
        public class ChatRequest
        {
            public TextContent[] contents;
            public TextContent systemInstruction;
        }
        
        public class ApiKeyJson
        {
            public string key;
        }

    }
}
