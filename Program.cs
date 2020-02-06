using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leaf.xNet;

namespace Aparecium
{
    class Program
    {
        public static void Main()
        {
            Console.Title = "Aparecium | Gemaakt door Frank";
            Console.Clear();


        vraag: try
            {
Text(text: @"Welkom bij Aparecium, een programma gemaakt door Frank.
                Dit programma zoekt een gebruikersnaam over het hele internet.
                Als je de gevolgen van OSINT weet, type 1. Weet je dit niet? Type 2.

       Antwoord: ");
            string antwoordVraag = Console.ReadLine();

                try
                {
                    if (antwoordVraag == "1")
                    {
                        Console.Clear();
                    }
                    else if (antwoordVraag == "2")
                    {
                        System.Diagnostics.Process.Start("https://en.wikipedia.org/wiki/Open-source_intelligence");
                        Console.Clear();
                        goto vraag;
                    }
                    else
                    {
                        Console.Clear();
                        goto vraag;
                    }
                }
                
                   
                catch(Exception fout)
                {
                    using (StreamWriter writer = File.AppendText(@"Fout.txt"))
                    {
                        writer.WriteLine(string.Format("{0:dd:HH:mm:ss} | Fout:", DateTime.Now));
                        writer.WriteLine(fout);
                        writer.WriteLine();
                    }
                }
            }
            catch(Exception fout)
            {
                using (StreamWriter writer = File.AppendText(@"Fout.txt"))
                {
                    writer.WriteLine(string.Format("{0:dd:HH:mm:ss} | Fout:", DateTime.Now));
                    writer.WriteLine(fout);
                    writer.WriteLine();
                }
            }
            vraagTwee: try
            {
                Text(text: @"Kies een optie...
            1. Zoeker
            2. Config maken
            3. Source verwerker

    Antwoord: ");
                string antwoordVraag = Console.ReadLine();

                if (antwoordVraag == "1")
                {
                    Console.Clear();
                    Zoeker();
                }
                else if (antwoordVraag == "2")
                {
                    Console.Clear();
                    configMaken();
                }
                else if (antwoordVraag == "3")
                {
                    Console.Clear();
                    Requests();
                }
                else
                {
                    goto vraagTwee;
                }
            }
            catch (Exception fout)
            {
                using (StreamWriter writer = File.AppendText(@"Fout.txt"))
                {
                    writer.WriteLine(string.Format("{0:dd:HH:mm:ss} | Fout:", DateTime.Now));
                    writer.WriteLine(fout);
                    writer.WriteLine();
                }
            }




        }

        public static string site;
        public static string geldig;
        public static string ongeldig;
        public static int geldigCnt = 0;
        public static int ongeldigCnt = 0;
        public static int isFout = 0;
        public static void Zoeker()
        {
            Console.Title = "Aparecium | Gemaakt door Frank | Zoeker | ";
            Text(text: "Wat is de gebruikersnaam: ");
            string gebruikersNaam = Console.ReadLine();
            Console.Title = "Aparecium | Gemaakt door Frank | Config maker | Gebruikersnaam: " + gebruikersNaam;
            Console.Clear();

            Directory.CreateDirectory(@"Uitslagen//" + gebruikersNaam);

            foreach (var config in Directory.GetFiles(@"Configs"))
            {
                string[] lines = File.ReadAllLines(config);
                foreach(string line in lines)
                {
                    if (line.Contains("Site:"))
                    {
                        site = getBetween(line, "\"", "\"");
                    }
                    else if (line.Contains("Geldig: \""))
                    {
                        geldig = getBetween(line, "\"", "\"");
                    }
                    else if (line.Contains("Ongeldig: \""))
                    {
                        ongeldig = getBetween(line, "\"", "\"");
                    }      
                }

                using (HttpRequest request = new HttpRequest())
                {
                    request.IgnoreProtocolErrors = true;
                    request.AllowAutoRedirect = true;
                    request.AllowEmptyHeaderValues = true;
                    request.IgnoreInvalidCookie = true;
                    request.KeepAlive = true;

                    string get = request.Get(site + gebruikersNaam).ToString();
                    if (get.Contains(geldig))
                    {
                        geldigCnt++;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(site + gebruikersNaam);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" | ");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("Geldig.");
                        Console.WriteLine();

                        using (StreamWriter writer = File.AppendText(@"Uitslagen//" + gebruikersNaam + "//Geldig.txt"))
                        {
                            writer.WriteLine(site + gebruikersNaam);
                            writer.Flush();
                            writer.Close();
                        }
                    }
                    else if (get.Contains(ongeldig))
                    {
                        ongeldigCnt++;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(site + gebruikersNaam);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" | ");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("Niet geldig.");
                        Console.WriteLine();

                        using (StreamWriter writer = File.AppendText(@"Uitslagen//" + gebruikersNaam + "//NietGeldig.txt"))
                        {
                            writer.WriteLine(site + gebruikersNaam);
                            writer.Flush();
                            writer.Close();
                        }
                    }
                    else
                    {
                        isFout++;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(site + gebruikersNaam);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" | ");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("Fout.");
                        Console.WriteLine();

                        using (StreamWriter writer = File.AppendText(@"Fout.txt"))
                        {
                            writer.WriteLine(string.Format("{0:dd:HH:mm:ss} | Fout:", DateTime.Now));
                            writer.WriteLine(get);
                            writer.WriteLine();
                        }
                    }
                    Console.Title = "Aparecium | Gemaakt door Frank | Zoeker | Gebruikersnaam: " + gebruikersNaam + " | Geldig: " + geldigCnt + " | Ongeldig: " + ongeldigCnt;

                }

            }

            Console.WriteLine("");
            Text(text: "Klaar! Klik op een knop om naar het hoofdmenu te gaan.");
            Console.ReadKey();
            ongeldigCnt = 0;
            geldigCnt = 0;
            Main();
           
            



        }

        public static void configMaken()
        {
            Console.Title = "Aparecium | Gemaakt door Frank | Config maker | ";
            Text(text: "Wat is de naam van de config: ");
            string naamConfig = Console.ReadLine();
            Console.Title = "Aparecium | Gemaakt door Frank | Config maker | " + naamConfig;
            Console.Clear();

            Text(text: "Wat is de url van de config: ");
            string urlConfig = Console.ReadLine();
            Console.Clear();

            Text(text: "Wat is het woord/de zin waaruit blijkt dat de gebruikersnaam WEL bestaat: ");
            string geldigConfig = Console.ReadLine();
            Console.Clear();

            Text(text: "Wat is het woord/de zin waaruit blijkt dat de gebruikersnaam NIET bestaat: ");
            string onGeldigConfig = Console.ReadLine();
            Console.Clear();

            using (StreamWriter writer = File.AppendText(@"Configs//" + naamConfig + ".apa"))
            {
                writer.WriteLine("Site: \"" + urlConfig + "\"");
                writer.WriteLine("Geldig: \"" + geldigConfig + "\"");
                writer.WriteLine("Ongeldig: \"" + onGeldigConfig + "\"");
                writer.Flush();
                writer.Close();
            }
            Console.Clear();

            Text(text: "Klaar! Klik op een knop om naar het hoofdmenu te gaan.");
            Console.ReadKey();
            ongeldigCnt = 0;
            geldigCnt = 0;
            Main();



        }

        public static void Requests()
        {
            Console.Title = "Aparecium | Gemaakt door Frank | Source verwerker | ";
            Text(text: "Wat is de url van de request: ");
            string url = Console.ReadLine();
            Console.Clear();

            Text(text: "Hoe moet het bestand heten: ");
            string domeinNaam = Console.ReadLine();
            Console.Clear();

            using (HttpRequest request = new HttpRequest())
            {
                request.IgnoreProtocolErrors = true;
                request.AllowAutoRedirect = true;
                request.AllowEmptyHeaderValues = true;
                request.IgnoreInvalidCookie = true;
                request.KeepAlive = true;

                string get = request.Get(url).ToString();

                using (StreamWriter writer = File.AppendText(@"Requests//" + domeinNaam + ".apa"))
                {
                    writer.WriteLine(get);
                    writer.Flush();
                    writer.Close();
                }

                Console.Clear();

                Text(text: "Klaar! Klik op een knop om naar het hoofdmenu te gaan.");
                Console.ReadKey();
                ongeldigCnt = 0;
                geldigCnt = 0;
                Main();

                System.Diagnostics.Process.Start(@"Requests//");



            }
        }

        static void Text(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(string.Format("{0:HH:mm:ss}", DateTime.Now));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" >>> ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);


        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}
