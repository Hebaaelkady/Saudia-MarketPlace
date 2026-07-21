using System;

namespace Kader.DTOs.Users
{
    public class LoginUserDto
    {
        //public Guid UserId {get;set;}
        public string AccessToken {get;set;}
        public string JwToken {get;set;}
        public DateTime ExpireIn {get;set;}

    }
}
