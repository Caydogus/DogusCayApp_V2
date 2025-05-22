using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.Entity.Entities.Talep;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
                .Include(tf => tf.AppUser)
                .Include(tf => tf.Kanal)
                .Include(tf => tf.Distributor)
                .Include(tf => tf.PointGroupType)
                .Include(tf => tf.Point)
                .OrderByDescending(tf => tf.TalepFormId)
                .ToList();
        }

        public List<TalepForm> GetAllByUserId(int userId)
        {
            return _context.TalepForms
                .Where(tf => tf.AppUserId == userId)
                .Include(tf => tf.Kanal)
                .Include(tf => tf.Distributor)
                .Include(tf => tf.PointGroupType)
                .Include(tf => tf.Point)
                .OrderByDescending(tf => tf.TalepFormId)
                .ToList();
        }

        public void UpdateStatus(int formId, TalepDurumu durum, int adminId)
        {
            var form = _context.TalepForms.Find(formId);
            if (form != null)
            {
                form.TalepDurumu = durum;
                form.OnaylayanAdminId = adminId;
                _context.SaveChanges();
            }
        }

        public TalepFormItem GetItemById(int itemId)
        {
            return _context.TalepFormItems
                .Include(x => x.Product)
                .Include(x => x.Category)
                .Include(x => x.SubCategory)
                .FirstOrDefault(x => x.TalepFormItemId == itemId);
        }

        public void UpdateItemFields(int itemId, int quantity, DateTime? validFrom, DateTime? validTo)
        {
            var item = _context.TalepFormItems.Find(itemId);
            if (item != null)
            {
                item.Quantity = quantity;
                item.ValidFrom = validFrom;
                item.ValidTo = validTo;
                _context.SaveChanges();
            }
        }

        public TalepForm GetDetailsForForm(int formId)
        {
            return _context.TalepForms
                .Include(tf => tf.AppUser)
                .Include(tf => tf.Kanal)
                .Include(tf => tf.Distributor)
                .Include(tf => tf.PointGroupType)
                .Include(tf => tf.Point)
                .Include(tf => tf.TalepFormItems)
                    .ThenInclude(item => item.Product)
                .Include(tf => tf.TalepFormItems)
                    .ThenInclude(item => item.Category)
                .Include(tf => tf.TalepFormItems)
                    .ThenInclude(item => item.SubCategory)
                .FirstOrDefault(tf => tf.TalepFormId == formId);
        }
    }
}
