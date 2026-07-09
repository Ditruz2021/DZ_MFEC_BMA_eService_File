using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_starter.Models
{
    public class LoggingUser
    {
        public int Id { get; set; }
        public required string RequestId { get; set; }
        public required string Request { get; set; }
        public required string Responses { get; set; }
        public required string Path { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
