using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace S4CAPP.Data
{
    public class Signature
    {
        [Key]
        public int SignatureId { get; set; }

        public int LicenseId { get; set; }

        [Required]
        public string Salt { get; set; }
        [Required]
        public string Hash { get; set; }
    }
}
