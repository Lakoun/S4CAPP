using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace S4CAPP.Data
{
    public class License
    {
        [Key]
        public int LicenseId { get; set; }
        [Required]
        public string LicenseName { get; set; }

        public int ProductCount { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Signature Signature { get; set; }
    }
}
