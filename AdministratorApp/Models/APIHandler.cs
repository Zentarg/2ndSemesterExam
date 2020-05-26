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
        
        /// <summary>
        /// Requests any T item from API.
        /// </summary>
        /// <param name="apiString">API String to request from.</param>
        /// <returns>T object</returns>
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

        /// <summary>
        /// Requests multiple T items from API.
        /// </summary>
        /// <param name="apiString">API String to request from.</param>
        /// <returns>List of T objects</returns>
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

        /// <summary>
        /// Requests deletion of any T item from API
        /// </summary>
        /// <param name="apiString">API string to request deletion of.</param>
        /// <returns>T Object that was deleted.</returns>
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

        /// <summary>
        /// Requests posting of any T item to API
        /// </summary>
        /// <param name="apiString">API string to request posting to.</param>
        /// <param name="objectToPost">Object to post to api.</param>
        /// <returns>T Object posted.</returns>
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


        /// <summary>
        /// Requests update of any T item to API.
        /// </summary>
        /// <param name="apiString">API String to request updating on.</param>
        /// <param name="objectToPut">New object to update to.</param>
        /// <returns>HttpResponseMessage response from API.</returns>
        public static async Task<HttpResponseMessage> PutOne(string apiString, T objectToPut)
        {
            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
            using (HttpClient client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    string item = JsonConvert.SerializeObject(objectToPut);
                    StringContent content = new StringContent(item, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(apiString, content);
                    response.EnsureSuccessStatusCode();
                    //string returnData = await response.Content.ReadAsStringAsync();
                    //T returnItem = JsonConvert.DeserializeObject<T>(returnData);
                    return response;
                }
                catch (Exception ex)
                {
                    return default(HttpResponseMessage);
                }
            }
        }
    }
}
