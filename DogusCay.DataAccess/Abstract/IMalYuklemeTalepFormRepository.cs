using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DTO.DTOs.MalYuklemeDtos;
using DogusCay.Entity.Entities;
using DogusCay.Entity.Entities.MalYuklemeTalep;
using DogusCay.Entity.Entities.Talep;


namespace DogusCay.DataAccess.Abstract
{
    public interface IMalYuklemeTalepFormRepository : IRepository<MalYuklemeTalepForm>
    {
        List<MalYuklemeTalepForm> GetAllWithUser(); // Bölge Müdürü için
        List<MalYuklemeTalepForm> GetAllByUserId(int userId); // Kullanıcıya özel
        void UpdateStatus(int formId, TalepDurumu durum, int adminId); // Admin onay/red
        MalYuklemeTalepForm GetDetailsForForm(int formid); // Navigation dahil detay
        public List<ResultMalYuklemeTalepFormDto> GetAllForIndex();
    }
}
