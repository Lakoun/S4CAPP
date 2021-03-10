using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace S4CAPP.Data
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int LicenseId { get; set; }
        public virtual License License { get; set; }

        [Required]
        public string ProductIdentifier { get; set; }

        [Required]
        public string ProductName { get; set; }
    }
}
