namespace FraudDetect.Interface.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Requests")]
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        [MaxLength(1000)]
        public string ParseError { get; set; }

        [MaxLength(40)]
        public string ExternalId { get; set; }

        [MaxLength(15)]
        public string Ip { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(30)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(3)]
        public string CountryCode { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(32000)]
        public string Json { get; set; }
    }
}