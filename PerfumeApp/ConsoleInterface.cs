using System;
using System.Collections.Generic;
using System.Linq;


namespace PerfumeApp
{
    class ConsoleInterface
    {

        private static String Input;
        private static List<String> AcceptedInputs = new List<String>();
        private static Action Scene;
        private static Boolean ProgramRun;

        public static void Start()
        {
            ProgramRun = true;
            Scene = MainMenu;

            PerfumeFinder.Initialize("data/parfume-data.txt");

            while(ProgramRun)
            {
                Scene();
            }
        }

        private static void MainMenu()
        {
            Console.Clear();
            Input = "";
            AcceptedInputs.Clear();
            AcceptedInputs.Add("1");
            AcceptedInputs.Add("2");
            AcceptedInputs.Add("3");
            AcceptedInputs.Add("4");
            AcceptedInputs.Add("5");

            while (!AcceptedInputs.Contains(Input))
            {
                Console.WriteLine("Hello ! This program contains more than 20.000 perfume and their datas. Select the operation you want to do :\n");
                Console.WriteLine("1 : Find a perfume by name.");
                Console.WriteLine("2 : Filter and find perfumes by accords.");
                Console.WriteLine("3 : Show infomation about perfume datas.");
                Console.WriteLine("4 : Suggest me a perfume !");
                Console.WriteLine("5 : Quit program... :/");
                Console.Write("\nSelect your operation : ");

                Input = Console.ReadLine();
                Console.Clear();
            }

            if (Input == "1") { Scene = FindByNameMenu; return; }
            if (Input == "2") { Scene = FilterByAccordsMenu; return; }
            if (Input == "3") { Scene = InformationMenu; return; }
            if (Input == "4") { Scene = SuggestPerfumeMenu; return; }
            if (Input == "5") { ProgramRun = false; return; }
        }

        private static void FindByNameMenu()
        {
            Console.Clear();

            Console.WriteLine("---- Find Perfume by Name ----\n");
            Console.WriteLine("From this menu, you can enter name of your perfume and it will show it's properties.");
            Console.WriteLine("If you don't know the exact name of it, you can write down as much as you can remember.\nProgram will show the possible ones.");
            Console.Write("\nEnter the name of your perfume : ");

            Input = Console.ReadLine();
            Console.Clear();

            PerfumeFinder.FindPerfumesByName(Input);
            PerfumeFinder.ConsoleWriteFoundPerfumes();

            Console.Write("\n -------------------------------------------");
            Console.Write("\n|...Press any key to go back to main menu...|");
            Console.Write("\n -------------------------------------------\n");

            Console.ReadKey();

            Scene = MainMenu;
        }

        private static void FilterByAccordsMenu()
        {
            Boolean Include_mode;

            Console.Clear();
            Input = "";
            AcceptedInputs.Clear();
            AcceptedInputs.Add("0");
            AcceptedInputs.Add("1");

            while (!AcceptedInputs.Contains(Input))
            {
                Console.WriteLine("---- Filter and Find Perfumes by Accords ----\n");
                Console.WriteLine("From this menu, you can enter the accords that you want in your perfume.");
                Console.WriteLine("Depending on your filter mode selection, program will show you the perfumes filtered by your desired accords!");
                Console.WriteLine("\nFilter modes :");
                Console.WriteLine("0 : Show perfumes that doesn't includes specified accords.");
                Console.WriteLine("1 : Show perfumes that does includes specified accords.");

                Input = Console.ReadLine();
                Console.Clear();
            }
            
            Include_mode = Input == "1";


            List<String> RequestedAccordIndexes = new List<string> { "" };
            AcceptedInputs.Clear();
            for(int i = 0; i<PerfumeFinder.FindKnownAccordsAmount(); ++i)
            {
                AcceptedInputs.Add(i.ToString());
            }

            while (RequestedAccordIndexes.Except(AcceptedInputs).Count() != 0)
            {
                Console.WriteLine("---- Filter and Find Perfumes by Accords ----\n");
                Console.WriteLine("From this menu, you can enter the accords that you want in your perfume.");
                Console.WriteLine("Depending on your filter mode selection, program will show you the perfumes filtered by your desired accords!");
                Console.WriteLine("\nKnown Accords :");
                PerfumeFinder.ConsoleWriteKnownAccords();
                Console.WriteLine("\nExample input for loved, vanilla, woody, smoky accords : 5,24,15,33");
                Console.Write("Enter the indexes of desired accords : ");

                Input = Console.ReadLine();
                RequestedAccordIndexes = new List<String>(Input.Split(','));

                Console.Clear();

            }

            PerfumeFinder.FindPerfumesByAccords(PerfumeFinder.FindAccordsByIndexes(RequestedAccordIndexes), Include_mode);
            PerfumeFinder.ConsoleWriteFoundPerfumes();

            Console.Write("\n -------------------------------------------");
            Console.Write("\n|...Press any key to go back to main menu...|");
            Console.Write("\n -------------------------------------------\n");

            Console.ReadKey();

            Scene = MainMenu;
        }

        private static void InformationMenu()
        {
            Console.Clear();
            Console.WriteLine("---- Information About Perfume Dataset ----\n");
            Console.WriteLine("From this menu, you can see some information about dataset that program uses.");
            Console.WriteLine($"\nTotal number of perfumes : {PerfumeFinder.FindPerfumeAmountTotal()}");
            Console.WriteLine($"Total number of perfumes for men : {PerfumeFinder.FindPerfumeAmountByGender(EGender.Men)}");
            Console.WriteLine($"Total number of perfumes for women: {PerfumeFinder.FindPerfumeAmountByGender(EGender.Women)}");
            Console.WriteLine($"Total number of perfumes for women and men: {PerfumeFinder.FindPerfumeAmountByGender(EGender.Both)}");
            Console.WriteLine($"Perfume amount for each accord : ");
            PerfumeFinder.ConsoleWritePerfumeAmountForEachAccord();

            Console.Write("\n -------------------------------------------");
            Console.Write("\n|...Press any key to go back to main menu...|");
            Console.Write("\n -------------------------------------------\n");

            Console.ReadKey();

            Scene = MainMenu;

        }

        private static void SuggestPerfumeMenu()
        {

            Console.Clear();

            Dictionary<String, int> RequestedAccords = new Dictionary<String, int>();
            RequestedAccords.Add("", 0);
            AcceptedInputs.Clear();
            AcceptedInputs = AcceptedInputs.Concat(PerfumeFinder.GetKnownAccords()).ToList();

            while(RequestedAccords.Keys.ToList().Except(AcceptedInputs).Count() != 0)
            {                      
                RequestedAccords.Clear();
                Console.WriteLine("---- Suggest Me A Perfume ----\n");
                Console.WriteLine("From this menu, program can suggest to you the closest perfume for your given accords.");
                Console.WriteLine("\nExample input : whiskey:82,citrus:70");
                Console.Write("\nEnter your accord selections : ");

                Input = Console.ReadLine();
                Console.Clear();

                try
                {
                    
                    foreach (String accord_data in Input.Split(','))
                    {
                        RequestedAccords.Add(accord_data.Split(':')[0], Int32.Parse(accord_data.Split(':')[1]));
                    }
                }
                catch(Exception ex )
                {
                    if (ex is ArgumentNullException || ex is FormatException || ex is IndexOutOfRangeException)
                    {
                        RequestedAccords.Add("",0);
                        continue;
                    }
                    throw;
                }               

            }

            Perfume SuggestedPerfume = PerfumeFinder.SuggestPerfumeByRequestedAccords(RequestedAccords);

            Console.WriteLine("---- Suggest Me A Perfume ----\n");
            Console.WriteLine("The program suggest you this perfume !");
            Console.WriteLine($"\n{SuggestedPerfume.Info()}");

            Console.Write("\n -------------------------------------------");
            Console.Write("\n|...Press any key to go back to main menu...|");
            Console.Write("\n -------------------------------------------\n");

            Console.ReadKey();

            Scene = MainMenu;
        }
    }
}
