using System.ComponentModel.DataAnnotations;

namespace DogusCay.Entity.Entities
{
    public class Region
    {
        public int RegionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RegionName { get; set; }
      
        // Bu bölgenin müdürü (tek kullanıcı)
        public int? ManagerUserId { get; set; }
        public AppUser ManagerUser { get; set; }


        // Bölgeye bağlı tüm kullanıcılar (satış temsilcileri)
        public ICollection<AppUser> Users { get; set; }

        public ICollection<PointGroup> PointGroups { get; set; }
       
    }

}

