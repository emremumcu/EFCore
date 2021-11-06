using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.AppLib.CodeFirst.Entities
{
    [Table("InternalLogs")]
    public class InternalLog : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string LogLevel { get; set; }
        
        public string EventId { get; set; }
        
        public string State { get; set; }
        
        public string ExceptionMessage { get; set; }
        
        public string InnerExceptionMessage { get; set; }
    }
}
