using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogusCay.DTO.DTOs.UserDtos;
using DogusCay.Entity.Entities;

namespace DogusCay.DTO.DTOs.DistributorDtos
{
    public class ResultDistributorDto
    {
       
            public int DistributorId { get; set; }
            public string DistributorName { get; set; }
            public string DistributorErcKod { get; set; }

            public SimpleUserDto AppUser { get; set; }
        

    }
}
