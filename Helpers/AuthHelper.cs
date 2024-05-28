using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace Gladwyne.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _configuration;
        public AuthHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
         public byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            // 2.) We're going to use the random string we set up in our appSettings Json file
            //     With that password key, it will be more secure.

            // 2a.) We're going to set our string.
            string passwordSaltPlusString = _configuration.GetSection("AppSettings:PasswordKey").Value + 
                Convert.ToBase64String(passwordSalt);
                
                // 3.) We're creating our 'actual password hash'.
                // 3a.) we are passing in our password.
                // 3b.) We are passing in our salt.
                // 3c.) Method of Hashing. We are describing the schema to use when we are hashing in the 'HMACSHA.."
                // 4d.) Our iteration. This is where we describe to our program how many times we want to hash this.
                //      the more you hash it, the more secure your application is.
                return KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10,
                    numBytesRequested: 256/8
                );
        }

        //JWT Token. This method will return a string that we want to pass back to the user so they are continually authenticated.
        public string CreateToken(int userId)
        {
            //This method takes in UserId
            //And returns a token that we can pass back to the user.

            //A claim is a piece of information inside of a token.
            //If we break open the claim we will be able to pull that information out later.

            //1.) First we need to create a token.
            //1a.) That will be in AppSettings.json
            //1b.) We need to create our claims.

            Claim[] claims = new Claim[] {
                new Claim("userId", userId.ToString()),
            };

            //We are accessing the random string we generated in the app settings json file.
            //This is one layer of complexity in the generation of a jwt token.
            string? tokenKeyString = _configuration.GetSection("AppSettings:TokenKey").Value;

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    tokenKeyString != null ? tokenKeyString : ""
                )
            );
            //Here we are generating our tokenKey. This tokenKey is the product of an encoding operation
            //That is used, in conjunction with the random tokenString key we generated. It is the 'Token' key
            //That allows us to regenerate the same string, because it is our "key".
            SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);
            //This goes ahead and 'signs' our token.

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1) //This articulates the life of the token.
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            //This class has methods that allows us to create and manage the descriptors
            //That we pass back to the user.

            SecurityToken token = tokenHandler.CreateToken(descriptor);

            //We will now convert it to a string, which makes the data itself more portable.
            return tokenHandler.WriteToken(token);
        }
    }
}