using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using static F_ListNameChecker.FlistApi;

namespace F_ListNameChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            Ticket ticket = GetApiTicket("svchost", "V&9iafMY7iNn");

            List<string> pokemons = File.ReadAllLines("pokemon.txt").ToList();
            List<string> theResults = new List<string>();
            int counter = 0;

            foreach (var pokemon in pokemons)
            //Parallel.ForEach (pokemons, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 10}, (pokemon) =>
            {
                Console.WriteLine("thread start " + counter++);
                string results = "";
                var profile = GetCharacterInfo(pokemon, ticket);
                if (profile["error"].ToString().StartsWith("You may not"))
                {
                    // banned or restricted viewing
                    results = $"{pokemon}: Banned or view restricted.";
                }
                else if (profile["error"].ToString().StartsWith("Character not"))
                {
                    // bingo, available
                    results = $"{pokemon}: Available.";
                }
                else
                {
                    string updated = profile["updated_at"].ToString();
                    results = $"{pokemon}: Last updated: {DateTimeOffset.FromUnixTimeSeconds(long.Parse(updated)).DateTime.ToString("yyyy-MM-dd HH:mm:ss")}";
                }
                theResults.Add(results);
            //});
            }
            theResults.Sort();
            File.WriteAllLines("primes.txt", theResults);
        }

 

        private void DoThing(int sanityCheck)
        {
            List<FileInfo> files = new DirectoryInfo(@"C:\Users\Inuktitut\Downloads\names").GetFiles().ToList();
            HashSet<string> names = new HashSet<string>();
            foreach (var file in files)
            {
                List<string> lines = File.ReadAllLines(file.FullName).ToList();
                foreach (string line in lines)
                {
                    int num = int.Parse(line.Split(',')[2]);
                    string name = line.Split(',')[0];
                    if (num < sanityCheck || name.Length <= 2)
                    {
                        continue;
                    }
                    names.Add(name);
                }
            }
            File.WriteAllLines("names.txt",names);
        }
    }
}
