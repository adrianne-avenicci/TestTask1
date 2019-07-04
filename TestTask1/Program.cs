using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace TestTask1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter URL: ");
            string urlAdress = (Console.ReadLine());

            const int aIndexInt = 20;

            string[] hrefList = GetHref(aIndexInt, GetPage(urlAdress));

            if (hrefList[0] != null) { Console.WriteLine("\nFirst 20 href attribute values of an <a> tag:\n"); }

            for (int i = 0; i < aIndexInt && hrefList[i] != null; i++)
            {
                Console.WriteLine(hrefList[i]);
                File.WriteAllLines($"HREF20.txt", hrefList);
            }

            Console.ReadLine();
        }

        static string GetPage(string urlAdress) //возвращает код страницы
        {
            string page = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try //проверка на формат ввода
            {
                request = (HttpWebRequest)WebRequest.Create(urlAdress);
                response = (HttpWebResponse)request.GetResponse();

                Stream receiveStream = response.GetResponseStream();
                Stream stream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(stream);

                page = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                //Console.WriteLine(page);

                return page;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return ("Failed to load page");
            }
        }

        static string[] GetHref(int indexALength, string page) //получаем значения аттрибута href
        {
            string[] hrefList = new string[indexALength];

            if (page != null)
            {
                Regex regex = new Regex(@"<a\s[^>]*\s*href\s*=\s*(?<word>[^ ><]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                MatchCollection hrefMatch = regex.Matches(page);

                for (int i = 0; i < indexALength; i++)
                {
                    if (i < hrefMatch.Count)
                    {
                        hrefList[i] = hrefMatch[i].Groups["word"].Value; //.Trim(stopSymbol); //режем кавычки
                    }
                    else
                    {
                        hrefList[i] = null;
                    }
                }
            }

            else
            {
                for (int i = 0; i < hrefList.Length; i++)
                {
                    hrefList[i] = null;
                }
            }

            return hrefList;
        }
    }
}
