//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JournalAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Articlet
    {
        public string AName { get; set; }
        public string ARtName { get; set; }
        public string Subj_Article { get; set; }
        public System.DateTime Article_Date { get; set; }
        public int ID { get; set; }
        public string Type_user { get; set; }
        public int Article_ID { get; set; }
    
        public virtual ABuser ABuser { get; set; }
        public virtual Authort Authort { get; set; }
    }
}
