using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication
{
    public class Program
    {
        public byte[] GetPostContent()
        {
            JObject postContent = new JObject(
                new JProperty("content","<html><body><div>Hello World!</div></body></html>"),
                new JProperty("pageSize",new JObject(
                    new JProperty("format","A4"),
                    new JProperty("orientation","landscape"),
                    new JProperty("margin",new JObject(
                        new JProperty("top","1cm"),
                        new JProperty("bottom","1cm"),
                        new JProperty("left","1cm"),
                        new JProperty("right","1cm")
                    ))
                ))
            );

            return System.Text.Encoding.UTF8.GetBytes(postContent.ToString());
        }
        public async Task<byte[]> GetPDF()
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                ByteArrayContent postContent = new ByteArrayContent(GetPostContent());
                postContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                try {
                    HttpResponseMessage response = await client.PostAsync("/",postContent);

                    postContent.Dispose();

                    if(response.IsSuccessStatusCode)
                    {
                        foreach(var header in response.Content.Headers)
                        {
                            System.Console.Write("{0}:",header.Key);
                            foreach(var v in header.Value){
                                System.Console.Write("{0}, ",v);
                            }
                            System.Console.WriteLine();
                        }
                        return await response.Content.ReadAsByteArrayAsync();
                    }
                }
                catch(AggregateException ex) {
                    Console.Error.WriteLine(ex.StackTrace);
                }
                return null;

            }

        }

        public static void Main(string[] args)
        {
            Program p = new Program();
            System.Console.WriteLine("Sending request...");
            var t = p.GetPDF();
            t.Wait();
        }
    }
}
