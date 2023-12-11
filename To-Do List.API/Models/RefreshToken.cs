using System.ComponentModel.DataAnnotations.Schema;

namespace To_Do_List.API.Models
{
    [NotMapped]
    public class RefreshToken
    {
        public required string Token { get; set; }
        public DateTime Created { get; } = DateTime.Now;
        public DateTime Exprires { get; set; }
    }
}
