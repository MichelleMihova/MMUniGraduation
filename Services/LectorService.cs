using MMUniGraduation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMUniGraduation.Services
{
    public class LectorService
    {
        private readonly ApplicationDbContext _db;
        public LectorService(ApplicationDbContext db)
        {
            _db = db;
        }
    }
}
