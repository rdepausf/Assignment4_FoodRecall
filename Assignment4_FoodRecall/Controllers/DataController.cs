using FoodRecall_Group11.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FoodRecall_Group11.ModelDto;
using FoodRecall_Group11.Models;

namespace FoodRecall_Group11.Controllers
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
