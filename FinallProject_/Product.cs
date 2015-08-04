//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinallProject_
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(IsReference =true)]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Images = new HashSet<Images>();
            this.OrderProduct = new HashSet<OrderProduct>();
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public Nullable<decimal> Price { get; set; }
        [DataMember]
        public bool Condition { get; set; }
        [DataMember]
        public Nullable<int> CategoryId { get; set; }
        [DataMember]
        public int SellerId { get; set; }
        [DataMember]
        public int Rating { get; set; }
        [DataMember]
        public Nullable<System.DateTime> Date { get; set; }
        [DataMember]
        public string Feedback { get; set; }

        [DataMember]
        public virtual Category Category { get; set; }
        [DataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
       
        public virtual ICollection<Images> Images { get; set; }
        [DataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProduct { get; set; }
        [DataMember]
        public virtual User User { get; set; }
    }
}
