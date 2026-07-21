using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Kader.Infrastructure.Shared.Interfaces;
namespace Kader.Infrastructure.Shared.Implementations
{
    public class Methods : IMethods
    {
        IKeys keys;
        public Methods(){
            keys = new Keys();
        }
        public string Encrypt(string plainText){
            string Encrypted;
            byte[] clearBytes = Encoding.Unicode.GetBytes(plainText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(keys.EncryptionMasterKey(), new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    Encrypted = Convert.ToBase64String(ms.ToArray());
                }
            }
            return Encrypted;
        }
        public string Decrypt(string encryptedText){
            byte[] cipherBytes = Convert.FromBase64String(encryptedText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(keys.EncryptionMasterKey(), new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    encryptedText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return encryptedText;
        }




    //    public bool SendMail(string _from, string _to, string _subject, string _body, bool isHtml, string _userName, string _pass)
    //    {
    //        try
    //        {
    //            // create email message
    //            var email = new MimeMessage();
    //            email.From.Add(MailboxAddress.Parse(_from));
    //            email.To.Add(MailboxAddress.Parse(_to));
    //            email.Subject = _subject;
    //            if (isHtml)
    //                email.Body = new TextPart(TextFormat.Html) { Text = _body };
    //            else
    //                email.Body = new TextPart(TextFormat.Plain) { Text = _body };
    //            // send email
    //            using var smtp = new SmtpClient();
    //            smtp.Connect("mail.klnda.com", 587, SecureSocketOptions.Auto);
    //            smtp.Authenticate(_userName, _pass);
    //            smtp.Send(email);
    //            smtp.Disconnect(true);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            var msg = ex.Message;
    //            return false;
    //            throw;
    //        }
    //    }
    }
}
