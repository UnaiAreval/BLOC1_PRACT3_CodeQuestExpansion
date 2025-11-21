using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeQuest
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //==============GENERAL==============
            Random random = new Random();
            const string LowerLeters = "abcçdefghijklmnñopqrstuvwxyz";
            const string UpperLeters = "ABCÇDEFGHIJKLMNÑOPQRSTUVWXYZ";
            const string ThereMustExistWizard = "You can't do that until you introduce a wizard";


            //==============STARTING·MENU·AND·SCREEN==============
            const string StartMenuTitle = "===== MAIN MENU - CODEQUEST =====";
            const string WizardWellcome = """
                Welcome {0} (Lvl. {1}),
                the {2}

                """;//0 -> wizardName : 1 -> wizardLevel : 2 -> wizardTitle
            const string WizardBits = "You have {0} Bits";
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
            const string WizardAlreadyExist = "You already have a wizard, so...";
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



            //==============WIZARD·TRAIN·SCREEN==============
            const string BattlePresentation = "A wild {0} appears! Rolling dice to determine the outcome of the battle";
            const string HealthPoints = """

                {0} Health:
                   ________________________
                  /                       /
                 /           {1}          /
                /_______________________/
                
                """;
            const string RollTheDice = "Press any key to role the dice";
            const string DiceRoll = """

                You roled the dice and it shous the number {0}
                {1}

                The monster take {2} damage
                """;
            const string MonsterAttack = "The monster answer with a {0} damage attack!";
            const string CriticalAttack = "A critical hit!!!";
            const string MonsterDefeeted = "Congrats! You killed the monster! You earn {0} Lvl. points";
            const string MonsterWins = "OH NO! The monster defeeted you!!! You lose {0} Lvl. points";
            string[] monsterNames =
            {
                "Wandering Skeleton 💀",
                "Forest Goblin 👹",
                "Green Slime 🟢",
                "Ember Wolf 🐺",
                "Giant Spider 🕷️",
                "Iron Golem 🤖",
                "Lost Necromancer 🧝‍♂️",
                "Ancient Dragon 🐉"
            };
            string[] diceFaces = {
                """
                  ________
                 /       /|
                /_______/ |
                |       | |
                |   O   | /
                |       |/
                '-------'
                """,
                """
                  ________
                 /       /|
                /_______/ |
                |     O | |
                |       | /
                | O     |/
                '-------'
                """,
                """
                  ________
                 /       /|
                /_______/ |
                |     O | |
                |   O   | /
                | O     |/
                '-------'
                """,
                """
                  ________
                 /       /|
                /_______/ |
                | O   O | |
                |       | /
                | O   O |/
                '-------'
                """,
                """
                  ________
                 /       /|
                /_______/ |
                | O   O | |
                |   O   | /
                | O   O |/
                '-------'
                """,
                """
                  ________
                 /       /|
                /_______/ |
                | O   O | |
                | O   O | /
                | O   O |/
                '-------'
                """,
            };
            int[] monsterHP = //each HP has the same position in this array than the corresponding monster in the monsterNames array
            {
                3,
                5,
                10,
                11,
                18,
                15,
                20,
                50
            };
            int[] monsterStrength = //each Strength has the same position in this array than the corresponding monster in the monsterNames array
            {
                1,
                2,
                5,
                5,
                9,
                7,
                10,
                25
            };
            int[] monsterLevelInteraction = //each Level Increse has the same position in this array than the corresponding monster in the monsterNames array, it's used to increse or reduse wizards level dipending if  he defeets or not a monster
            {
                1,
                1,
                1,
                1,
                2,
                2,
                2,
                3
            };

            

            //==============LOOT·THE·MINE==============
            const string MineEnter = """
                You have {0} chances to excavate in the mine:

                /\/\/\/\/\/\/\/\/\/\|·|/\/\/\/\/\/\/\/\/\/\
                |               0                         |
                |               ^                         |
                |               |       ➖ = NoExcavated  |
                | 0 <- X ->     Y       ❌ = EmptyHole    |
                |               |       🪙 = BitsHole     |
                |               V                         |
                \_________________________________________/

                """;
            const string YcordsAsk = """
                Enter the Y cords to excavate.
                    -> 
                """; 
            const string XcordsAsk = """
                Enter the X cords to excavate.
                    -> 
                """;
            const string ExcavationMsg = "You excavated in X = {0} : Y = {1}";
            const string BitsHole = "You found {0} Bits";
            const string EmptyHole = "This cords are empty";
            const string AlreadyExcavated = "You already excavated there, you can't find nothing";
            const int MinBitsFound = 5;
            const int MaxBitsFound = 51;//used to generate the amount of bits found
            const int BitsProv = 30; // % of provavilitis that a hole have bits
            const int LengthX = 5;
            const int LengthY = 5;
            const int ShovelUses = 5; //the amount of trys you have to excavate
            bool[,] bitsMap = new bool[LengthY, LengthX]; ; // false => no bits : true => bits
            bool[,] excavatedMap = new bool[LengthY, LengthX]; ; //false => you can excavate there : true => you already excavated there



            //==============INVENTORY·AND·SHOP==============
            const string InventoryTitle = """
                _______________________________________________________
                \====================< {0} Items >====================/
                 \__
                    |
                   [0] - Leave Inventory
                    |
                """;
            const string ShopTitle = """
                   
                  (____________________________________________)
                   /                   ____                   \
                  /                   /    \                   \
                 /                   / SHOP \                   \
                /====================\  \/  /====================\
                 |                    \____/                    |
                  \                                             |
                   \                                            |
                    |
                   [0] - Leave without buying
                    |
                    |
                """;
            const string ItemWriter = """
                   [{0}] - {1}        {2}
                    |
                """;//the 2 is used for the amount of this object you have in your inventory or for the object price if it's in the shop
            const string ShowItemDescription = """

                Selecto the item to show its description [Item identifier] or leave [0]:
                
                    -> 
                """;//used to ask for the item which you want to know the description in the inventory
            const string ItemOptions = """
                What do you want from this:
                    [0] - Tell me about how it works
                    [1] - Buy it $

                 -> 
                """;//used in the shop
            const string ItemPurchased = "You puchased {0} : {1} Bits";
            const string ItemDescriptionWriter = """
                {0}:
                      << {1} >>
                """;
            string[] items = { "Iron Dagger 🗡️", "Healing Potion ⚗️", "Ancient Key 🗝️", "Crossbow 🏹", "Metal Shield 🛡️" };
            string[] itemDescription = { 
                "In combat, it gives you a 5% chance of making a critical attack",
                "In combat, it regenerates your live 10 Hp, but you consume 1 each time you use it",
                "Let you open some closed dors in dangeons. It's consumed after using it",
                "In combat, it lets you attack the first turn from distence, doing half the damage. Enemy can't attack you in this first turn if he don't have a distance weapon",
                "In combat, you have a chance of the 10% of don't recive damage from an enemy attack"
            };
            int[] itemAmountInPropiety = new int[items.GetLength(0)];
            int[] minLevelToBuy = { 30, 10, 50, 40, 20 };
            int[] itemPrice = { 100, 20, 200, 120, 75 };
            int[] maxItemAmountInPropiety = { 1, 5, 0, 1, 1};//0 mean that it don't have an amount limitation



            //==============INVENTORY·AND·SHOP==============
            const string WizardAttacksListTitle = """
                <================= WIZARD INFORMATION =================>
                Name: {0}       Level: {1}

                Attacks:
                """;
            const string AttackWriter = "   - {0}";
            string[][] wizardAttacks = {
                new string[] {  "Magic Spark 💫" }, // level <= 15
                new string[] { "Fireball 🔥", "Ice Ray 🥏", "Arcane Shield ⚕️" }, // 15 < level <= 30
                new string[] { "Meteor ☄️", "Pure Energy Explosion 💥", "Minor Charm 🎭", "Air Strike 🍃" }, // 30 < level <= 45
                new string[] { "Wave of Light ⚜️", "Storm of Wings 🐦" }, // 45 < level <= 60
                new string[] { "Cataclysm 🌋", "Portal of Chaos 🌀", "Arcane Blood Pact 🩸", "Elemental Storm ⛈️" }  // 60 < level
            };
            int[] levelToUnlockAttack = { 0, 16, 31, 45, 60 };



            string wizardName = "";
            string wizardTitle = "";
            string titleMsg = "";
            int optionChosen; //used to chose an option in the menus
            int wizardLevel = 1;
            int wizardLive;
            int bits = 0; //bits are the game coin
            int yCords;
            int xCords;
            bool continueWithGame = true;//if the user chose the option to exit it changes to false and the game ends
            bool existingWizard = false;//if the wizard does not have name you can't run any option apart from the training, where the user enter the wizard name and then it become true
            bool leaveShopOrInventory;



            for (int i = 0;  i < itemAmountInPropiety.GetLength(0); i++)
            {
                itemAmountInPropiety[i] = 0;
            }
            do
            {
                
                do
                {
                    Console.Clear();

                    if (existingWizard)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(WizardWellcome, wizardName, wizardLevel, wizardTitle);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(WizardBits, bits);
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
                            if (!existingWizard)
                            {
                                Console.Write(AskWizardName);
                                Console.ForegroundColor = ConsoleColor.Gray;
                                wizardName = Console.ReadLine();


                                string wnCorrector = "";//Used to correct the wizard name (The first leter must be Upper and the rest Lower)
                                if (LowerLeters.Contains(wizardName[0]))
                                {
                                    int leterRuner = 0;
                                    while (LowerLeters.Contains(wizardName[0]) && leterRuner < LowerLeters.Length)//LowerLeters and UpperLeters have the same length
                                    {
                                        if (wizardName[0] == LowerLeters[leterRuner]) wnCorrector += UpperLeters[leterRuner];
                                        leterRuner++;
                                    }
                                }
                                else
                                {
                                    wnCorrector += wizardName[0];
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

                                if (wizardLevel < 20)
                                {
                                    wizardTitle = wizardTitles[0];
                                    titleMsg = titleMessages[0];
                                }
                                else if (20 <= wizardLevel && wizardLevel <= 29)
                                {
                                    wizardTitle = wizardTitles[1];
                                    titleMsg = titleMessages[1];
                                }
                                else if (30 <= wizardLevel && wizardLevel <= 34)
                                {
                                    wizardTitle = wizardTitles[2];
                                    titleMsg = titleMessages[2];
                                }
                                else if (35 <= wizardLevel && wizardLevel <= 39)
                                {
                                    wizardTitle = wizardTitles[3];
                                    titleMsg = titleMessages[3];
                                }
                                else
                                {
                                    wizardTitle = wizardTitles[4];
                                    titleMsg = titleMessages[4];
                                }

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(TrainingEnd, DaysTraining, wizardName, wizardTitle, titleMsg);
                            }
                            else
                            {
                                Console.WriteLine(WizardAlreadyExist);
                            }
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(PressToContinue);
                            Console.ReadKey();
                            break;

                        case 2:
                            if (existingWizard) {
                                wizardLive = wizardLevel;

                                int mNum = random.Next(0, monsterNames.GetLength(0));
                                string mName = monsterNames[mNum];
                                int mHP = monsterHP[mNum];
                                int mS = monsterStrength[mNum];
                                int mLI = monsterLevelInteraction[mNum];
                                Console.WriteLine(BattlePresentation, mName);

                                do
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine(HealthPoints, wizardName, wizardLive);

                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine(HealthPoints, mName, mHP);
                                    Console.WriteLine(RollTheDice);
                                    Console.ReadKey(false);
                                    int diceNumber = random.Next(0, 6);
                                    Console.WriteLine(DiceRoll, diceNumber + 1, diceFaces[diceNumber], (diceNumber + 1) * 2);
                                    mHP = mHP - ((diceNumber + 1) * 2);

                                    if (mHP > 0)
                                    {
                                        diceNumber = random.Next(0, 101); // the monster have a 10% chance of a critical hit, but only if his strength is bigger than 2
                                        if (diceNumber < 10 && mS > 2)
                                        {
                                            Console.WriteLine(MonsterAttack, mS);
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine(CriticalAttack);
                                            wizardLive -= mS;
                                        }
                                        else
                                        {
                                            Console.WriteLine(MonsterAttack, mS / 2);
                                            wizardLive = wizardLive - (mS / 2);
                                        }
                                    }
                                    Thread.Sleep(1000);
                                } while (mHP > 0 && wizardLive > 0);

                                if (wizardLive != 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine(MonsterDefeeted, mLI);
                                    wizardLevel += mLI;

                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine(MonsterWins, mLI);
                                    wizardLevel -= mLI;
                                }
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(PressToContinue);
                                Console.ReadKey();

                            }
                                break;

                        case 3:
                            if (existingWizard)
                            {
                                //MAP·GENERATOR
                                for (int i = 0; i < bitsMap.GetLength(0); i++)
                                {
                                    for (int j = 0; j < bitsMap.GetLength(1); j++)
                                    {
                                        int thereAreCoins = random.Next(0, 100);
                                        if (thereAreCoins <= BitsProv) bitsMap[i, j] = true;
                                        else bitsMap[i, j] = false;
                                    }
                                }
                                for (int i = 0; i < excavatedMap.GetLength(0); i++)
                                {
                                    for (int j = 0; j < excavatedMap.GetLength(1); j++)
                                    {
                                        excavatedMap[i, j] = false;
                                    }
                                }

                                //EXCAVATIONS
                                for (int i = 0; i < ShovelUses; i++)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Clear();
                                    Console.WriteLine(MineEnter, ShovelUses - i);
                                    //MAP·PRINTER
                                    for (int j = 0; j < excavatedMap.GetLength(0); j++)
                                    {
                                        for (int k = 0; k < excavatedMap.GetLength(1); k++)
                                        {
                                            if (!excavatedMap[j, k]) Console.Write("➖");
                                            else if (bitsMap[j, k]) Console.Write("🪙");
                                            else Console.Write("❌");
                                        }
                                        Console.WriteLine();
                                    }

                                    //Excavation
                                    do
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(YcordsAsk);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                    } while (false == Int32.TryParse(Console.ReadLine(), out yCords) || yCords > excavatedMap.GetLength(0));
                                    do
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(XcordsAsk);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                    } while (false == Int32.TryParse(Console.ReadLine(), out xCords) || xCords > excavatedMap.GetLength(1));

                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    if (!excavatedMap[yCords, xCords])
                                    {
                                        excavatedMap[yCords, xCords] = true;
                                        if (bitsMap[yCords, xCords])
                                        {
                                            int b = random.Next(MinBitsFound, MaxBitsFound); //bits found
                                            bits = bits + b;
                                            Console.ForegroundColor = ConsoleColor.Cyan;
                                            Console.WriteLine(BitsHole, b);
                                        }
                                        else Console.WriteLine(EmptyHole);
                                    }
                                    else Console.WriteLine(AlreadyExcavated);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine(PressToContinue);
                                    Console.ReadKey();
                                }
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine(WizardBits, bits);

                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(PressToContinue);
                                Console.ReadKey();
                            }
                            break;

                        case 4:
                            if (existingWizard)
                            {
                                do
                                {
                                    leaveShopOrInventory = false;
                                    do
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine(InventoryTitle, wizardName);
                                        for (int i = 0; i < itemAmountInPropiety.GetLength(0); i++)
                                        {
                                            if (itemAmountInPropiety[i] > 0)
                                            {
                                                Console.WriteLine(ItemWriter, i + 1, items[i], itemAmountInPropiety[i]);
                                            }
                                        }
                                        Console.Write(ShowItemDescription);
                                    } while (false == Int32.TryParse(Console.ReadLine(), out optionChosen));
                                    if (optionChosen > 0 && optionChosen - 1 < itemAmountInPropiety.GetLength(0) && itemAmountInPropiety[optionChosen - 1] > 0)
                                    {
                                        Console.WriteLine(ItemDescriptionWriter, items[optionChosen - 1], itemDescription[optionChosen - 1]);
                                    }
                                    else if (optionChosen == 0)
                                    {
                                        leaveShopOrInventory = true;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine(OptionNoRecognised);
                                    }
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine(PressToContinue);
                                    Console.ReadKey();
                                } while (!leaveShopOrInventory);
                            }
                            break;

                        case 5:
                            if (existingWizard)
                            {
                                do
                                {
                                    leaveShopOrInventory = false;
                                    do
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine(WizardBits, bits);
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine(ShopTitle);
                                        for (int i = 0; i < items.GetLength(0); i++)
                                        {
                                            if ((itemAmountInPropiety[i] < maxItemAmountInPropiety[i] || maxItemAmountInPropiety[i] == 0) && wizardLevel >= minLevelToBuy[i])
                                            {
                                                Console.WriteLine(ItemWriter, i + 1, items[i], itemPrice[i] + " Bits.");
                                            }
                                        }
                                        Console.Write(ChoseOption);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                    } while (false == Int32.TryParse(Console.ReadLine(), out optionChosen));
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    if (optionChosen > 0 && optionChosen - 1 < itemAmountInPropiety.GetLength(0))
                                    {
                                        int subMenuOptionSelector;
                                        Console.Write(ItemOptions);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        if (Int32.TryParse(Console.ReadLine(), out subMenuOptionSelector) && subMenuOptionSelector == 1 && itemAmountInPropiety[optionChosen - 1] < maxItemAmountInPropiety[optionChosen - 1])
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            itemAmountInPropiety[optionChosen - 1]++;
                                            bits -= itemPrice[optionChosen - 1];
                                            Console.WriteLine(ItemPurchased, items[optionChosen - 1], itemPrice[optionChosen - 1]);
                                        }
                                        else if (subMenuOptionSelector == 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine(ItemDescriptionWriter, items[optionChosen - 1], itemDescription[optionChosen - 1]);
                                        }
                                        else
                                        {

                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine(OptionNoRecognised);
                                        }
                                    }
                                    else if (optionChosen == 0)
                                    {
                                        leaveShopOrInventory = true;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine(OptionNoRecognised);
                                    }
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine(PressToContinue);
                                    Console.ReadKey();
                                } while (!leaveShopOrInventory);
                            }
                            break;

                        case 6:
                            if (existingWizard)
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.Clear();
                                Console.WriteLine(WizardAttacksListTitle, wizardName, wizardLevel);
                                for (int i = 0; i < wizardAttacks.GetLength(0); i++)
                                {
                                    if (levelToUnlockAttack[i] <= wizardLevel)
                                    {
                                        foreach (string attack in wizardAttacks[i])
                                        {
                                            Console.WriteLine(AttackWriter, attack);
                                        }
                                    }
                                }
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.WriteLine(PressToContinue);
                                Console.ReadKey();
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Clear();
                            }
                            break;

                        case 7:
                            break;

                        case 8:
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine(OptionNoRecognised);
                            Console.WriteLine(PressToContinue);
                            Console.ReadKey();
                            break;
                    }
                    if (!existingWizard && optionChosen != 0)
                    {
                        Console.WriteLine(ThereMustExistWizard);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(PressToContinue);
                        Console.ReadKey();
                    }
                } 
                catch
                {
                    Console.WriteLine($"Error in option {optionChosen}");
                }

            } while (continueWithGame);
        }
    }
}
