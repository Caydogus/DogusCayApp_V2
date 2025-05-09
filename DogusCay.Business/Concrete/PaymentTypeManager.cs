using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Concrete
{
    public class PaymentTypeManager : GenericManager<PaymentType>, IPaymentTypeService
    {
        private readonly IPaymentTypeService _paymentTypeService;

        public PaymentTypeManager(IRepository<PaymentType> _repository) : base(_repository)
        {
        }
    }
}
