using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections;


namespace Hearthscraper
{
    class Program
    {        
        static void Main(string[] args)
        {
            bool parsingMechanics = false;

            // Settings are in here for the program
            String tierPath = @"C:\Users\davidngo\Desktop\trump_all.json";
            String cardPath = @"C:\Users\davidngo\Desktop\cards.txt";
            
            // List of card tiers
            List<CardTier> tiers;

            // List of cards
            List<Card> cards = new List<Card>();

            // Open file
            using (StreamReader reader = new StreamReader(tierPath))
            {
                // look for cards with an image
                string content = reader.ReadToEnd();
                tiers = Utilities.GetTiersFromJson(content);
            }

            using (StreamReader reader = new StreamReader(cardPath))
            {
                string currentLine = "";
                string jsonString = "";
                Card currentCard;
                while (reader.Peek() >= 0)
                {

                    currentLine = reader.ReadLine();

                    if (currentLine.Contains("g_hearthstone_mechanics"))
                    {
                        parsingMechanics = true;
                        continue;
                    }

                    if (!parsingMechanics && currentLine.Contains("\"id\""))
                    {
                        jsonString = currentLine.Substring(currentLine.IndexOf("{"), currentLine.IndexOf("}") - currentLine.IndexOf("{") + 1);
                        currentCard = Utilities.GetCardFromJson(jsonString);
                        cards.Add(currentCard);
                    }
                    parsingMechanics = false;
                }
            }

            bool matchFound = false;

            foreach (CardTier tier in tiers)
            {
                foreach (Card card in cards)
                {
                    if (tier.name.ToLower() == card.name.ToLower())
                        matchFound = true;
                }

                if (!matchFound)
                {
                    Console.WriteLine(tier.name);
                }

                matchFound = false;
            }

        }
    }
}
