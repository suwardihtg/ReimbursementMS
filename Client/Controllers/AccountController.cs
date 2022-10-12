using API.ViewModel;
using Client.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            LoginResult loginResult = new LoginResult();
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsJsonAsync("https://localhost:44382/api/User/Login", login))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    loginResult = JsonConvert.DeserializeObject<LoginResult>(apiResponse);
                    HttpContext.Session.SetString("Token", loginResult.Token);
                    if (loginResult.status == 200)
                    {
                        return View("Success");
                    }
                    return View("Index");
                }
            }
        }
    }
}
