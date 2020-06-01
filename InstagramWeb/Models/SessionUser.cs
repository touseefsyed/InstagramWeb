namespace InstagramWeb.Models
{
    public class SessionUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool EmailVerified { get; set; }
        public bool InActive { get; set; }
        public string ConnectionIdentifier { get; set; }
        public string ImagePath { get; set; }

        public string InstagramUsername { get; set; }
        public string InstagramPassword { get; set; }
        public string Proxy { get; set; }
        public SessionUser(User user)
        {
            this.Id = user.Id;
            this.Username = user.Username;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.Password = user.Password;
            this.RoleId = user.RoleId;
            this.EmailVerified = user.EmailVerified;
            this.InActive = user.InActive;
            this.InstagramPassword = user.InstagramPassword;
            this.InstagramUsername = user.InstagramUsername;
            if (user.Proxy != null)
            {
                this.Proxy = user.Proxy.IpAddress;
            }
        }
    }
}