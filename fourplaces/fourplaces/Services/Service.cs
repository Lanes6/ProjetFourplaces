using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using fourplaces.Models;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace fourplaces
{
    public class Service 
    {

        HttpClient httpClient;

        public Service()
        {
            httpClient= new HttpClient();
        }

        public async Task<bool> tryRefresh()
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_REFRESH, string.Empty));
                RefreshRequest temp = new RefreshRequest(App.SESSION_LOGIN.RefreshToken);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(contentResponse);
                    App.SESSION_LOGIN = res.Data;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<ListeLieux> GetPlaces(){
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_GET_PLACES, string.Empty));
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Response<List<PlaceItemSummary>> res = JsonConvert.DeserializeObject<Response<List<PlaceItemSummary>>>(content);
                    return new ListeLieux(res.Data);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        public async Task<PlaceItem> GetPlace(int id)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_GET_PLACES+"/"+id, string.Empty));
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Response<PlaceItem> res = JsonConvert.DeserializeObject<Response<PlaceItem>>(content);
                    return res.Data;
                }
            }catch(Exception e)
            {
                return null;
            }
            return null;
        }

        public async Task<bool> GetUser()
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_USER, string.Empty));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",App.SESSION_LOGIN.AccessToken);
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Response<UserItem> res = JsonConvert.DeserializeObject<Response<UserItem>>(content);
                    App.SESSION_PROFIL = res.Data;
                    return true;
                }
                else
                {
                    if (response.StatusCode==System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await tryRefresh();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.GetAsync(uri);
                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                Response<UserItem> res = JsonConvert.DeserializeObject<Response<UserItem>>(content);
                                App.SESSION_PROFIL = res.Data;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> Login(string mail, string mdp)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_LOGIN, string.Empty));
                LoginRequest temp = new LoginRequest(mail, mdp);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(contentResponse);
                    App.SESSION_LOGIN = res.Data;
                    if (await GetUser())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Register(string mail, string fisrtName, string lastName, string mdp)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_REGISTER, string.Empty));
                RegisterRequest temp = new RegisterRequest(mail, fisrtName, lastName, mdp);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(contentResponse);
                    App.SESSION_LOGIN = res.Data;
                    if (await GetUser())
                    {
                        return true;
                    }
                }
                return false;
            }catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> EditUser(string firstname, string lastName, int? imageId)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_USER, string.Empty));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                UpdateProfileRequest temp = new UpdateProfileRequest(firstname,lastName,imageId);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"),uri);
                requestMessage.Content = contentRequest;

                var response = await httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    if (await GetUser())
                    {
                        return true;
                    }
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await tryRefresh();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.SendAsync(requestMessage);
                            if (response.IsSuccessStatusCode)
                            {
                                if (await GetUser())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> EditMdp(string oldMdp, string mdp)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_USER + App.URI_PASS, string.Empty));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                UpdatePasswordRequest temp = new UpdatePasswordRequest(oldMdp,mdp);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");

                HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
                requestMessage.Content = contentRequest;

                var response = await httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await tryRefresh();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.SendAsync(requestMessage);
                            if (response.IsSuccessStatusCode)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> SendComment(string comment,int? placeId)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_GET_PLACES+"/"+ placeId+App.URI_COMMENT, string.Empty));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                CreateCommentRequest temp = new CreateCommentRequest(comment);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await tryRefresh();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.PostAsync(uri, contentRequest);
                            if (response.IsSuccessStatusCode)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> AddPlace(string title, string description, int imageId, double Latitude, double longitude)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_GET_PLACES, string.Empty));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                CreatePlaceRequest temp = new CreatePlaceRequest(title, description, imageId, Latitude, longitude);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await tryRefresh();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.PostAsync(uri, contentRequest);
                            if (response.IsSuccessStatusCode)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<MediaFile> ChoosePicture()
        {
            await CrossMedia.Current.Initialize();
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                return photo;
            }
            return null;
        }

        public async Task<int?> UploadPicture()
        {
            try
            {
                var uri = new Uri(string.Format(App.URI_BASE + App.URI_POST_IMAGE, string.Empty));
                MediaFile file = await ChoosePicture();
                httpClient = new HttpClient();
                byte[] imageData = File.ReadAllBytes(file.Path);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                MultipartFormDataContent requestContent = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(imageData);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                requestContent.Add(imageContent, "file", "file.jpg");
                request.Content = requestContent;
                HttpResponseMessage response = await httpClient.SendAsync(request);
                string result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Response<ImageItem> res = JsonConvert.DeserializeObject<Response<ImageItem>>(result);
                    return res.Data.Id;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool tryRef = await tryRefresh();
                        if (tryRef)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.SendAsync(request);
                            result = await response.Content.ReadAsStringAsync();
                            if (response.IsSuccessStatusCode)
                            {
                                Response<ImageItem> res = JsonConvert.DeserializeObject<Response<ImageItem>>(result);
                                return res.Data.Id;
                            }
                        }
                    }
                }
                return null;
            }catch(Exception e)
            { 
                return null;
            }
        }

        public async Task<Position> Localisation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                if (CrossGeolocator.IsSupported && CrossGeolocator.Current.IsGeolocationEnabled && CrossGeolocator.Current.IsGeolocationAvailable)
                {
                    return await locator.GetPositionAsync();
                }
                return await locator.GetLastKnownLocationAsync();
            }catch(Exception e){
                return null;
            }
        }

    }
}
