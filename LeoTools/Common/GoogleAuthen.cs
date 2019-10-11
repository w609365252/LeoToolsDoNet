using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoTools.Common
{
    public class GoogleAuthen
    {
        public static bool CheckValidate(string key,string code)
        {
            string ogKey = GetOrgianalKey(key);
            var d = new Google.Authenticator.TwoFactorAuthenticator();
            return d.ValidateTwoFactorPIN(ogKey, code);
        }

        public static string GetRandomSecrect()
        {
            string key = getStr(10);
            var d = new Google.Authenticator.TwoFactorAuthenticator();
            var bytes = Encoding.Default.GetBytes(key);
            string newKey = Base32.ToString(bytes);
            return newKey;
        }

        static string  GetOrgianalKey(string key)
        {
            var s = Base32.ToBytes(key);
            return Encoding.Default.GetString(s);
        }

        static string  getStr(int n)//b：是否有复杂字符，n：生成的字符串长度
        {
            string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder SB = new StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < n; i++)
            {
                SB.Append(str.Substring(rd.Next(0, str.Length), 1));
            }
            return SB.ToString();
        }
    }

    public static class Base32
    {
        /// <summary>
        /// The to bytes.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// throws if Argument Null Exception
        /// </exception>
        public static byte[] ToBytes(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            input = input.TrimEnd('='); // remove padding characters
            var byteCount = input.Length * 5 / 8; // this must be TRUNCATED
            var returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            var arrayIndex = 0;

            foreach (var c in input)
            {
                var cValue = CharToValue(c);

                var mask = 0;
                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            // if we didn't end with a full byte
            if (arrayIndex != byteCount)
            {
                returnArray[arrayIndex] = curByte;
            }

            return returnArray;
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// throws if Argument Null Exception
        /// </exception>
        public static string ToString(byte[] input)
        {
            if (input == null || input.Length == 0)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
            var returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            var arrayIndex = 0;

            foreach (var b in input)
            {
                nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
                returnArray[arrayIndex++] = ValueToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                    returnArray[arrayIndex++] = ValueToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)((b << bitsRemaining) & 31);
            }

            // if we didn't end with a full char
            if (arrayIndex != charCount)
            {
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                while (arrayIndex != charCount)
                {
                    returnArray[arrayIndex++] = '='; // padding
                }
            }

            return new string(returnArray);
        }

        /// <summary>
        /// The char to value.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// throws if Argument Null Exception
        /// </exception>
        private static int CharToValue(char c)
        {
            var value = (int)c;

            // 65-90 == uppercase letters
            if (value < 91 && value > 64)
            {
                return value - 65;
            }

            // 50-55 == numbers 2-7
            if (value < 56 && value > 49)
            {
                return value - 24;
            }

            // 97-122 == lowercase letters
            if (value < 123 && value > 96)
            {
                return value - 97;
            }
            return 1;
        }

        /// <summary>
        /// The value to char.
        /// </summary>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The <see cref="char"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// throws if Argument Null Exception
        /// </exception>
        private static char ValueToChar(byte b)
        {
            if (b < 26)
            {
                return (char)(b + 65);
            }

            if (b < 32)
            {
                return (char)(b + 24);
            }
            return ' ';
        }
    }

}
