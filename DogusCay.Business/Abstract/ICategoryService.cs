using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DTO.DTOs.CategoryDtos;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Abstract
{
    public interface ICategoryService : IGenericService<Category>
    {
        void TShowOnHome(int id);
        void TDontShowOnHome(int id);
    }
}
