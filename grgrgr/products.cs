//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace grgrgr
{
    using System;
    using System.Collections.Generic;
    
    public partial class products
    {
        public int product_id { get; set; }
        public string name { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<int> userId { get; set; }
        public byte[] product_img { get; set; }
 

        public virtual users users { get; set; }
    }
}