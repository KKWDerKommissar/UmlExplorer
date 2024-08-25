using OpenAI_API;
using OpenAI_API.Chat;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UmlExplorer.Helper;

namespace UmlExplorer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Version version =  Assembly.GetEntryAssembly().GetName().Version;
            string appVersion = version.ToString();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(@"
           *    (             ) (   (       )  (       (     
         (  `   )\ )       ( /( )\ ))\ ) ( /(  )\ )    )\ )  
      (  )\))( (()/(   (   )\()|()/(()/( )\())(()/((  (()/(  
      )\((_)()\ /(_))  )\ ((_)\ /(_))(_)|(_)\  /(_))\  /(_)) 
   _ ((_|_()((_|_))   ((_)__((_|_))(_))   ((_)(_))((_)(_))   
  | | | |  \/  | |    | __\ \/ / _ \ |   / _ \| _ \ __| _ \  
  | |_| | |\/| | |__  | _| >  <|  _/ |__| (_) |   / _||   /  
   \___/|_|  |_|____| |___/_/\_\_| |____|\___/|_|_\___|_|_\  ");
            Console.WriteLine("   <------------------- Version " + appVersion+" ------------------->");
            Console.ResetColor();
            while (true) // infinite loop for repeated inputs
            {
                Console.WriteLine("Bitte geben Sie den Pfad zur XML-Datei ein (oder 'CHANGE' zum Wechseln):");
                string filePath = await GetFilePath();

                if (filePath.Equals("CHANGE", StringComparison.OrdinalIgnoreCase))
                {
                    continue; 
                }

                try
                {
                    // reading the content of the XML file
                    string xmlContent = File.ReadAllText(filePath);

                    Console.WriteLine("Diagramm gelesen. Erstelle Assistenten...");

                    // creating the assistant and starting the infinite loop
                    await StartAssistantLoop(xmlContent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fehler aufgetreten:");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static async Task<string> GetFilePath()
        {
            string? input = Console.ReadLine();

            // check if input is "CHANGE"
            if (input.Equals("CHANGE", StringComparison.OrdinalIgnoreCase))
            {
                return "CHANGE";
            }

            
            if (input == null)
            {
                Console.WriteLine("Fehler: Keine Eingabe erkannt.");
                return await GetFilePath(); 
            }

            string filePath = input.Trim();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Die angegebene Datei existiert nicht.");
                return await GetFilePath(); 
            }

            return filePath;
        }

        static async Task StartAssistantLoop(string xmlContent)
        {
            try
            {
                // create assistent
                AiHelper helper = new AiHelper(xmlContent);
                Console.WriteLine("Assistent erstellt.");
                string firstResponse =  await helper.InvokeConversation();
                Console.WriteLine("Assistent: " + firstResponse);
                await helper.RunAssistantLoop();
             
             
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Ausführen des Assistenten:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
