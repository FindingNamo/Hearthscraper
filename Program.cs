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
            Hearthscraper.Settings.Settings settings = new Hearthscraper.Settings.Settings();

            // List of cards
            List<Card> cards = new List<Card>();
            
            for (int cardNumber = 0; cardNumber<2000; cardNumber++)
            {

                // build url
                string currentUrl = settings.Hearthhead.baseURL + cardNumber + settings.Hearthhead.powerURL;
                
                // create web request
                HttpWebRequest request = WebRequest.CreateHttp(currentUrl);

                try
                {
                    // Get response
                    WebResponse response = request.GetResponse();

                    // Get stream from response and shove it into a string
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    string responseBody = streamReader.ReadToEnd();

                    // if it's a valid card store response in object
                    if (responseBody.Contains("\r"))
                    {
                        // Get Name
                        string cardName = responseBody.Substring(responseBody.IndexOf("'") + 1);
                        cardName = cardName.Substring(0, cardName.IndexOf("'"));

                        // Get Image
                        int startImage = responseBody.IndexOf("http");
                        int endImage = responseBody.IndexOf(".png") + 4;
                        string cardPNG = responseBody.Substring(startImage, endImage - startImage);

                        // add to collection
                        cards.Add(new Card(cardName, cardPNG));
                    }
                }
                catch
                {
                }
            }

            // write to file
            string outfile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar + "out.txt";
            StreamWriter streamWriter = new StreamWriter(outfile);
            foreach (Card card in cards)
            {
                streamWriter.WriteLine(card.Name + "\t" + card.Image);
            }
            streamWriter.Flush();
            streamWriter.Close();

        }
    }
}
