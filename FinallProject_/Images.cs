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
    public partial class Images
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Nullable<int> UserId { get; set; }
        [DataMember]
        public Nullable<int> ProductId { get; set; }
        [DataMember]
        public string img { get; set; }
        [DataMember]
        public Nullable<int> Shipping_CompaneyId { get; set; }
        [DataMember]
        public Nullable<bool> Profile { get; set; }
        [DataMember]
        public virtual Product Product { get; set; }
        [DataMember]
        public virtual User User { get; set; }
    }
}