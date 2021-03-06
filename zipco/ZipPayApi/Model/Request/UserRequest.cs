using System.ComponentModel.DataAnnotations;

namespace ZipPayApi.Model
{
    public class UserRequest
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public decimal Expenses { get; set; }
    }
}