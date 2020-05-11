using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AdministratorApp.Models
{
    public static class APIHandler<T>
    {
        private static string url = "http://localhost:52889/api/";
        

        public static async Task<T> GetOne(string apiString)
        {
            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (HttpClient client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.GetAsync(apiString);
                    response.EnsureSuccessStatusCode();
                    string data = await response.Content.ReadAsStringAsync();
                    T item = JsonConvert.DeserializeObject<T>(data);
                    return item;
                } catch(Exception e)
                {
                    return default(T);
                }

            }
        }

        public static async Task<List<T>> GetMultiple(string apiString)
        {
            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (HttpClient client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.GetAsync(apiString);
                    response.EnsureSuccessStatusCode();
                    string data = await response.Content.ReadAsStringAsync();
                    List<T> item = JsonConvert.DeserializeObject<List<T>>(data);
                    return item;
                }
                catch (Exception e)
                {
                    return default(List<T>);
                }

            }
        }

        public static async Task<T> DeleteOne(string apiString)
        {
            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (HttpClient client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.DeleteAsync(apiString);
                    response.EnsureSuccessStatusCode();
                    string data = await response.Content.ReadAsStringAsync();
                    T item = JsonConvert.DeserializeObject<T>(data);
                    return item;
                }
                catch (Exception e)
                {
                    return default(T);
                }

            }
        }

        public static async Task<T> PostOne(string apiString, T objectToPost)
        {
            HttpClientHandler handler = new HttpClientHandler() {UseDefaultCredentials = true};
            using (HttpClient client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    string item = JsonConvert.SerializeObject(objectToPost);
                    StringContent content = new StringContent(item, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(apiString, content);
                    response.EnsureSuccessStatusCode();
                    string returnData = await response.Content.ReadAsStringAsync();
                    T returnItem = JsonConvert.DeserializeObject<T>(returnData);
                    return returnItem;
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }
        }
    }
}
