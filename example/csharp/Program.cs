using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication
{
    public class Program
    {
        public Stream GetPostContent()
        {
            System.IO.MemoryStream contentStream = new MemoryStream();
            StreamWriter sw = new StreamWriter(contentStream);
            sw.WriteLine(@"{
                content: '<html><body><div>Hello World!</div></body></html>',
                pageSize: {
                    format: 'A4',
                    orientation: 'landscape',
                    margin: {
                        top: '1cm',
                        bottom: '3cm'
                        left: '1cm'
                        right: '1cm'
                    }
                }
            }");
            sw.Flush();
            return contentStream;
        }
        public async Task<byte[]> GetPDF() 
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://pdf-generator/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                StreamContent postContent = new StreamContent(GetPostContent());
                postContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync("/",postContent);

                postContent.Dispose();

                if(response.IsSuccessStatusCode)
                {
                    foreach(var header in response.Content.Headers)
                    {
                        System.Console.WriteLine("{0}:{1}",header.Key, header.Value);
                    }
                    return await response.Content.ReadAsByteArrayAsync();
                } else {
                    return null;
                }
            }
            
        }

        public static void Main(string[] args)
        {
            Program p = new Program();
            System.Console.WriteLine("Sending request...");
            var t = p.GetPDF();
            t.Wait();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
