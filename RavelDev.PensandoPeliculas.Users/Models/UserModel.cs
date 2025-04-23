namespace RavelDev.PensandoPeliculas.Users.Models
{
    public class UserModel
    {
        public UserModel(string userName, Guid userId)
        {
            UserName = userName;
            UserId = userId;
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<RefreshTokenModel> RefreshTokens { get; set; }
        public int RoleId { get; set; }
    }

}
