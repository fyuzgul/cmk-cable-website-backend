using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    namespace CmkCable.Entities
    {
        public class ProductCertificate
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [ForeignKey(nameof(Product))]
            public int ProductId { get; set; }


            [ForeignKey(nameof(Certificate))]
            public int CertificateId { get; set; }

            public virtual Product Product { get; set; }
            public virtual Certificate Certificate { get; set; }
        }
    }
}
