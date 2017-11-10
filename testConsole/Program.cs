using System;
using System.Collections.Generic;
using Newtonsoft.Json;


class class1
{
    static void Main(string[] args)
    {
        Dictionary<string, string> p = new Dictionary<string, string>();
        p.Add("Jeff","34");
        p.Add("A","34");
        p.Add("B","56");

        string json = JsonConvert.SerializeObject(p);

        Console.WriteLine(json);
        Dictionary<string, string> d = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        Console.WriteLine(d["A"]);
        Console.ReadLine();
    }
}