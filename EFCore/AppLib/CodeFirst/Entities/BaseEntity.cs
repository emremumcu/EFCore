namespace EFCore.AppLib.CodeFirst.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BaseEntity
    {
        // [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // public int Id { get; set; }
        public DateTime RowTimeStamp { get; set; }
    }
}
