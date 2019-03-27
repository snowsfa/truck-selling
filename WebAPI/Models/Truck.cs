using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TruckSelling.WebAPI.Models
{
    public class Truck
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TruckId { get; set; }

        [Required]
        public Model Model { get; set; }

        [Required]
        [StringLength(40, MinimumLength=3)]
        public string Chassis { get; set; }

        [Required]
        [Range(1900, 3000)]
        public short Year { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
