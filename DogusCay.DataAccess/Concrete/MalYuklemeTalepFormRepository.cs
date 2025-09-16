using System.Collections.Generic;
using System.Linq;
using DogusCay.DataAccess.Abstract;
using DogusCay.DataAccess.Context;
using DogusCay.DataAccess.Repositories;
using DogusCay.DTO.DTOs.KanalDtos;
using DogusCay.DTO.DTOs.MalYuklemeDtos;
using DogusCay.DTO.DTOs.PointDtos;
using DogusCay.DTO.DTOs.UserDtos;
using DogusCay.Entity.Entities.MalYuklemeTalep;
using DogusCay.Entity.Entities.Talep; // TalepDurumu enum'ı için

using Microsoft.EntityFrameworkCore; // Include metodları için

namespace DogusCay.DataAccess.Concrete
{
    public class MalYuklemeTalepFormRepository : GenericRepository<MalYuklemeTalepForm>, IMalYuklemeTalepFormRepository
    {
        private readonly DogusCayContext _context;
        public MalYuklemeTalepFormRepository(DogusCayContext context) : base(context)
        {
            _context = context;
        }
        public List<MalYuklemeTalepForm> GetAllByUserId(int userId)
        {
            return _context.MalYuklemeTalepForms
                .Where(x => x.AppUserId == userId)
                .Include(x => x.Kanal)
                .Include(x => x.Distributor)
                .Include(x => x.PointGroupType)
                .Include(x => x.Point)
                .Include(x => x.AppUser)
                .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.Product) // Product'ı yükle
                .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.Category) // Ana kategoriyi yükle
                .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.SubCategory) // Alt kategoriyi yükle
                .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.SubSubCategory) // Alt alt kategoriyi yükle
                .OrderByDescending(x => x.MalYuklemeTalepFormId)
                .ToList();
        }

        public void UpdateStatus(int formId, TalepDurumu durum, int adminId)
        {
            var form = _context.MalYuklemeTalepForms.Find(formId);
            if (form != null)
            {
                form.TalepDurumu = durum;
                form.OnaylayanAdminId = adminId;
                _context.SaveChanges();
            }
        }

        public MalYuklemeTalepForm GetDetailsForForm(int formid)
        {
            return _context.MalYuklemeTalepForms
                .Include(x => x.Kanal)
                .Include(x => x.Distributor)
                .Include(x => x.PointGroupType)
                .Include(x => x.Point)
                .Include(x => x.AppUser)
                .Include(x => x.OnaylayanAdmin)
                .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.Product) // Product'ı yükle
                .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.Category) // Ana kategoriyi yükle
                .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.SubCategory) // Alt kategoriyi yükle
                .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.SubSubCategory) // Alt-alt kategoriyi yükle
                .FirstOrDefault(x => x.MalYuklemeTalepFormId == formid);
        }

        public List<MalYuklemeTalepForm> GetAllWithUser()
        {
            return _context.MalYuklemeTalepForms
               .Include(tf => tf.AppUser)
               .Include(tf => tf.Kanal)
               .Include(tf => tf.Distributor)
               .Include(tf => tf.PointGroupType)
               .Include(tf => tf.Point)
               .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.Product) // Product'ı yükle
               .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.Category) // Ana kategoriyi yükle
               .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.SubCategory) // Alt kategoriyi yükle
               .Include(x => x.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.SubSubCategory) // Alt-alt kategoriyi yükle
               .OrderByDescending(tf => tf.MalYuklemeTalepFormId)
               .ToList();
        }

        public List<ResultMalYuklemeTalepFormDto> GetAllForIndex()
        {
            var forms = _context.MalYuklemeTalepForms
                .Include(f => f.AppUser)
                .Include(f => f.Kanal)
                .Include(f => f.Point)
                .Include(f => f.Distributor)
                .Include(f => f.PointGroupType)
                .Include(f => f.MalYuklemeTalepFormDetails)
                    .ThenInclude(d => d.Product)
                        .ThenInclude(p => p.Category)
                            .ThenInclude(c => c.ParentCategory)
                                .ThenInclude(pc => pc.ParentCategory)
                .OrderByDescending(f => f.MalYuklemeTalepFormId)
                .ToList();

            var result = forms.Select(form => new ResultMalYuklemeTalepFormDto
            {
                MalYuklemeTalepFormId = form.MalYuklemeTalepFormId,
                AppUserId = form.AppUserId,


                KanalId = form.KanalId,
                Kanal = form.Kanal != null ? new ResultKanalDto
                {
                    KanalId = form.Kanal.KanalId,
                    KanalName = form.Kanal.KanalName
                } : null,
                PointId = form.PointId,
                Point = form.Point != null ? new ResultPointDto
                {
                    PointId = form.Point.PointId,
                    PointName = form.Point.PointName
                } : null,

                Total = form.Total,
                BrutTotal = form.BrutTotal,
                ToplamAgirlikKg = form.ToplamAgirlikKg,
                Maliyet = form.Maliyet,
                TalepDurumu = form.TalepDurumu,
                CreateDate = form.CreateDate,
                MalYuklemeTalepFormDetails = form.MalYuklemeTalepFormDetails?.Select(d => new ResultMalYuklemeTalepFormDetailDto
                {
                    ResultMalYuklemeTalepFormDetailId = d.MalYuklemeTalepFormDetailId,
                    ProductId = d.ProductId,
                    ProductName = d.Product?.ProductName,
                    ErpCode = d.Product?.ErpCode,
                    CategoryId = d.CategoryId,
                    SubCategoryId = d.SubCategoryId,
                    SubSubCategoryId = d.SubSubCategoryId,

                    CategoryName = d.Product?.Category?.ParentCategory?.ParentCategory?.CategoryName,
                    SubCategoryName = d.Product?.Category?.ParentCategory?.CategoryName,
                    SubSubCategoryName = d.Product?.Category?.CategoryName,

                    UnitTypeId = d.UnitTypeId,
                    ApproximateWeightKg = d.ApproximateWeightKg,
                    Price = d.Price,
                    KoliIciAdet = d.KoliIciAdet,
                    Quantity = d.Quantity,
                    Discount1 = d.Discount1,
                    Discount2 = d.Discount2,
                    FixedPrice = d.FixedPrice,
                    NetTutar = d.NetTutar,
                    NetAdetFiyat = d.NetAdetFiyat,
                    BrutTutar = d.BrutTutar,
                    Maliyet = d.Maliyet
                }).ToList()
            }).ToList();

            return result;
        }

        public MalYuklemeTalepForm GetByIdWithUserAndPoint(int id)
        {
            return _context.MalYuklemeTalepForms
                .Include(x => x.AppUser)
                .Include(x => x.Point)
                .FirstOrDefault(x => x.MalYuklemeTalepFormId == id);
        }

    }
}



