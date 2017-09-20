using iMotto.Adapter.Readers.Requests;
using iMotto.Adapter.Readers.Results;
using iMotto.Data;
using iMotto.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMotto.Adapter.Readers.Handlers
{
    class ReadRecruitsHandler:BaseHandler<ReadRecruitsRequest>
    {
        private readonly IRecruitRepo _recruitRepo;

        public ReadRecruitsHandler(IRecruitRepo recruitRepo) 
        {
            _recruitRepo = recruitRepo;
        }

        protected async override Task<HandleResult> HandleCoreAsync(ReadRecruitsRequest reqObj)
        {
            var today = DateTime.Today;
            var start = today.AddDays(-28).ToInteger();
            var threshingStart = today.AddDays(-21).ToInteger();
            var collectingStart = today.AddDays(-14).ToInteger();
            var warmingStart = today.AddDays(-7).ToInteger();
            var end = today.ToInteger();
            /*
                20160301
                
                Warming: 20160322,20160323,20160324,20160325,20160326,20160327
                Gathering:
                Threshing:
            */

            List<Recruit> recruits = await _recruitRepo.GetRecruitsAsync(start, end);

            return new ReadRecruitsResult
            {
                State = HandleStates.Success,
                Warming = recruits.Where(r => r.ID > warmingStart).ToList(),
                Collecting = recruits.Where(r => r.ID > collectingStart && r.ID <= warmingStart).ToList(),
                Threshing = recruits.Where(r => r.ID > threshingStart && r.ID <= collectingStart).ToList(),
                Finalizing = recruits.Where(r => r.ID <= threshingStart).ToList()
            };
        }

       
    }
}
