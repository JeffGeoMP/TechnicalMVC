using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using TechnicalMVC.Data;
using TechnicalMVC.Models.Entities;


namespace TechnicalMVC.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ClientController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            var applicationURL = _configuration.GetSection("Url").Value;
            ViewBag.ApplicationURL = applicationURL;

            return View();
        }

        [HttpGet]
        public IActionResult AddClient()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditClient(int clientId)
        {
            var client = _context.Clients.Find(clientId);
            return View(client);
        }

        /// <summary>
        /// Get for all clients.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _context.Clients.ToListAsync();
            return Ok(clients);
        }

        /// <summary>
        /// Delete client by id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteClient(int clientId)
        {
            try
            {
                var client = await _context.Clients.FindAsync(clientId);
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Add new client.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] Client client)
        {
            try
            {
                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Edit client.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditClient([FromBody] Client client)
        {
            try
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }   
    }
}
