using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Assignment4_FoodRecall.Models;
using Assignment4_FoodRecall.APIHandlerManager;
using Newtonsoft.Json;
using Assignment4_FoodRecall.DataAccess;
using Assignment4_FoodRecall.ModelDto;
using AutoMapper;

namespace Assignment4_FoodRecall.Controllers
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

            APIHandler webHandler = new APIHandler();
            var dataList = webHandler.GetData();

            LoadData(dataList);
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
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Results, FoodRecallDto>().ReverseMap();
            });
            IMapper mapper = config.CreateMapper();
            // var listToReturn = _mapper.Map<List<FoodRecallDto>>(results);

            foreach (Results a in results)
            {
                var validIds = dbContext.FoodRecall.Select(obj => obj.event_id).ToList();
                // _mapper.Map<Results, FoodRecallDto>(a);
                //  FoodRecallDto b = mapper.Map<Results, FoodRecallDto>(a);
                if (!validIds.Contains(a.event_id))
                dbContext.FoodRecall.Add(a); 
            }

            dbContext.SaveChanges();

           // return listToReturn;
        }
        public IActionResult Information()
        {
            ViewData["Message"] = "Information";
            return View(dbContext.FoodRecall.ToList());
        }

        public ActionResult Visualization()
        {
            List<Visualization> dataPoints = new List<Visualization>();

            dataPoints.Add(new Visualization("Class I", 30));
            dataPoints.Add(new Visualization("Class II", 37));
            dataPoints.Add(new Visualization("Class III", 14));
          
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
