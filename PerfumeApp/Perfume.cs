using System;
using System.Collections.Generic;


namespace PerfumeApp
{
    class Perfume
    {
        public String Name;
        public Dictionary<String, int> Accords = new Dictionary<string, int>();
        public EGender Gender;

        public Perfume(String pure_txt_data)
        {
            List<String> perfume_data = new List<String>(pure_txt_data.Split(';'));


            Name = perfume_data[0].Trim();
            Gender = GetGenderFromName();

            //Remove the title info, thus we only have accords to iterate
            perfume_data.RemoveAt(0);

            Accords = GetAccordsFromListData(perfume_data);

        }

        public String Info()
        {
            String Title = $"==============\nPerfume Info for {Name}\n--------------\n";
            String NameLine = $"Name : {Name}\n";
            String GenderLine = $"Gender : {Gender.ToString()}\n";
            String AccordsLine = "";
            foreach(KeyValuePair<String,int> accord_info in Accords)
            {
                AccordsLine = $"{AccordsLine}{accord_info.Key} : {accord_info.Value}\n";
            }

            String Info = $"{Title}{NameLine}{GenderLine}{AccordsLine}";
            return Info;
        }


        private Dictionary<String,int> GetAccordsFromListData(List<String> list_data)
        {
            Dictionary<String, int> accords = new Dictionary<string, int>();
            foreach(String accord_data in list_data )
            {
                String accord_name = accord_data.Split(':')[0];
                int accord_amount = Int32.Parse(accord_data.Split(':')[1]);
                accords.Add(accord_name, accord_amount);
            }
            return accords;
        }

        private EGender GetGenderFromName()
        {
            Boolean bMan = Name.Contains(" men");
            Boolean bWomen = Name.Contains(" women");

            if(bMan && bWomen) { return EGender.Both; }
            else if(bMan) { return EGender.Men; }
            else { return EGender.Women; }
        }


    }

    enum EGender
    {
        Women,
        Men,
        Both
    }
}
