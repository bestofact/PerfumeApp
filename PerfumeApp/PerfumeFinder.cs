using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace PerfumeApp
{
    class PerfumeFinder
    {
        private static String PerfumeTxtPath = "data\\parfume-data.txt";
        private static List<Perfume> AllPerfumes = new List<Perfume>();
        private static List<String> KnownAccords = new List<string>();
        private static List<Perfume> FoundPerfumes = new List<Perfume>();
 
        //Public Functions--------------------------------------------
        public static void Initialize(String perfume_data_txt_path)
        {
            PerfumeTxtPath = perfume_data_txt_path;

            SetAllPerfumesFromParsedData(ParsePerfumeData());
            //FindPerfumeByName("Oliver");
            //KnownAccords.ForEach(Console.WriteLine);
            //List<String> filtered_accords = new List<String> { "loved" };
            //Dictionary<String, int> requested_accords = new Dictionary<string, int>();
            //requested_accords.Add("whiskey", 22);
            //requested_accords.Add("fruity", 17);
            //Console.Write(SuggestPerfumeByRequestedAccords(new Dictionary<string, int>(requested_accords)).Info());
            //FindPerfumesByAccords(filtered_accords,true);
            //ConsoleWriteFoundPerfumes();

        }


// Find functions.
        public static void FindPerfumesByName(String PerfumeName)
        {
            FoundPerfumes.Clear();
            foreach (Perfume perfume in AllPerfumes)
            {
                if (perfume.Name.Contains(PerfumeName))
                {
                    FoundPerfumes.Add(perfume);
                }
            }
        }
        public static List<String> FindAccordsByIndexes(List<String> accord_indexes)
        {
            List<String> accords = new List<string>();
            foreach(String index in accord_indexes)
            {
                accords.Add(KnownAccords[Int32.Parse(index)]);
            }
            return accords;
        }
        public static void FindPerfumesByAccords(List<String> accord_list, Boolean include_mode)
        {
            FoundPerfumes.Clear();
            foreach (Perfume perfume in AllPerfumes)
            {
                if (include_mode)
                {
                    if(accord_list.Except(perfume.Accords.Keys).Count() == 0)
                    {
                        FoundPerfumes.Add(perfume);
                    }
                }
                else
                {
                    if (accord_list.Except(perfume.Accords.Keys).Count() == accord_list.Count())
                    {
                        FoundPerfumes.Add(perfume);
                    }

                }
            }
        }
        public static int FindPerfumeAmountByGender(EGender gender)
        {
            int total_amount = 0;
            foreach(Perfume perfume in AllPerfumes)
            {
                if (perfume.Gender == gender) { ++total_amount; }
            }
            return total_amount;
        }
        public static int FindPerfumeAmountByAccord(String accord)
        {
            int total_amount = 0;
            foreach(Perfume perfume in AllPerfumes)
            {
                if (perfume.Accords.Keys.Contains(accord)) { ++total_amount; }
            }
            return total_amount;
        }
        public static int FindPerfumeAmountTotal()
        {
            return AllPerfumes.Count();
        }
        public static int FindKnownAccordsAmount()
        {
            return KnownAccords.Count();
        }

        //Our Lovely Suggest Function
        public static Perfume SuggestPerfumeByRequestedAccords(Dictionary<String,int> requested_accords)
        {
            List<String> requested_accord_names = requested_accords.Keys.ToList();

            Perfume ClosestPerfume = null;
            double previous_point_distance = double.MaxValue;
            double total_point_distance_squared = 0;
            int perfume_accord_value = 0;
            int requested_accord_value = 0;
            double current_point_distance;

            foreach(Perfume perfume in AllPerfumes)
            {
                total_point_distance_squared = 0;
                foreach(String accord_name in requested_accord_names)
                {
                    perfume_accord_value = perfume.Accords.Keys.Contains(accord_name) ? perfume.Accords[accord_name] : 0;
                    requested_accord_value = requested_accords[accord_name];

                    total_point_distance_squared += Math.Pow((perfume_accord_value - requested_accord_value),2);
                }
                current_point_distance = Math.Sqrt((double)total_point_distance_squared);

                //
                if (current_point_distance < previous_point_distance)
                {
                    ClosestPerfume = perfume;
                    previous_point_distance = current_point_distance;
                }
            }
            return ClosestPerfume;
        }


        public static void ConsoleWriteFoundPerfumes()
        {
            if (FoundPerfumes.Count() == 0)
            {
                Console.WriteLine("Unfortunately there is no perfume found by the given criterias....");
                return;
            }
            foreach (Perfume perfume in FoundPerfumes)
            {
                Console.Write(perfume.Info());
            }
        }
        public static void ConsoleWriteAllPerfumes()
        {
            foreach (Perfume perfume in AllPerfumes)
            {
                Console.WriteLine(perfume.Info());
            }
        }
        public static void ConsoleWriteKnownAccords()
        {
            foreach(String accord in KnownAccords)
            {
                Console.WriteLine($"{KnownAccords.IndexOf(accord)} : {accord}");
            }
        }
        public static void ConsoleWritePerfumeAmountForEachAccord()
        {
            foreach(String accord in KnownAccords)
            {
                Console.WriteLine($"Total number of {accord} perfumes : {FindPerfumeAmountByAccord(accord)}");
            }
        }


        public static List<String> GetKnownAccords() { return KnownAccords;  }





        //Private section--------------------------------------------------------
        private static void SetAllPerfumesFromParsedData(List<String> parsed_data)
        {

            foreach (String data in parsed_data)
            {
                Perfume perfume = new Perfume(data);
                AddPerfumeToAllPerfumes(perfume);
                AddPerfumeAccordsToKnownAccords(perfume);
            }
        }

        private static List<String> ParsePerfumeData()
        {
            List<String> seperated_perfume_infos = new List<String>();
            using (var reader = new StreamReader(GetPerfumeTxtPath()))
            {
                while (!reader.EndOfStream)
                {
                    seperated_perfume_infos = seperated_perfume_infos.Concat(new List<String>(reader.ReadLine().Split('~'))).ToList();
                }

            }
            seperated_perfume_infos.RemoveAt(seperated_perfume_infos.Count - 1);
            return seperated_perfume_infos;
        }

        private static void AddPerfumeToAllPerfumes(Perfume perfume)
        {
            if (perfume.Accords.Keys.Count() != 0) { AllPerfumes.Add(perfume); }
        }

        private static void AddPerfumeAccordsToKnownAccords(Perfume perfume)
        {
            foreach (String accord_name in perfume.Accords.Keys)
            {
                if (!KnownAccords.Contains(accord_name)) { KnownAccords.Add(accord_name); }
            }

        }

        private static String GetPerfumeTxtPath()
        {
            return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"..\\..\\..\\{PerfumeTxtPath}"));
        }

        
    }
}
