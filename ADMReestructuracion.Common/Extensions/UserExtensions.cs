using ADMReestructuracion.Common.Interfaces;
using System.Security.Claims;

namespace ADMReestructuracion.Common.Extensions
{
    public static class UserExtensions
    {
        public static ClaimsIdentity ToIdentity(this IUserEntity user, bool changePassword = false)
        {
            if (changePassword)
            {
                return new ClaimsIdentity(new[] {
             //new Claim(IdentityClaims.UserID, user.IdUsuario),
             new Claim(IdentityClaims.ChangePassword, "1")
         });
            }

            return new ClaimsIdentity(new[] {
         //new Claim(IdentityClaims.UserID, user.Token),
         new Claim(IdentityClaims.ChangePassword, "0"),
         //new Claim(IdentityClaims.IdentityID, $"{user.IdEntidad}"),
         new Claim(IdentityClaims.Name, user.Nombres),
         new Claim(IdentityClaims.Fullname, $"{user.Nombres} {user.Apellidos}"),
         new Claim(IdentityClaims.Email, user.Correo),
         //new Claim(IdentityClaims.AppId, $"{user.AppId ?? Guid.Empty.ToString()}"),
     });
        }
    }

    public static class IdentityClaims
    {
        //public const string UserID = "ipsp://userid";
        public const string ChangePassword = "ipsp://change-password";
        //public const string IdentityID = "ipsp://id";
        public const string Name = "ipsp://username";
        public const string Fullname = "ipsp://fullname";
        public const string Email = "ipsp://email";
        //public const string AppId = "ipsp://appid";
    }
}
