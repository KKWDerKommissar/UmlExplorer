using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmlExplorer.Helper
{
    /// <summary>
    /// Ai Helper for all the OpenAi Related Stuff -> Chat and prompt creation 
    /// </summary>
    public class AiHelper
    {
       private OpenAIAPI _Api;
       private Conversation _Conversation;
        private string _XmlContent;
        private string _SystemMessage;
        private string _SystemMessageContent;
        private string _DefaultFirstInput;

        public AiHelper(string xmlContent)
        {
             _Api = new OpenAIAPI("YOURAPIKEY");
            _XmlContent = xmlContent;
            _SystemMessage = "Du bist ein Chatbot, der darauf spezialisiert ist, mit Benutzern über UML-Diagramme zu interagieren. Deine Aufgabe besteht darin, Benut-zern bei der Interpretation und dem Verständnis von UML-Diagrammen zu helfen. Wenn ein Benutzer ein UML-Diagramm präsentiert, solltest du in der Lage sein, die enthaltenen Elemente zu erkennen und Informati-onen darüber bereitzustellen. Du kannst beispielsweise Objekte, Klassen, Assoziationen und andere relevante Strukturen im Diagramm identifizie-ren und beschreiben. Die überlieferten UML-Diagramme können durch nicht-Standardelemente erweitert sein, sogenannte Objekte/Instanzen. Diese lassen sich folgend beschreiben: Objekte oder Instanzen sind eine Erweiterung der UML und haben den Zweck spezifische, anpassbare In-stanzen zu erzeugen. So ist es möglich einzelnen Instanzen innerhalb ei-nes Modells besondere Attribute oder Methoden zu verleihen. So kann beispielsweise eine spezifische Instanz eines Buches zusätzliche Attribute besitzen, welche es einzigartig machen. Dadurch muss nicht die gesamte Klasse angepasst werden und es ist möglich Aus-nahmen und Beispiele effizient abzubilden. Deine Antworten sollten präzise und informativ sein, um den Benutzern bei ihrem Verständnis zu helfen. Gehe auf die spezifischen Elemente im Diagramm ein und erkläre ihre Bedeutung und Beziehung zueinander. Dein Ziel ist es, den Benutzern ein besseres Ver-ständnis der dargestellten UML-Diagramme zu vermitteln";
            _SystemMessageContent = "Hier hast du nun das UML-Klassendiagramm in XML-Form,zu welchem der Benutzer dir Fragen stellen wird und du erklären sollst: ";
            _DefaultFirstInput = "Was ist der Hauptzweck des Bibliotheks-verwaltungssystems und wie hängen die verschiedenen Klassen zusammen, um diesen Zweck zu erfüllen?";
            _Conversation = CreateNewChat();
        }
        /// <summary>
        /// <para> Creates a new instace of a conversation. </para>
        /// note: system message is hardcoded and already set in this function.
        /// </summary>
        /// <returns></returns>
        public Conversation CreateNewChat()
        {

            _Conversation = _Api.Chat.CreateConversation();
            _Conversation.Model = Model.GPT4_Turbo;
            _Conversation.RequestParameters.Temperature = 0;
            //Explain GPT what to do...
            _Conversation.AppendSystemMessage(_SystemMessage + _SystemMessageContent + _XmlContent);
            return _Conversation;

        }
        public async Task<string> InvokeConversation()
        {
            _Conversation.AppendUserInput(_DefaultFirstInput);
            string response = await _Conversation.GetResponseFromChatbotAsync();
            return response;
        }
        /// <summary>
        /// <para> sends a new message to OpenAi and returns the response</para>
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateNewMessage(string userInput)
        {
                _Conversation.AppendUserInput(userInput);
                string response = await _Conversation.GetResponseFromChatbotAsync();
                return response;
        }
        /// <summary>
        /// <para> Serves as a loop to wait for a userinput, send it to OpenAi and print the answer </para>
        /// </summary>
        /// <returns></returns:
        public async Task RunAssistantLoop()
        {
            while (true) // runs forever -> since we are waiting for our user to take an input..
            {
              
                string? userInput = Console.ReadLine();
                if (userInput == null)
                {
                    Console.WriteLine("Fehler: Keine Eingabe erkannt. Bitte versuchen Sie es erneut.");
                    continue; 
                }
                try
                {
                    string response = await CreateNewMessage(userInput);
                    Console.WriteLine("Antwort des Assistenten: " + response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
              
            }
        }

    }
}
