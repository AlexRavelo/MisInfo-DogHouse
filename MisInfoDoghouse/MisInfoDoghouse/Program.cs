using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace MisInfoDoghouse
{
    internal class Program
    {

        static void Main(string[] args)
        {
            siteParser("https://kotaku.com/sonic-x-shadow-generations-review-levels-powers-1851676560");
        }

        static void siteParser(string webLink)
        {
            // GET request and load html
            var client = new HttpClient();
	        var html = client.GetStringAsync(webLink).Result;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // TODO: Find out what dir to have the output files save to ==> Gotta Do this on the cloud
            // TODO: Make it so each output file isnt just "input.txt" ===> DONE
            // TODO: Filter out stuff that isnt plaintext ===> DONE!!!
            // TODO: Delete Text File after Everything is done ==> Gotta Do this on the cloud

            String fileName = "input" + randGenerator().ToString() + ".txt";

            File.WriteAllText(fileName, htmlToString(htmlDoc));

            Console.WriteLine("Input Saved");

        }

        static int randGenerator()
        {
            Random rnd = new Random();

            return rnd.Next(100000000,999999999);
        }

        static String htmlToString(HtmlDocument htmlDoc)
        {
            var res = new List<string>();

            // Remove Scripts and CSS Styling from html
            htmlDoc.DocumentNode.Descendants()
                .Where(n => n.Name == "script" || n.Name == "style")
                .ToList()
                .ForEach(n => n.Remove());

            // Get plaintext

            // For this shit, html stuff:
            // Genres: ipc-chip-list__scroller, ipc-chip__text
            // Synopsys: plot
            // Directors, Writers and Actors: ipc-metadata-list-item__list-content-item ipc-metadata-list-item__list-content-item--link
            // Maybe nab em all from ipc-metadata-list__item
            // Title: hero__primary-text

            foreach (HtmlNode? item in htmlDoc.DocumentNode.DescendantsAndSelf())
            {
                if(item.NodeType == HtmlNodeType.Text)
                {
                    if ( item.InnerText.Trim() != "")
                    {
                        res.Add(item.InnerText.Trim());
                    }
                }
            }

            return String.Join(" ", res);
        }
    }
}
