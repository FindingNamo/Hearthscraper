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
            // Settings are in here for the program
            String filePath = @"C:\Users\davidngo\Desktop\cards.txt";

            // List of cards
            List<Card> cards = new List<Card>();

            // Open file
            using (StreamReader reader = new StreamReader(filePath))
            {
                // look for cards with an image
                string currentLine = "";
                while (reader.Peek() > 0)
                {
                    currentLine = reader.ReadLine();

                    //if the card has an image add to it to the list
                    if (currentLine.Contains("image"))
                    {
                        currentLine = currentLine.Substring(currentLine.IndexOf("{"), currentLine.IndexOf("}") - currentLine.IndexOf("{") + 1);
                        cards.Add(Utilities.GetCardFromJson(currentLine));
                    }
                }
            }

            // Download Image for each card
            foreach (Card card in cards)
            {
                using (WebClient webclient = new WebClient())
                {
                    webclient.DownloadFile(card.imageURL, @"C:\Users\davidngo\Desktop\out\" + card.image + ".png");
                }
            }
        }
    }
}
