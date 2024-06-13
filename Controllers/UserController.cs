using Microsoft.AspNetCore.Mvc;
using RestApiExample.Models;
using RestApiExample.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiExample.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApiService _apiService;

        public UsersController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = searchString;

            var users = await _apiService.GetUsersAsync();

            // Filtracja danych
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Name.Contains(searchString, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Sortowanie danych
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.Name).ToList();
                    break;
                case "email":
                    users = users.OrderBy(u => u.Email).ToList();
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email).ToList();
                    break;
                default:
                    users = users.OrderBy(u => u.Name).ToList();
                    break;
            }

            return View(users);
        }
    }
}
