using IdentityService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    public class OAuthController : ControllerBase
#pragma warning restore CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    {
        private readonly IdentityContext _context;

        public OAuthController(IdentityContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Регистрация пользователя.
        /// </summary>         
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterUserInfo>> RegisterUser(RegisterUser user)
        {
            //проверяем полученную регистрационную модель
            if (user == null) return BadRequest();
            else
            {                
                //проверяем нет ли пользователя с таким логином
                if (_context.User.Any(i => i.login == user.login)) ModelState.AddModelError("login", "Пользователь с таким логином уже существует");
                //общую проверку модели делаем
                if (!ModelState.IsValid) return BadRequest(ModelState);
                //добавляем пользователя в базу данных
                User newuser = new User() { 
                    login = user.login, 
                    name = user.name, 
                    password = GetHashStringMS(user.password), 
                    roleid = 2,
                    phone = user.phone,
                    email = user.email
                };
                _context.Add(newuser);
                await _context.SaveChangesAsync();
                RegisterUserInfo userInfo = new RegisterUserInfo() { 
                    id = newuser.id, 
                    name = newuser.name,
                    phone = newuser.phone,
                    email=newuser.email
                };
                return CreatedAtAction("TokenClaims", new { id = newuser.id }, userInfo);
            }
        }

        //метод получения информации по пользователю
        /// <summary>
        /// Получение информации по пользователю по ид.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ICollection<KeyValue>>> TokenClaims()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;                
                return claims.Select(c => new KeyValue() { Key = c.Type.ToString(), Value = c.Value }).ToList();
            }
            else return BadRequest();            
        }


        /// <summary>
        /// Получение токена доступа по логину и паролю.
        /// </summary>
        [HttpPost]
        public ActionResult<string> GetTokenByLogin([FromForm] string username, [FromForm] string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            //возвращаем полученный токен
            return encodedJwt;
        }
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = _context.User.Include(i => i.role)
                .FirstOrDefault(x => x.login == username && x.password == GetHashStringMS(password));

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.role.name),
                    new Claim("role", user.role.name)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        //методы кодирования паролей        
        private string GetHashStringMS(string s)
        {
            //переводим строку в байт-массим  
            byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(s);
            //Compute hash based on source data
            byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            int i;
            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (i = 0; i < tmpHash.Length - 1; i++) sOutput.Append(tmpHash[i].ToString("X2"));
            return sOutput.ToString();
        }
    }
}
