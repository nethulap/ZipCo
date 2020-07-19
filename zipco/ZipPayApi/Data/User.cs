using System.ComponentModel.DataAnnotations.Schema;

namespace ZipPayApi.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal Salary { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal Expenses { get; set; }
    }
}