//using System;
//using System.IO;
//using System.Reflection;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ignorethis
//{
//    public string GetDirectory()
//    {
//        string value;
//        value = @"D:\school\leerjaar 2\3d Project\builds\saveFileSystems build";
//        return value;
//    }

//    public string MakeNewSaveFile(string path)
//    {
//        string value;
//        value = Path.Combine(path, "saveFile.txt");
//        if (!File.Exists(path))
//        {
//            var file = File.Create(path);
//            file.Close();
//        }
//        return value;
//    }

//    public string ReadFile(string path)
//    {
//        string value = "";

//        value = File.ReadAllText(path);

//        return value;
//    }

//    public void WriteFile(string path, string newText)
//    {
//        File.WriteAllText(path, newText);
//    }
//}
