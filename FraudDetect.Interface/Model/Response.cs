namespace FraudDetect.Interface.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Responses")]
    public class Response
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Request_Id")]
        public Request Request { get; set; }

        public int Request_Id { get; set; }

        [MaxLength(50)]
        public string BureauType { get; set; }

        [Required]
        public DateTime ResponseDate { get; set; }

        public int? Score { get; set; }

        [MaxLength(32000)]
        public string Json { get; set; }

        public bool IsSuccess { get; set; }
    }
}