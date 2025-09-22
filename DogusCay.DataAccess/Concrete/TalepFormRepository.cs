
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.DTO.DTOs.ExcelDtos;
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

        //admin tum talep formlarını gorur
        public List<TalepForm> GetAllWithUser()
        {
            return _context.TalepForms
                .Include(tf => tf.AppUser)
                .Include(tf => tf.Kanal)
                .Include(tf => tf.Distributor)
                .Include(tf => tf.PointGroupType)
                .Include(tf => tf.Point)
                .Include(tf => tf.Category)
                .Include(tf => tf.SubCategory)
                .Include(tf => tf.SubSubCategory)
                .OrderByDescending(tf => tf.TalepFormId)
                .ToList();
        }
        //bolge muduru sadece kendi talep formlarını görür
        public List<TalepForm> GetAllByUserId(int userId)
        {
            return _context.TalepForms
                .Where(tf => tf.AppUserId == userId)
                .Include(tf => tf.Kanal)
                .Include(tf => tf.Distributor)
                .Include(tf => tf.PointGroupType)
                .Include(tf => tf.Point)
                .Include(tf => tf.Category)
                .Include(tf => tf.SubCategory)
                .Include(tf => tf.SubSubCategory)
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
        public void UpdateItemFields(int itemId, int quantity, DateTime validFrom, DateTime validTo)
        {
            var item = _context.TalepForms.Find(itemId);
            if (item != null)
            {
                item.Quantity = quantity;
                item.ValidFrom = validFrom;
                item.ValidTo = validTo;
                _context.SaveChanges();
            }
        }
        
        //belirli talep formunun detaylarını getir.indexe çek
        public TalepForm GetDetailsForForm(int formId)
        {
            return _context.TalepForms
                .Include(tf => tf.AppUser)
                .Include(tf => tf.Kanal)
                .Include(tf => tf.Distributor)
                .Include(tf => tf.PointGroupType)
                .Include(tf => tf.Point)
                .Include(tf => tf.Category)
                .Include(tf => tf.SubCategory)
                .Include(tf => tf.SubSubCategory)
                .FirstOrDefault(tf => tf.TalepFormId == formId);
        }

        public TalepForm GetByIdWithUserAndPoint(int id)
        {
            return _context.TalepForms
        .Include(x => x.AppUser)
        .Include(x => x.Point)
        .FirstOrDefault(x => x.TalepFormId == id);
        }

        public List<TalepForm> GetAllForExport()
        {
            return _context.TalepForms
       .Include(tf => tf.AppUser)
       .Include(tf => tf.Kanal)
       .Include(tf => tf.Distributor)
       .Include(tf => tf.PointGroupType)
       .Include(tf => tf.Point)
       .Include(tf => tf.Category)
       .Include(tf => tf.SubCategory)
       .Include(tf => tf.SubSubCategory)
       .Include(tf => tf.Product)
       .Include(tf => tf.OnaylayanAdmin)
       .AsNoTracking()
       .OrderByDescending(tf => tf.TalepFormId)
       .ToList();
        }

        public List<TalepForm> GetListForExportByUserId(int userId)
        {
            return _context.TalepForms
        .Where(tf => tf.AppUserId == userId)
        .Include(tf => tf.AppUser)
        .Include(tf => tf.Kanal)
        .Include(tf => tf.Distributor)
        .Include(tf => tf.PointGroupType)
        .Include(tf => tf.Point)
        .Include(tf => tf.Category)
        .Include(tf => tf.SubCategory)
        .Include(tf => tf.SubSubCategory)
        .Include(tf => tf.Product)
        .Include(tf => tf.OnaylayanAdmin)
        .AsNoTracking()
        .OrderByDescending(tf => tf.TalepFormId)
        .ToList();
        }

     
    }
}
