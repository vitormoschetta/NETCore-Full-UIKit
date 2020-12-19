using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Frontend.Interfaces;
using Frontend.Models;

namespace Frontend.Services
{
    public class UserService: IUserService
    {
        private readonly GetUserAuth _userAuth;
        public UserService(GetUserAuth userAuth)
        {
            _userAuth = userAuth;
        }
        private string baseUrl = "https://localhost:5001/api/UserAuth";

        public async Task<UserResult> Register(UserRegister user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent conteudo = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{baseUrl}/Register", conteudo))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<UserResult>(apiResponse);
                    return result;
                }
            }
        }

        public async Task<UserResult> RegisterAdmin(UserRegister user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent conteudo = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userAuth.GetToken());

                using (var response = await httpClient.PostAsync($"{baseUrl}/RegisterAdmin", conteudo))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<UserResult>(apiResponse);
                    return result;
                }
            }
        }

        public async Task<UserResult> Login(UserLogin user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent conteudo = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{baseUrl}/Login", conteudo))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<UserResult>(apiResponse);
                    return result;
                }
            }
        }


        public async Task<List<User>> GetInactivesFirstAccess()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userAuth.GetToken());

                using (var response = await httpClient.GetAsync($"{baseUrl}/GetInactivesFirstAccess"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                    return result;
                }
            }
        }

        public async Task<UserResult> ActivateFirstAccess(Guid id, string role = "User")
        {
            var user = new User();
            user.Id = id;
            user.Role = role;

            using (var httpClient = new HttpClient())
            {
                StringContent conteudo = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userAuth.GetToken());

                using (var response = await httpClient.PostAsync($"{baseUrl}/ActivateFirstAccess", conteudo))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<UserResult>(apiResponse);
                    return result;
                }
            }
        }


        public async Task<UserResult> Delete(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userAuth.GetToken());

                using (var response = await httpClient.DeleteAsync($"{baseUrl}/Delete/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<UserResult>(apiResponse);
                    return result;
                }
            }
        }


        public async Task<List<User>> GetAll()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userAuth.GetToken());

                using (var response = await httpClient.GetAsync($"{baseUrl}/GetAll"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                    return users;
                }
            }
        }

        public async Task<User> GetById(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userAuth.GetToken());

                using (var response = await httpClient.GetAsync($"{baseUrl}/GetById/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<User>(apiResponse);
                    return users;
                }
            }
        }

        public async Task<User> GetByName(string name)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userAuth.GetToken());

                using (var response = await httpClient.GetAsync($"{baseUrl}/GetByName/{name}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<User>(apiResponse);
                    return users;
                }
            }
        }


        public async Task<UserResult> UpdateRoleActive(User user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent conteudo = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userAuth.GetToken());

                using (var response = await httpClient.PostAsync($"{baseUrl}/UpdateRoleActive", conteudo))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<UserResult>(apiResponse);
                    return result;
                }
            }
        }

        public async Task<UserResult> UpdatePassword(UserUpdatePassword userUpdatePassword)
        {        
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userAuth.GetToken());

                StringContent conteudo = new StringContent(JsonConvert.SerializeObject(userUpdatePassword), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync($"{baseUrl}/UpdatePassword", conteudo))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<UserResult>(apiResponse);
                    return result;
                }
            }
        }

        public async Task<List<User>> Search(string param)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userAuth.GetToken());

                param = (param == string.Empty || param == null) ? "empty" : param;

                using (var response = await httpClient.GetAsync($"{baseUrl}/Search/{param}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                    return result;
                }
            }
        }

        public Task<List<User>> SearchRequestAccess(string filter)
        {
            throw new NotImplementedException();
        }
    }
}