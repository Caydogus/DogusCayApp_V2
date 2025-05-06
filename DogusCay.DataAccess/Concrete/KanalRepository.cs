using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.Entity.Entities;

namespace DogusCay.DataAccess.Concrete
{
    public class KanalRepository : GenericRepository<Kanal>, IKanalRepository
    {
        public KanalRepository(DogusCayContext context) : base(context)
        {
        }
    }
}
