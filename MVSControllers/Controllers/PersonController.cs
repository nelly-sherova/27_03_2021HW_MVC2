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
        private readonly string _conString = "Data source = (local); Initial catalog = MVCHomeWorkdb; Integrated Security = True";
        private readonly IConfiguration _configuration;
        public PersonController(ILogger<PersonController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            //_conString = _configuration.GetConnectionString("Default");
            Console.WriteLine(_conString);
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


        // GET: PersonController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        public async Task<IActionResult> Create(PersonsModel model)
        {
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                using (IDbConnection conn = new SqlConnection(_conString))
                {
                    await conn.ExecuteAsync("INSERT INTO Person(FirstName,LastName,MiddleName) VALUES(@FirstName,@LastName,@MiddleName) ", model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index");
        }
		[HttpGet]
		public async Task<IActionResult> FindById(int id)
		{
			if (id <= 0)
			{
				return RedirectToAction("Index", "Home");
			}
			var lst = new List<PersonsModel>();
			try
			{
				using (IDbConnection conn = new SqlConnection(_conString))
				{
					lst = (await conn.QueryAsync<PersonsModel>($"SELECT * FROM Person WHERE Id = {id}")).ToList();
				}
			}
			catch (Exception ex)
			{
				System.Console.WriteLine(ex.Message);
			}
			return View("Index", lst);
		}
		[HttpGet]
		public async Task<IActionResult> FindByFio(string fio)
		{
			if (fio == null || fio == "")
			{
				return RedirectToAction("Index", "Home");
			}
			var lst = new List<PersonsModel>();
			try
			{
				using (IDbConnection conn = new SqlConnection(_conString))
				{
					lst = (await conn.QueryAsync<PersonsModel>($"SELECT * FROM Person WHERE (LastName+' '+FirstName+' '+MiddleName) LIKE '%{fio}%' ")).ToList();
				}
			}
			catch (Exception ex)
			{
				System.Console.WriteLine(ex.Message);
			}
			return View("Index", lst);
		}

	}
}
