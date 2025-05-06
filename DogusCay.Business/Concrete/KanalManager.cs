using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Concrete;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Concrete
{
    public class KanalManager : GenericManager<Kanal>, IKanalService
    {
       private readonly IKanalService _kanalService;

        public KanalManager(IRepository<Kanal> _repository) : base(_repository)
        {
        }
    }
}
