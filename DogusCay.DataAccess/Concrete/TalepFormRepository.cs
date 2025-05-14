using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.Entity.Entities.Talep;
using Microsoft.EntityFrameworkCore;

namespace DogusCay.DataAccess.Concrete
{
    public class TalepFormRepository : GenericRepository<TalepForm>, ITalepFormRepository
    {
        private readonly DogusCayContext _context;

        public TalepFormRepository(DogusCayContext context) : base(context)
        {
            _context = context;
        }

        public List<TalepForm> GetAllWithUser()
        {
            return _context.TalepForms
                                    .Include(x => x.AppUser)
                                    .Include(x => x.Items)
                                        .ThenInclude(i => i.Product)
                                    .ToList();
        }

        public List<TalepForm> GetAllByUserId(int userId)
        {
            return _context.TalepForms
                                    .Include(x => x.Items)
                                        .ThenInclude(i => i.Product)
                                    .Where(x => x.AppUserId == userId)
                                    .ToList();
        }

        public void UpdateStatus(int formId, TalepDurumu durum, int adminId)
        {
            var form = _context.TalepForms.Find(formId);
            if (form == null) return;

            form.TalepDurumu = durum;
            form.OnaylayanAdminId = adminId;
            _context.SaveChanges();
        }

        public TalepFormItem GetItemById(int itemId)
        {
            return _context.TalepFormItems
                                        .Include(i => i.Product)
                                        .FirstOrDefault(i => i.TalepFormItemId == itemId);
        }

        public void UpdateItemFields(int TalepFormitemId, int quantity, DateTime? validFrom, DateTime? validTo)
        {
            var item = _context.TalepFormItems.Find(TalepFormitemId);
            if (item == null) return;

            item.Quantity = quantity;
            item.ValidFrom = validFrom;
            item.ValidTo = validTo;

            _context.SaveChanges();
        }

        public TalepForm GetDetailsForForm(int formId)
        {
            return _context.TalepForms
                                    .Include(x => x.AppUser)
                                    .Include(x => x.Items)
                                        .ThenInclude(i => i.Product)
                                    .FirstOrDefault(x => x.TalepFormId == formId);
        }
    }
}
