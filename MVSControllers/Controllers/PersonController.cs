using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MVSControllers.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MVSControllers.Controllers
{
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly string _conString = "Data source = NILUFARSHEROVA; Initial catalog = MVCHomeWorkdb; Integrated Security = True";
        private readonly IConfiguration _configuration;
        public PersonController(ILogger<PersonController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _conString = _configuration.GetConnectionString("Default");
        }
        // GET: PersonController
        [HttpGet]
		public async Task<IActionResult> Index()
		{
			var persons = new List<PersonsModel>();
			try
			{
				using (IDbConnection conn = new SqlConnection(_conString))
				{
					persons = (await conn.QueryAsync<PersonsModel>("SELECT * FROM Person")).ToList();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return View(persons);
		}

        // GET: PersonController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
