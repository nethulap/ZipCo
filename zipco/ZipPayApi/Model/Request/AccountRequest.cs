using System.ComponentModel.DataAnnotations;

namespace ZipPayApi.Model
{
    public class AccountRequest
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}