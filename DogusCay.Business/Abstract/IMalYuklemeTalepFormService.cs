using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DTO.DTOs.MalYuklemeDtos;
using DogusCay.Entity.Entities.MalYuklemeTalep;
using DogusCay.Entity.Entities.Talep;

namespace DogusCay.Business.Abstract
{
    public interface IMalYuklemeTalepFormService: IGenericService<MalYuklemeTalepForm>
    {
        List<MalYuklemeTalepForm> TGetAllWithUser(); // Bölge Müdürü için
        List<MalYuklemeTalepForm> TGetAllByUserId(int userId); // Kullanıcıya özel
        void TUpdateStatus(int formId, TalepDurumu durum, int adminId); // Admin onay/red
        MalYuklemeTalepForm TGetDetailsForForm(int id); // Navigation dahil detay

        MalYuklemeTalepForm TCreateMalYuklemeTalepForm(CreateMalYuklemeTalepFormDto dto, int authenticatedUserId);
        public List<ResultMalYuklemeTalepFormDto> TGetAllForIndex();
    }
}
