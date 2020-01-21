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

        public Request Request { get; set; }

        [Required]
        public DateTime ResponseDate { get; set; }

        [MaxLength(32000)]
        public string JsonResponse { get; set; }
    }
}