using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Passwordmanager.Data;
using Passwordmanager.Model;
using Passwordmanager.ViewModel;
using System.Security.Cryptography;
using System.Text;

namespace Passwordmanager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordManagercontroller : Controller
    {
        private readonly ApplicationDbContext _context;

        public PasswordManagercontroller(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("encrypt")]
        public IActionResult encrypt( PasswordManager User)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            byte[] clearBytes = Encoding.Unicode.GetBytes(User.UserName);
            byte[] clearBytess = Encoding.Unicode.GetBytes(User.Password);

            using (Aes encryptor = Aes.Create())
            {

                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
              });


                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);


                using (MemoryStream ms1 = new MemoryStream())
                {

                    using (CryptoStream cs = new CryptoStream(ms1,encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes,0, clearBytes.Length);
                        cs.Write(clearBytess,0, clearBytess.Length);
                        cs.Close();

                    }

                    User.UserName=Convert.ToBase64String(clearBytes.ToArray());
                    User.Password=Convert.ToBase64String(clearBytess.ToArray());

                  //  User.UserName = Convert.ToBase64String(ms1.ToArray());
                  // User.Password = Convert.ToBase64String(ms1.ToArray());

                }

            }

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }


            var encryptionkey = Convert.ToBase64String(randomNumber);
            User.Encryptionkey = encryptionkey;


            _context.PasswordManagers.Add(User);
            _context.SaveChanges();
            return Ok(User);
        }

        [HttpPost]
        [Route("decrypt")]

        public IActionResult Decrypt(PassViewModel cipherText)
        {

            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            byte[] cipherBytes = Encoding.ASCII.GetBytes(cipherText.UserName);
            byte[] cipherBytess = Encoding.ASCII.GetBytes(cipherText.Password);


            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
               
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                encryptor.Padding = PaddingMode.None;

                using (MemoryStream ms1 = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms1, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Write(cipherBytess, 0, cipherBytess.Length);
                            cs.FlushFinalBlock();

                        }
                
                    }

                    cipherText.UserName = Encoding.Unicode.GetString(ms1.ToArray());
                    cipherText.Password = Encoding.Unicode.GetString(cipherBytess.ToArray());
                }
            }

            return Ok(cipherText);
        }

        [HttpGet]
        public IActionResult Getuser()
        {
          var allUser  =  _context.PasswordManagers.ToList();
            return Ok (allUser);

        }



    }
}
