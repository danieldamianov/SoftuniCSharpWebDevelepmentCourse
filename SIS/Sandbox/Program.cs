using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SIS.HTTP.Common;
using SIS.HTTP.Requests;

namespace Sandbox
{
    public static class CookieContainerExtensions
    {

        /// <summary>
        /// Uses Reflection to get ALL of the <see cref="Cookie">Cookies</see> where <see cref="Cookie.Domain"/> 
        /// contains part of the specified string. Will return cookies for any subdomain, as well as dotted-prefix cookies. 
        /// </summary>
        /// <param name="cookieContainer">The <see cref="CookieContainer"/> to extract the <see cref="Cookie">Cookies</see> from.</param>
        /// <param name="domain">The string that contains part of the domain you want to extract cookies for.</param>
        /// <returns></returns>
        public static IEnumerable<Cookie> GetCookies(this CookieContainer cookieContainer, string domain)
        {
            var domainTable = GetFieldValue<dynamic>(cookieContainer, "m_domainTable");
            foreach (var entry in domainTable)
            {
                string key = GetPropertyValue<string>(entry, "Key");

                if (key.Contains(domain))
                {
                    var value = GetPropertyValue<dynamic>(entry, "Value");

                    var internalList = GetFieldValue<SortedList<string, CookieCollection>>(value, "m_list");
                    foreach (var li in internalList)
                    {
                        foreach (Cookie cookie in li.Value)
                        {
                            yield return cookie;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the value of a Field for a given object instance.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> you want the value to be converted to when returned.</typeparam>
        /// <param name="instance">The Type instance to extract the Field's data from.</param>
        /// <param name="fieldName">The name of the Field to extract the data from.</param>
        /// <returns></returns>
        internal static T GetFieldValue<T>(object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo fi = instance.GetType().GetField(fieldName, bindFlags);
            return (T)fi.GetValue(instance);
        }

        /// <summary>
        /// Gets the value of a Property for a given object instance.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> you want the value to be converted to when returned.</typeparam>
        /// <param name="instance">The Type instance to extract the Property's data from.</param>
        /// <param name="propertyName">The name of the Property to extract the data from.</param>
        /// <returns></returns>
        internal static T GetPropertyValue<T>(object instance, string propertyName)
        {
            var pi = instance.GetType().GetProperty(propertyName);
            return (T)pi.GetValue(instance, null);
        }

    }
    class Content : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            throw new NotImplementedException();
        }

        protected override bool TryComputeLength(out long length)
        {
            throw new NotImplementedException();
        }
    }
    static class Program
    {

        static void Main(string[] args)
        {
            //CookieContainer cookies = new CookieContainer();
            //HttpClientHandler handler = new HttpClientHandler();
            //handler.CookieContainer = cookies;

            //HttpClient client = new HttpClient(handler);
            //HttpResponseMessage response = client.GetAsync("http://google.com").Result;

            //Uri uri = new Uri("http://google.com");
            //IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
            //foreach (Cookie cookie in responseCookies)
            //    Console.WriteLine(cookie.Name + ": " + cookie.Value);

            //Console.ReadLine();


            //var person = new Person();
            //person.Name = "John Doe";
            //person.Occupation = "gardener";

            //var json = JsonConvert.SerializeObject(person);
            var data = new StringContent("_token=KceU5qwcxU7nySzg1GO0fSG9PntdJ95DvVdn9DDQ&login=danieldamianov02%40gmail.com&password=0246293242Dd&remember=1",
                Encoding.UTF8,
                "text/plain");


            //CookieContainer cookies = new CookieContainer();
            //HttpClientHandler handler = new HttpClientHandler();
            //handler.CookieContainer = cookies;

            HttpClient client = new HttpClient();
            //HttpResponseMessage response = client.GetAsync("http://google.com").Result;

            //Uri uri = new Uri("http://app.shkolo.bg");

            var url = "http://app.shkolo.bg/auth/login";
            //Task<HttpResponseMessage> task = client.GetAsync(url);
            Task<HttpResponseMessage> task = client.PostAsync(url, data);
            task.Wait();
            var response = task.Result;
            //Console.WriteLine(string.Join($"{Environment.NewLine}", response.Headers.GetValues("set-cookie")));
            string cookieString = string.Join("; ",response.Headers.GetValues("set-cookie").Select(unparsedCookie => unparsedCookie.Split("; ", StringSplitOptions.None)
               [0]));
            data.Headers.Add("Cookie", cookieString);

            Task<HttpResponseMessage> task2 = client.PostAsync(url, data);
            task.Wait();
            var response2 = task.Result;
            string result = response.Content.ReadAsStringAsync().Result;
            File.WriteAllText("correctPass.html", result);
            //IEnumerable<Cookie> responseCookies = cookies.GetCookies("http://app.shkolo.bg");
            //foreach (var item in responseCookies)
            //{
            //    Console.WriteLine(item.Name + " " + item.Value);
            //}

            //var url = "http://app.shkolo.bg/auth/login";
            //using (var client = new HttpClient())
            //{
            //    var response1 = await client.PostAsync(url, data);


            //    handler.CookieContainer = cookies;



            //    data.Headers.Add("cookie", string.Join("; ", responseCookies.Select(c => $"{c.Name}={c.Value}")));

            //    var response = await client.PostAsync(url, data);

            //    string result = response.Content.ReadAsStringAsync().Result;
            //    File.WriteAllText("correctPass.html", result);
            //}


        }
        //            HttpRequest httpRequest = new HttpRequest("POST /cgi-bin/process.cgi?id=1&name=dani#fragment HTTP/1.1" + GlobalConstants.HttpNewLine +
        //"User - Agent: Mozilla / 4.0(compatible; MSIE5.01; Windows NT)" + GlobalConstants.HttpNewLine +
        //"Host: www.tutorialspoint.com" + GlobalConstants.HttpNewLine +
        //"Content - Type: application / x - www - form - urlencoded" + GlobalConstants.HttpNewLine +
        //"Content - Length: length" + GlobalConstants.HttpNewLine +
        //"Accept - Language: en - us" + GlobalConstants.HttpNewLine +
        //"Accept - Encoding: gzip, deflate" + GlobalConstants.HttpNewLine +
        //"Connection: Keep - Alive" + GlobalConstants.HttpNewLine
        //+ GlobalConstants.HttpNewLine +
        //"licenseID=string & content = string &/ paramsXML = string");
        //            Console.WriteLine();
        //            Console.WriteLine($"RequestMethod: {httpRequest.RequestMethod}");
        //            Console.WriteLine($"Path: {httpRequest.Path}");
        //            Console.WriteLine($"Url: {httpRequest.Url}");


    }
}

