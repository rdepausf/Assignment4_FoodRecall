using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodRecall_Group11.Models;
using FoodRecall_Group11.APIHandlerManager;
using Newtonsoft.Json;
using FoodRecall_Group11.DataAccess;
using FoodRecall_Group11.ModelDto;
using AutoMapper;
using FoodRecall_Group11.Models.ViewModel;

namespace FoodRecall_Group11.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ApplicationDbContext dbContext;

        private readonly IMapper _mapper;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            dbContext = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "About";
            return View();
        }
        public void LoadData(List<Results> results)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Results, FoodRecallDto>().ReverseMap();
            });
            IMapper mapper = config.CreateMapper();
            foreach (Results a in results)
            {
                var validIds = dbContext.FoodRecall.Select(obj => obj.recall_number).ToList();
                if (!validIds.Contains(a.recall_number))
                    dbContext.FoodRecall.Add(a);
            }

            dbContext.SaveChanges();
        }
        [HttpGet]
        public IActionResult Information()
        {
            ViewData["Message"] = "Information";
            return View(dbContext.FoodRecall.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Information(string id, string succ)
        {
            List<Results> details = dbContext.FoodRecall.Where(a => a.classification == id).OrderByDescending(x => x.report_date).ToList();

            if (succ == "True")
                ViewBag.Success = 1;

            if (succ == "edittrue")
                ViewBag.EditSuccess = 1;
            return View(details);
        }

        [HttpGet]
        public IActionResult Classifications()
        {
            var type_counts = dbContext.FoodRecall.GroupBy(a => a.classification).OrderBy(group => group.Key).Select(group => Tuple.Create(group.Key, group.Count())).ToList();

            List<CountbyClass> counttypes = new List<CountbyClass>();
            foreach (var a in type_counts)
            {
                counttypes.Add(new CountbyClass { classtype = a.Item1, count = a.Item2 });
            }

            return View(counttypes);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            Results d = dbContext.FoodRecall.Where(a => a.recall_number == id).FirstOrDefault();
            return View(d);
        }

        [HttpPost]
        public ActionResult Edit(Results res)
        {
            var d = dbContext.FoodRecall.Where(a => a.recall_number == res.recall_number).FirstOrDefault();
            var newd = d;
            newd.product_description = res.product_description;
            newd.product_quantity = res.product_quantity;

            dbContext.FoodRecall.Remove(d);
            dbContext.SaveChanges();
            dbContext.FoodRecall.Add(newd);
            dbContext.SaveChanges();
            return RedirectToAction("Information", new { id = newd.classification, succ = "edittrue" });
        }


        public IActionResult Delete(string id)
        {
            string type = "";
            try
            {
                var del = dbContext.FoodRecall.Where(a => a.recall_number == id).FirstOrDefault();
                type = del.classification;
                dbContext.FoodRecall.Remove(del);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //  return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Information", new { id = type , succ = true});
        }
        public ActionResult Visualization()
        {
            var type_counts = dbContext.FoodRecall.GroupBy(a => a.classification).OrderBy(group => group.Key).Select(group => Tuple.Create(group.Key, group.Count())).ToList();

            List<Visualization> dataPoints = new List<Visualization>();
            foreach (var a in type_counts)
            {
                dataPoints.Add(new Visualization(a.Item1, a.Item2));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
