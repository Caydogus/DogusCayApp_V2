using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DataAccess.Abstract;
using DogusCay.DTO.DTOs.CategoryDtos;
using DogusCay.Entity.Entities;

namespace DogusCay.Business.Concrete
{
    public class CategoryManager : GenericManager<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryManager(IRepository<Category> _repository, ICategoryRepository CategoryRepository) : base(_repository)
        {
            _categoryRepository = CategoryRepository;
        }

        public void TDontShowOnHome(int id)
        {
            _categoryRepository.DontShowOnHome(id);
        }

        public void TShowOnHome(int id)
        {
            _categoryRepository.ShowOnHome(id);
        }
    }
}
