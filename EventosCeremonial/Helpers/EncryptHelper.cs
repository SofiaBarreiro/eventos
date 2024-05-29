using EventosCeremonial.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EventosCeremonial.Helpers
{
    public class EncryptHelper
    {
        /// <summary>Permite encriptar una string.</summary>
        /// <param name="text">The text.</param>
        /// <param name="keyString">The key string.
        /// Dato desde appsettings</param>
        public static string EncryptString(string text, string keyString)
        {


            text = text.Substring(0, 15);
            var key = Encoding.UTF8.GetBytes(keyString);
            LoggerManger logger = new LoggerManger();
            string convertString = "";
            try
            {
                using (var aesAlg = Aes.Create())
                {
                    using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(text);
                            }

                            var iv = aesAlg.IV;

                            aesAlg.Padding = PaddingMode.Zeros;//deja los paddings en 0, en la url no funcionan los paddings

                            var decryptedContent = msEncrypt.ToArray();

                            var result = new byte[iv.Length + decryptedContent.Length];

                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
                            convertString = Convert.ToBase64String(result).Replace('/', '_');//reemplaza los caracteres especiales que no son aceptados por el navegador

                            convertString = convertString.Replace("+", "-");
                            convertString = convertString.Remove((convertString.Length - 2));//le saca los ultimos dos caracteres que son == porque en el navegador no son aceptados

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en encriptacion", ex);

            }
            return convertString;

        }

        /// <summary>Decrypts the string.</summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="keyString">The key string.</param>
        public static string DecryptString(string cipherText, string keyString)
        {
            LoggerManger logger = new LoggerManger();

            cipherText = cipherText.Replace('_', '/');//reemp0laza los caracteres que fueron modificados en el encriptado

            cipherText = cipherText.Replace("-", "+");//reemp0laza los caracteres que fueron modificados en el encriptado

            cipherText = cipherText + "==";//agrega los ultimos dos caracteres

            string result = "";


            //Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            //Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);



            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[fullCipher.Length];
            var cipher = new byte[fullCipher.Length];

            //Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);


            Buffer.BlockCopy(fullCipher, 0, cipher, 0, iv.Length);

            var key = Encoding.UTF8.GetBytes(keyString);

            try
            {

                using (var aesAlg = Aes.Create())
                {

                    aesAlg.Padding = PaddingMode.Zeros;

                    using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                    {


                        using (var msDecrypt = new MemoryStream(cipher))
                        {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (var srDecrypt = new StreamReader(csDecrypt))
                                {
                                    result = srDecrypt.ReadToEnd();
                                    logger.LogError(result);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                //logger.LogError("DecryptString", ex);

            }

            return result;
        }

    }
}
