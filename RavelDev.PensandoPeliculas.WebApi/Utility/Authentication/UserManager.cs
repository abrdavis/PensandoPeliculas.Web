using RavelDev.PensandoPeliculas.Entity;
using RavelDev.PensandoPeliculas.Entity.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace RavelDev.PensandoPeliculas.WebApi.Utility.Authentication
{
    public class UserManager
    {
        private PeliculaDbContext conext;

        public UserManager(PeliculaDbContext dbCntext)
        {
            this.conext = dbCntext;
        }

        public bool CreateUser(string userName, string password)
        {
            try
            {
                var user = new User();
                user.UserName = userName;
                user.Password = BCryptNet.HashPassword(password);

                // save user
                conext.Users.Add(user);
                conext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }
    }
}
