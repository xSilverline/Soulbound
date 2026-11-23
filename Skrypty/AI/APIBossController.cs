using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Controllers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace AI
{
    public class APIBossController : MonoBehaviour
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

        private string[] _attackContent;
        private string _chosenAttack;
        private TextContent[] _chatHistory;

        private const string GetWordsCommand = "Podaj słowa";
        private const string GetAttack = "Podaj atak";

        public event Action WordsFinishedEvent;
        public event Action AttackWordFinishedEvent;
        
         void Start()
        {
            APIConnector.ApiKeyJson jsonApiKey = JsonUtility.FromJson<APIConnector.ApiKeyJson>(jsonApi.text);
            apiKey = jsonApiKey.key;   
            _chatHistory = new TextContent[] { };
            StartCoroutine(SendMessageToAPI(GetWordsCommand));
        }

        public void AskForAttack()
        {
            StartCoroutine(SendMessageToAPI(GetAttack));
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
                    npcResponseText = "Error";
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    File.WriteAllText(Application.dataPath + "/BossResponse.json", jsonResponse);
                    APIConnector.APIResponse response = JsonUtility.FromJson<APIConnector.APIResponse>(jsonResponse);
                    
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
                        if (message == GetWordsCommand)
                        {
                            ParseWords(npcText);
                        }
                        else
                        {
                            ParseAttack(npcText);
                        }
                    }
                    else
                    {
                        npcResponseText = "Error";
                    }
                }
            }
        }

        private void ParseWords(string message)
        {
            _attackContent = message.Split(' ');
            WordsFinishedEvent?.Invoke();
        }

        public string[] GetWords()
        {
            return _attackContent;
        }

        private void ParseAttack(string message)
        {
            _chosenAttack = message;
            AttackWordFinishedEvent?.Invoke();
        }

        public string GetAttackWord()
        {
            return _chosenAttack;
        }

        [System.Serializable]
        public class ChatRequest
        {
            public TextContent[] contents;
            public TextContent systemInstruction;
        }

    }
}
