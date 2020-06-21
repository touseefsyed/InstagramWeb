namespace InstagramWeb.Models
{
    public class SessionUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool EmailVerified { get; set; }
        public bool InActive { get; set; }
        public string ConnectionIdentifier { get; set; }
        public string ImagePath { get; set; }

        public string InstagramUsername { get; set; }
        public string InstagramPassword { get; set; }
        public string InstagramUserId { get; set; }
        public string InstagramAuthToken { get; set; }
        public string FacebookAuthToken { get; set; }
        public string FacebookPageId { get; set; }


        public string Proxy { get; set; }
        public SessionUser(User user)
        {
            this.Id = user.Id;
            this.Username = user.Username;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.RoleId = user.RoleId;
            this.EmailVerified = user.EmailVerified;
            this.InActive = user.InActive;
            this.ImagePath = user.ImagePath;

            this.InstagramUserId = user.InstagramUserId;
            this.InstagramPassword = user.InstagramPassword;
            this.InstagramUsername = user.InstagramUsername;
            this.InstagramAuthToken = user.InstagramAuthToken;

            this.FacebookAuthToken = user.FacebookAuthToken;
            this.FacebookPageId = user.FacebookPageId;
            
            if (user.Proxy != null)
            {
                this.Proxy = user.Proxy.IpAddress;
            }
        }
    }
}