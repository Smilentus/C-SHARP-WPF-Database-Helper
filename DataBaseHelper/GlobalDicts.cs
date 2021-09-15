using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHelper
{
    public static class GlobalDicts
    {
        private static Random random = new Random();

        public static List<CustomDict> dicts = new List<CustomDict>();
         
        public static string dictsSavePath = Environment.CurrentDirectory + @"\Dictionaries\";

        public static string[] getDictsNames()
        {
            string[] data = new string[dicts.Count];
            for (int i = 0; i < dicts.Count; i++)
                data[i] = dicts[i].dictName;
            return data;
        }

        public static string getRandomLineFromDict(int dictNum)
        {
            return dicts[dictNum].dictData[random.Next(0, dicts[dictNum].dictData.Length)];
        }

        public static void SaveDictionaries()
        {
            foreach (var dict in dicts)
            {
                string saveData = "";
                foreach (var line in dict.dictData)
                    saveData += line + ";";
                saveData = saveData.Substring(0, saveData.Length - 1);
                File.WriteAllText(dictsSavePath + dict.dictName + ".txt", saveData, Encoding.UTF8);
            }
        }

        public static void LoadDictionaries()
        {
            string[] files = Directory.GetFiles(dictsSavePath, "*.txt");
            for (int i = 0; i < files.Length; i++)
            {
                string[] data = File.ReadAllText(files[i], Encoding.UTF8).Split(';');
                string name = Path.GetFileNameWithoutExtension(files[i]);
                dicts.Add(new CustomDict() { dictID = i, dictName = name, dictData = data });
            }
        }
    }
}
