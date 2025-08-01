using PharmaMoov.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PharmaMoov.Models
{

    public class DeliveryJob : APIBaseModel
    {
        [Key]
        public int Id { get; }
        public int OrderId { get; set; }
        public string AssignmentCode { get; set; }
        public EDeliveryStatus Status { get; set; }
        public string CreateParam { get; set; }
        [IgnoreDataMember]
        public virtual Order Order { get; set; }
    }
 
}
