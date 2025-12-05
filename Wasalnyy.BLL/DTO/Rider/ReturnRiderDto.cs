namespace Wasalnyy.BLL.DTO.Rider
{
    public class ReturnRiderDto
    {
        public string RiderId { get; set; }
        public string FullName { get; set; }
        public string? Image { get; set; }
        public string phonenumber { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
