namespace FraudDetect.Interface.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Logs")]
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime LogDate { get; set; }

        [MaxLength(20)]
        public string Source { get; set; }

        [MaxLength(20000)]
        public string Message { get; set; }
    }
}