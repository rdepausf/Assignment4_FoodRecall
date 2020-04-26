using System;
using System.Linq;
using System.Collections.Generic;
using FoodRecall_Group11.Models;
using FoodRecall_Group11.APIHandlerManager;
using FoodRecall_Group11.DataAccess;

namespace HawaiiCrimeDetails.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext dbContext)
        {
            APIHandler webHandler = new APIHandler();
            var dataList = webHandler.GetData();

            //var config = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<Results, FoodRecallDto>().ReverseMap();
            //});
            //IMapper mapper = config.CreateMapper();
            // var listToReturn = _mapper.Map<List<FoodRecallDto>>(results);

            foreach (Results a in dataList)
            {
                var validIds = dbContext.FoodRecall.Select(obj => obj.recall_number).ToList();
                // _mapper.Map<Results, FoodRecallDto>(a);
                //  FoodRecallDto b = mapper.Map<Results, FoodRecallDto>(a);
                if (!validIds.Contains(a.recall_number))
                    dbContext.FoodRecall.Add(a);
            }

            dbContext.SaveChanges();

        }
    }
}