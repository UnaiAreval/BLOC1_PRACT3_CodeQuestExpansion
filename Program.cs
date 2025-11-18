using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeQuest
{
    public class Program
    {
        public static void Main()
        {
            //==============GENERAL==============
            Random random = new Random();
            const string LowerLeters = "abcçdefghijklmnñopqrstuvwxyz";
            const string UpperLeters = "ABCÇDEFGHIJKLMNÑOPQRSTUVWXYZ";


            //==============STARTING·MENU·AND·SCREEN==============
            const string StartMenuTitle = "===== MAIN MENU - CODEQUEST =====";
            const string WizardWellcome = """
                Welcome {0} (Lvl. {1}),
                the {2}

                """;//0 -> wizardName : 1 -> wizardLevel : 2 -> wizardTitle
            const string MenuItem = "   [{0}] - {1}";
            const string PressToContinue = "Press any key to continue...";
            const string ChoseOption = """

                Write the number of the option that you will chose:
                
                    -> 
                """;
            const string OptionNoRecognised = "This option do not exist";
            const string StartingOption = "Starting option {0}";
            string[] menuOptions =
            {
                "Exit game",
                "Train your wizard",
                "Increase LVL",
                "Loot the mine",
                "Show inventory",
                "Buy items",
                "Show attacks by LVL",
                "Decode ancient Scroll"
            };



            //==============EXIT·SCREEN==============
            const string ExitMsg = "Come back sone";



            //==============WIZARD·TRAIN·SCREEN==============
            const string AskWizardName = """
                What is your wizard name?
                    -> 
                """;
            const string DayOfTrainingInfo = "Day: {0} Houers trained: {1} Level earned: {2}";
            const string TrainingEnd = """
                You trained {0} days, {1} the {2}.
                {3}
                """; //in the {3} print the title message
            const int DaysTraining = 5;
            const int MaxLevelXDey = 11; //Used in a random (1 <= rnd < 11) to generate the amount of level that the wizard encrease each day of training
            const int MaxHouersTraining = 25; //Used in a random (1 <= rnd < 25) to generate the amount of houers trained each day

            string[] wizardTitles = //A list of wizard titles to asign to the wizard dipending on his level
            {
                "Raoden el Elantrí", // level < 20
                "Zyn el Buguejat", // 20 <= level <= 29
                "Arka Nullpointer", // 30 <= level <= 34
                "Elarion de les Brases", // 35 <= level <= 39
                "ITB-Wizard el Gris" // level >= 40
            };
            string[] titleMessages = //A list of messages where each one is related to a title. Depending on the title that the wizard have it will display the message in the same position
            {
                "Repeteixes a 2a convocatòria.",
                "Encara confons la vareta amb una cullera.",
                "Ets un Invocador de Brises Màgiques.",
                "Uau! Pots invocar dracs sense cremar el laboratori!",
                "Has assolit el rang de Mestre dels Arcans!"
            };
            string wizardName = "";
            string wizardTitle = "";
            string titleMsg = "";
            int optionChosen; //used to chose an option in the menus
            int wizardLevel = 1;
            bool continueWithGame = true;//if the user chose the option to exit it changes to false and the game ends
            bool existingWizard = false;//if the wizard does not have name you can't run any option apart from the training, where the user enter the wizard name and then it become true

            do
            {
                
                do
                {
                    Console.Clear();

                    if (existingWizard)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(WizardWellcome, wizardName, wizardLevel, wizardTitle);
                    }
                    else
                    {
                        Console.WriteLine();
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(StartMenuTitle);
                    for (int i = 0; i < menuOptions.GetLength(0); i++)
                    {
                        Console.WriteLine(MenuItem, i, menuOptions[i]);
                    }
                    Console.Write(ChoseOption);
                    Console.ForegroundColor = ConsoleColor.Gray;
                } while (Int32.TryParse(Console.ReadLine(), out optionChosen) == false);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(StartingOption, optionChosen);
                Console.WriteLine(PressToContinue);
                Console.ReadKey();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Clear();

                try
                {
                    switch (optionChosen)
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine(ExitMsg);
                            continueWithGame = false;
                            Thread.Sleep(1000);
                            break;

                        case 1:
                            Console.Write(AskWizardName);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            wizardName = Console.ReadLine();


                            string wnCorrector = "";//Used to correct the wizard name (The first leter must be Upper and the rest Lower)
                            for (int i = 0; i < LowerLeters.Length && LowerLeters.Contains(wizardName[0]); i++)//LowerLeters and UpperLeters have the same length
                            {
                                if (wizardName[0] == LowerLeters[i]) wnCorrector += UpperLeters[i];
                            }
                            for (int i = 1; i < wizardName.Length; i++)
                            {
                                if (UpperLeters.Contains(wizardName[i]))
                                {
                                    for (int j = 0; j < UpperLeters.Length; j++)//LowerLeters and UpperLeters have the same length
                                    {
                                        if (UpperLeters[j] == wizardName[i]) wnCorrector += LowerLeters[j];
                                    }
                                }
                                else
                                {
                                    wnCorrector += wizardName[i];
                                }
                            }
                            wizardName = wnCorrector;
                            existingWizard = true;

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            for (int i = 0; i < DaysTraining; i++)
                            {
                                Thread.Sleep(1000);
                                int lvlIncrase = random.Next(1, MaxLevelXDey);
                                int hTrained = random.Next(1, MaxHouersTraining);
                                Console.WriteLine(DayOfTrainingInfo, i + 1, hTrained, lvlIncrase);
                                wizardLevel += lvlIncrase;
                            }
                            
                            if (wizardLevel < 20) {
                                wizardTitle = wizardTitles[0];
                                titleMsg = titleMessages[0];
                            } else if (20 <= wizardLevel && wizardLevel <= 29) {
                                wizardTitle = wizardTitles[1];
                                titleMsg = titleMessages[1];
                            } else if (30 <= wizardLevel && wizardLevel <= 34) {
                                wizardTitle = wizardTitles[2];
                                titleMsg = titleMessages[2];
                            } else if (35 <= wizardLevel && wizardLevel <= 39) {
                                wizardTitle = wizardTitles[3];
                                titleMsg = titleMessages[3];
                            } else {
                                wizardTitle = wizardTitles[4];
                                titleMsg = titleMessages[4];
                            }

                                Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(TrainingEnd, DaysTraining, wizardName, wizardTitle, titleMsg);

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(PressToContinue);
                            Console.ReadKey();
                            break;

                        case 2:
                            break;

                        case 3:
                            break;

                        case 4:
                            break;

                        case 5:
                            break;

                        case 6:
                            break;

                        case 7:
                            break;

                        case 8:
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(OptionNoRecognised);
                            Console.WriteLine(PressToContinue);
                            Console.ReadKey();
                            break;
                    }
                } 
                catch
                {

                }

            } while (continueWithGame);
        }
    }
}
