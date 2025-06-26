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
                // MalYuklemeTalepFormDetails'ı yükle, ancak Product navigasyonu yok
                .Include(x => x.MalYuklemeTalepFormDetails)
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
                // MalYuklemeTalepFormDetails'ı yükle, ancak Product navigasyonu yok
                .Include(x => x.MalYuklemeTalepFormDetails)
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
               // MalYuklemeTalepFormDetails'ı yükle, ancak Product navigasyonu yok
               .Include(x => x.MalYuklemeTalepFormDetails)
               .OrderByDescending(tf => tf.MalYuklemeTalepFormId)
               .ToList();
        }
        public List<ResultMalYuklemeTalepFormDto> GetAllForIndex()
        {
            var forms = _context.MalYuklemeTalepForms
                .Include(f => f.AppUser)
                .Include(f => f.Kanal)
                .Include(f => f.Point)
                .Include(f => f.MalYuklemeTalepFormDetails)
                .OrderByDescending(f => f.MalYuklemeTalepFormId)
                .ToList();

            var result = forms.Select(form => new ResultMalYuklemeTalepFormDto
            {
                MalYuklemeTalepFormId = form.MalYuklemeTalepFormId,
                AppUserId = form.AppUserId,
                KanalId = form.KanalId,
                DistributorId = form.DistributorId,
                PointGroupTypeId = form.PointGroupTypeId,
                PointId = form.PointId,
                Total = form.Total,
                BrutTotal = form.BrutTotal,
                ToplamAgirlikKg = form.ToplamAgirlikKg,
                Maliyet = form.Maliyet,
                TalepDurumu = form.TalepDurumu,
                CreateDate = DateTime.Now,
                //AppUser = form.AppUser != null ? new ResultUserDto
                //{
                //    UserId = form.AppUser.AppUserId,
                //    FirstName = form.AppUser.FirstName,
                //    LastName = form.AppUser.LastName
                //} : null,

                Kanal = form.Kanal != null ? new ResultKanalDto
                {
                    KanalId = form.Kanal.KanalId,
                    KanalName = form.Kanal.KanalName
                } : null,

                Point = form.Point != null ? new ResultPointDto
                {
                    PointId = form.Point.PointId,
                    PointName = form.Point.PointName
                } : null,

                MalYuklemeTalepFormDetails = form.MalYuklemeTalepFormDetails?.Select(d => new ResultMalYuklemeTalepFormDetailDto
                {
                    ResultMalYuklemeTalepFormDetailId = d.MalYuklemeTalepFormDetailId,
                    ProductId = d.ProductId,
                    ProductName = d.ProductName,
                    ErpCode = d.ErpCode,
                    CategoryId = d.CategoryId,
                    SubCategoryId = d.SubCategoryId,
                    SubSubCategoryId = d.SubSubCategoryId,
                    UnitTypeId = d.UnitTypeId,
                    ApproximateWeightKg = d.ApproximateWeightKg,
                    Price = d.Price,
                    KoliIciAdet = d.KoliIciAdet,
                    Quantity = d.Quantity
                }).ToList()
            }).ToList();

            return result;
        }

    }
}

















































//using System.Collections.Generic;
//using System.Linq;
//using DogusCay.DataAccess.Abstract;
//using DogusCay.DataAccess.Context;
//using DogusCay.DataAccess.Repositories;
//using DogusCay.Entity.Entities.MalYuklemeTalep;
//using DogusCay.Entity.Entities.Talep;
//using Microsoft.EntityFrameworkCore;

//namespace DogusCay.DataAccess.Concrete
//{
//    public class MalYuklemeTalepFormRepository : GenericRepository<MalYuklemeTalepForm>, IMalYuklemeTalepFormRepository
//    {
//        private readonly DogusCayContext _context;

//        public MalYuklemeTalepFormRepository(DogusCayContext context) : base(context)
//        {
//            _context = context;
//        }

//        public List<MalYuklemeTalepForm> GetAllByUserId(int userId)
//        {
//            return _context.MalYuklemeTalepForms
//                .Where(x => x.AppUserId == userId)
//                .Include(x => x.Kanal)
//                .Include(x => x.Distributor)
//                .Include(x => x.PointGroupType)
//                .Include(x => x.Point)
//                .Include(x => x.AppUser)
//                .Include(x => x.MalYuklemeTalepFormDetails)
//                .OrderByDescending(x => x.MalYuklemeTalepFormId)
//                .ToList();
//        }
//        public void UpdateStatus(int formId, TalepDurumu durum, int adminId)
//        {
//            var form = _context.MalYuklemeTalepForms.Find(formId);
//            if (form != null)
//            {
//                form.TalepDurumu = durum;
//                form.OnaylayanAdminId = adminId;
//                _context.SaveChanges();
//            }
//        }

//        public MalYuklemeTalepForm GetDetailsForForm(int formid)
//        {
//            return _context.MalYuklemeTalepForms
//                .Include(x => x.Kanal)
//                .Include(x => x.Distributor)
//                .Include(x => x.PointGroupType)
//                .Include(x => x.Point)
//                .Include(x => x.AppUser)
//                .Include(x => x.OnaylayanAdmin)
//                .Include(x => x.MalYuklemeTalepFormDetails) // Detay objelerini yükle
//                    .ThenInclude(d => d.Product) // **BU SATIR ÇOK ÖNEMLİ VE EKLENMELİ!** Detay objelerinin içindeki Product'ı da yükle
//                .FirstOrDefault(x => x.MalYuklemeTalepFormId == formid);
//        }
//        public List<MalYuklemeTalepForm> GetAllWithUser()
//        {
//            return _context.MalYuklemeTalepForms
//               .Include(tf => tf.AppUser)
//               .Include(tf => tf.Kanal)
//               .Include(tf => tf.Distributor)
//               .Include(tf => tf.PointGroupType)
//               .Include(tf => tf.Point)
//               .OrderByDescending(tf => tf.MalYuklemeTalepFormId)
//               .ToList();
//        }

//    }
//}
