using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTelematikaService.Infrastructure.Interfaces;
using TestTelematikaService.Models;

namespace TestTelematikaService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICassetteService _cassetteService;

        public HomeController(ICassetteService cassetteService)
        {
            _cassetteService = cassetteService;
        }

        public IActionResult Index()
        {
           
            return View(Tuple.Create(_cassetteService.GetAllCassettes(), _cassetteService.GetAllNominal()));
        }

        public IActionResult CreateAndGet(int? countCassettes)
        {
            if (countCassettes.HasValue)
                _cassetteService.CreateListCassette(countCassettes.Value);
            return RedirectToAction("Index");
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Index(CassetteModel model, int nominalValue)
        {
            NominalModel nominal = _cassetteService.GetAllNominal().SingleOrDefault(c => c.NominalValue == nominalValue);
            model.NominalValue = nominal;
            _cassetteService.Edit(model);
            return View(Tuple.Create(_cassetteService.GetAllCassettes(), _cassetteService.GetAllNominal()));
        }

        [HttpPost]
        public IActionResult CheckAmount(int? amount)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (!amount.HasValue)
            {
                return RedirectToAction("Index");
            }

            bool check = _cassetteService.IssueBanknotes(amount.Value);
            stopwatch.Stop();
            StringBuilder str = new StringBuilder();
            List<CassetteModel> cassetteModels = _cassetteService.GetIssueBanknotes();
            foreach (var cassette in cassetteModels)
            {
                str.Append(
                    $"{cassette.Quantity} banknotes with a nominal value of {cassette.NominalValue.NominalValue} were issued from cassette {cassette.Id}\\n");
            }
            if (check)
                TempData["alertMessage"] = $"The issue is possible. Time spent {stopwatch.Elapsed.TotalMilliseconds} Milliseconds\\n{str}";
            else
                TempData["alertMessage"] = $"The issue is impossible. Time spent {stopwatch.Elapsed.TotalMilliseconds} Milliseconds";


            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
