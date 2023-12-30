namespace FundraisingApp.Entities
{
    public class Donor : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
    }
}
