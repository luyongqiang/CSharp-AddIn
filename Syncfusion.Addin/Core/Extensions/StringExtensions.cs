using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Net.Mail;

namespace   Syncfusion.Core.Extensions
{
    /// <summary>
    /// String Extentensions
    /// </summary>
    public static class StringExtensions
    {
        #region FormatWith
        /// <summary>
		/// Formats a string with one literal placeholder.
		/// </summary>
		/// <param name="text">The extension text</param>
		/// <param name="arg0">Argument 0</param>
		/// <returns>The formatted string</returns>
        public static string FormatWith(this string text, object arg0)
        {
			return string.Format(text, arg0);
        }

		/// <summary>
		/// Formats a string with two literal placeholders.
		/// </summary>
		/// <param name="text">The extension text</param>
		/// <param name="arg0">Argument 0</param>
		/// <param name="arg1">Argument 1</param>
		/// <returns>The formatted string</returns>
        public static string FormatWith(this string text, object arg0, object arg1)
        {
			return string.Format(text, arg0, arg1);
        }

		/// <summary>
		/// Formats a string with tree literal placeholders.
		/// </summary>
		/// <param name="text">The extension text</param>
		/// <param name="arg0">Argument 0</param>
		/// <param name="arg1">Argument 1</param>
		/// <param name="arg2">Argument 2</param>
		/// <returns>The formatted string</returns>
        public static string FormatWith(this string text, object arg0, object arg1, object arg2)
        {
			return string.Format(text, arg0, arg1, arg2);
        }

		/// <summary>
		/// Formats a string with a list of literal placeholders.
		/// </summary>
		/// <param name="text">The extension text</param>
		/// <param name="args">The argument list</param>
		/// <returns>The formatted string</returns>
        public static string FormatWith(this string text, params object[] args)
        {
			return string.Format(text, args);
        }

		/// <summary>
		/// Formats a string with a list of literal placeholders.
		/// </summary>
		/// <param name="text">The extension text</param>
		/// <param name="provider">The format provider</param>
		/// <param name="args">The argument list</param>
		/// <returns>The formatted string</returns>
        public static string FormatWith(this string text, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, text, args);
        }
        #endregion

        #region XmlSerialize XmlDeserialize
        /// <summary>Serialises an object of type T in to an xml string</summary>
		/// <typeparam name="T">Any class type</typeparam>
		/// <param name="objectToSerialise">Object to serialise</param>
		/// <returns>A string that represents Xml, empty oterwise</returns>
		public static string XmlSerialize<T>(this T objectToSerialise) where T : class
		{
			var serialiser = new XmlSerializer(typeof(T));
			string xml;
			using (var memStream = new MemoryStream())
			{
				using (var xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8))
				{
					serialiser.Serialize(xmlWriter, objectToSerialise);
					xml = Encoding.UTF8.GetString(memStream.GetBuffer());
				}
			}
            
            // ascii 60 = '<' and ascii 62 = '>'
			xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
			xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1)); 
			return xml;
		}

		/// <summary>Deserialises an xml string in to an object of Type T</summary>
		/// <typeparam name="T">Any class type</typeparam>
		/// <param name="xml">Xml as string to deserialise from</param>
		/// <returns>A new object of type T is successful, null if failed</returns>
		public static T XmlDeserialize<T>(this string xml) where T : class
		{
			var serialiser = new XmlSerializer(typeof(T));
			T newObject;

			using (var stringReader = new StringReader(xml))
			{
				using (var xmlReader = new XmlTextReader(stringReader))
				{
					try
					{
						newObject = serialiser.Deserialize(xmlReader) as T;
					}
					catch (InvalidOperationException) // String passed is not Xml, return null
					{
						return null;
					}

				}
			}

			return newObject;
        }
        #endregion

        #region To X conversions
        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string value)
        {
            return ToEnum<T>(value, false);
        }

        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string value,bool ignorecase)
        {
            if (value == null)
                throw new ArgumentNullException("Value");
            
            value = value.Trim();

            if (value.Length == 0)
                throw new ArgumentNullException("Must specify valid information for parsing in the string.", "value");

            Type t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");

            return (T)Enum.Parse(t, value, ignorecase);
        }
        
        /// <summary>
        /// Toes the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static int ToInteger(this string value, int defaultvalue)
        {
            return (int)ToDouble(value, defaultvalue);
        }
        /// <summary>
        /// Toes the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int ToInteger(this string value)
        {
            return ToInteger(value, 0);
        }

        ///// <summary>
        ///// Toes the U long.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <returns></returns>
        //public static ulong ToULong(this string value)
        //{
        //    ulong def = 0;
        //    return value.ToULong(def);
        //}
        ///// <summary>
        ///// Toes the U long.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <param name="defaultvalue">The defaultvalue.</param>
        ///// <returns></returns>
        //public static ulong ToULong(this string value, ulong defaultvalue)
        //{
        //    return (ulong)ToDouble(value, defaultvalue);
        //}

        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static double ToDouble(this string value, double defaultvalue)
        {
            double result;
            if (double.TryParse(value, out result))
            {
                return result;
            } else return defaultvalue;
        }
        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double ToDouble(this string value)
        {
            return ToDouble(value, 0);
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value, DateTime? defaultvalue)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result))
            {
                return result;
            }
            else return defaultvalue;
        }
        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value)
        {
            return ToDateTime(value, null);
        }

        /// <summary>
        /// Converts a string value to bool value, supports "T" and "F" conversions.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>A bool based on the string value</returns>
        public static bool? ToBoolean(this string value)
        {
            if (string.Compare("T",value,true) == 0)
            {
                return true;
            }
            if (string.Compare("F", value, true) == 0)
            {
                return false;
            }
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            else return null;
        }
        #endregion

        #region ValueOrDefault
        public static string GetValueOrEmpty(this string value)
        {
            return GetValueOrDefault(value, string.Empty);
        }
        public static string GetValueOrDefault(this string value, string defaultvalue)
        {
            if (value != null) return value;
            return defaultvalue;
        }
        #endregion

        #region ToUpperLowerNameVariant
        /// <summary>
        /// Converts string to a Name-Format where each first letter is Uppercase.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns></returns>
        public static string ToUpperLowerNameVariant(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            char[] valuearray = value.ToLower().ToCharArray();
            bool nextupper = true;
            for (int i = 0; i < (valuearray.Count() -1); i++)
            {
                if (nextupper)
                {
                    valuearray[i] = char.Parse(valuearray[i].ToString().ToUpper());
                    nextupper = false;
                } 
                else
                {
                    switch (valuearray[i])
	                {
                        case ' ':
                        case '-':
                        case '.':
                        case ':':
                        case '\n':
                            nextupper = true;
                            break;
		                default:
                            nextupper = false;
                            break;
	                }
                }
            }
            return new string(valuearray);
        }
        #endregion

        #region Encrypt Decrypt
        /// <summary>
        /// Encryptes a string using the supplied key. Encoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="key">Encryptionkey.</param>
        /// <returns>A string representing a byte array separated by a minus sign.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
        public static string Encrypt(this string stringToEncrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
            }

            System.Security.Cryptography.CspParameters cspp = new System.Security.Cryptography.CspParameters();
            cspp.KeyContainerName = key;

            System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(cspp);
            rsa.PersistKeyInCsp = true;

            byte[] bytes = rsa.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(stringToEncrypt), true);

            return BitConverter.ToString(bytes);
        }

        /// <summary>
        /// Decryptes a string using the supplied key. Decoding is done using RSA encryption.
        /// </summary>
        /// <param name="key">Decryptionkey.</param>
        /// <returns>The decrypted string or null if decryption failed.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
        public static string Decrypt(this string stringToDecrypt, string key)
        {
            string result = null;

            if (string.IsNullOrEmpty(stringToDecrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
            }

            try
            {
                System.Security.Cryptography.CspParameters cspp = new System.Security.Cryptography.CspParameters();
                cspp.KeyContainerName = key;

                System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(cspp);
                rsa.PersistKeyInCsp = true;

                string[] decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
                byte[] decryptByteArray = Array.ConvertAll<string, byte>(decryptArray, (s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber))));


                byte[] bytes = rsa.Decrypt(decryptByteArray, true);

                result = System.Text.UTF8Encoding.UTF8.GetString(bytes);

            }
            finally
            {
                // no need for further processing
            }

            return result;
        }
        #endregion

        #region IsValidUrl
        /// <summary>
        /// Determines whether it is a valid URL.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid URL] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidUrl(this string text)
        {
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(text);
        }
        #endregion

        #region IsValidEmailAddress
        /// <summary>
        /// Determines whether it is a valid email address
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid email address] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(this string email)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email);
        }
        #endregion

        #region Email
        /// <summary>
        /// Send an email using the supplied string.
        /// </summary>
        /// <param name="body">String that will be used i the body of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="sender">The email address from which the message was sent.</param>
        /// <param name="recipient">The receiver of the email.</param> 
        /// <param name="server">The server from which the email will be sent.</param>  
        /// <returns>A boolean value indicating the success of the email send.</returns>
        public static bool Email(this string body, string subject, string sender, string recipient, string server)
        {
            try
            {
                // To
                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(recipient);

                // From
                MailAddress mailAddress = new MailAddress(sender);
                mailMsg.From = mailAddress;

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.Body = body;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(server);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not send mail from: " + sender + " to: " + recipient + " thru smtp server: " + server + "\n\n" + ex.Message, ex);
            }

            return true;
        }
        #endregion

        #region Truncate
        /// <summary>
        /// Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <returns>truncated string</returns>
        public static string Truncate(this string text, int maxLength)
        {
            // replaces the truncated string to a ...
            const string suffix = "...";
            string truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            int strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }
        #endregion


        #region Format
        /// <summary>
        /// Replaces the format item in a specified System.String with the text equivalent
        /// of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="arg">The arg.</param>
        /// <param name="additionalArgs">The additional args.</param>
        public static string Format(this string format, object arg, params object[] additionalArgs)
        {
            if (additionalArgs == null || additionalArgs.Length == 0)
            {
                return string.Format(format, arg);
            }
            else
            {
                return string.Format(format, new object[] { arg }.Concat(additionalArgs).ToArray());
            }
        }
        #endregion

        #region IsNullOrEmpty
        /// <summary>
        /// Determines whether [is not null or empty] [the specified input].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is not null or empty] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNullOrEmpty(this string input)
        {
            return !String.IsNullOrEmpty(input);
        }
        #endregion

        public static bool MatchesWildcard(this string text, string pattern)
        {
            int it = 0;
            while (text.CharAt(it) != 0 &&
                   pattern.CharAt(it) != '*')
            {
                if (pattern.CharAt(it) != text.CharAt(it) && pattern.CharAt(it) != '?')
                    return false;
                it++;
            }

            int cp = 0;
            int mp = 0;
            int ip = it;

            while (text.CharAt(it) != 0)
            {
                if (pattern.CharAt(ip) == '*')
                {
                    if (pattern.CharAt(++ip) == 0)
                        return true;
                    mp = ip;
                    cp = it + 1;
                }
                else if (pattern.CharAt(ip) == text.CharAt(it) || pattern.CharAt(ip) == '?')
                {
                    ip++;
                    it++;
                }
                else
                {
                    ip = mp;
                    it = cp++;
                }
            }

            while (pattern.CharAt(ip) == '*')
            {
                ip++;
            }
            return pattern.CharAt(ip) == 0;
        }

        public static char CharAt(this string s, int index)
        {
            if (index < s.Length)
                return s[index];
            return '\0';
        }
        
    }
}
