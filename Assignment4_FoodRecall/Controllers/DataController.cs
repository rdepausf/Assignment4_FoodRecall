using Assignment4_FoodRecall.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Assignment4_FoodRecall.ModelDto;
using Assignment4_FoodRecall.Models;

namespace Assignment4_FoodRecall.Controllers
{
    [Produces("application/json")]
    [Route("api/data")]
    public class DataController : Controller
    {
        public ApplicationDbContext dbContext;

        private readonly IMapper _mapper;

        public DataController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public DataController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        [HttpGet()]
        public IActionResult GetData()
        {
            List<Results> list = dbContext.FoodRecall
                                        .Include(c => c.country)
                                        .OrderBy(c => c.event_id)
                                        .ToList();

            var listToReturn = _mapper.Map<List<Results>>(list);
            return Ok(listToReturn);
        }
    }
}
