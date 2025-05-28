using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.Entity.Entities;

namespace DogusCay.DTO.DTOs.PointDtos
{
    public class ResultPointDto
    {
        [Required(ErrorMessage = "Nokta seçimi zorunludur.")]

        public int PointId { get; set; }
        public string PointName { get; set; }
        public string PointErc { get; set; }
        public string? KanalName { get; set; }
        public string? DistributorName { get; set; }
        public string? PointGroupTypeName { get; set; }
        public string? AppUserFullName { get; set; }
    }
}
