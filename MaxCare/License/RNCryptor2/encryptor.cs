using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace License.RNCryptor2
{
	public class Encryptor : Cryptor
	{
		private Schema defaultSchemaVersion = Schema.V2;
		public static string EncryptString(string key, string plainText)
		{
			byte[] iv = new byte[16];
			byte[] array;

			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(key);
				aes.IV = iv;

				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
						{
							streamWriter.Write(plainText);
						}

						array = memoryStream.ToArray();
					}
				}
			}

			return Convert.ToBase64String(array);
		}


		public byte[] Encrypt (byte[] plainBytes, string password)
		{
			return this.Encrypt (plainBytes, password, this.defaultSchemaVersion);
		}

		public byte[] Encrypt (byte[] plainBytes, string password, Schema schemaVersion)
		{
			this.configureSettings (schemaVersion);
            

			PayloadComponents components = new PayloadComponents();
			components.schema = new byte[] {(byte)schemaVersion};
			components.options = new byte[] {(byte)this.options};
			components.salt = this.generateRandomBytes (Cryptor.saltLength);
			components.hmacSalt = this.generateRandomBytes (Cryptor.saltLength);
			components.iv = this.generateRandomBytes (Cryptor.ivLength);

			byte[] key = this.generateKey (components.salt, password);

			switch (this.aesMode) {
				case AesMode.CTR:
					components.ciphertext = this.encryptAesCtrLittleEndianNoPadding(plainBytes, key, components.iv);
					break;
					
				case AesMode.CBC:
					components.ciphertext = this.encryptAesCbcPkcs7(plainBytes, key, components.iv);
					break;
			}

			components.hmac = this.generateHmac(components, password);

			List<byte> binaryBytes = new List<byte>();
			binaryBytes.AddRange (this.assembleHeader(components));
			binaryBytes.AddRange (components.ciphertext);
			binaryBytes.AddRange (components.hmac);

            return binaryBytes.ToArray();
		}

		private byte[] encryptAesCbcPkcs7 (byte[] plaintext, byte[] key, byte[] iv)
		{
			var aes = Aes.Create();
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			var encryptor = aes.CreateEncryptor(key, iv);

			byte[] encrypted;

			using (var ms = new MemoryStream())
			{
				using (var cs1 = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
					cs1.Write(plaintext, 0, plaintext.Length);
				}

				encrypted = ms.ToArray ();
			}

			return encrypted;
		}
		
		private byte[] generateRandomBytes (int length)
		{
			byte[] randomBytes = new byte[length];
			var rng = new RNGCryptoServiceProvider ();
			rng.GetBytes (randomBytes);

			return randomBytes;
		}
	}
}

