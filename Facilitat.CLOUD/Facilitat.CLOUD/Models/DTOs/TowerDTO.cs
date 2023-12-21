using System.Collections.Generic;

namespace Facilitat.CLOUD.Models.DTOs
{
    public class TowerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<ApartmentDTO> Apartments { get; set; }
    }
}
