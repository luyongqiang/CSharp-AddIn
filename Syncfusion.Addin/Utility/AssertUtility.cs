using System;
using System.Globalization;

namespace Syncfusion.Addin.Utility
{
    /// <summary>
    /// Class AssertUtility. This class cannot be inherited.
    /// </summary>
    public sealed class AssertUtility
    {
        private AssertUtility()
        {
        }

        /// <summary>
        /// Determines whether the specified value is true.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void IsTrue(bool value)
        {
            AssertUtility.IsTrue(value, string.Empty);
        }

        /// <summary>
        /// Determines whether the specified value is true.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="msg">The MSG.</param>
        /// <exception cref="System.Exception"></exception>
        public static void IsTrue(bool value, string msg)
        {
            if (!value)
            {
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// Enums the defined.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void EnumDefined(Type enumType, object enumValue, string name)
        {
            if (!Enum.IsDefined(enumType, enumValue))
            {
                throw new ArgumentException(name, string.Format(CultureInfo.InvariantCulture, "Argument '{0}' is not defined in the enumeration type '{1}'.", new object[]
                {
                    name,
                    enumType.FullName
                }));
            }
        }

        /// <summary>
        /// Arguments the not null.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void ArgumentNotNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, "Argument '{0}' cannot be null.", new object[]
                {
                    name
                }));
            }
        }

        /// <summary>
        /// Nots the null.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void NotNull(object member, string name)
        {
            if (member == null)
            {
                throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, "Member '{0}' cannot be null.", new object[]
                {
                    name
                }));
            }
        }

        /// <summary>
        /// Nots the null.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <exception cref="System.Exception">Invalid value.</exception>
        public static void NotNull(object member)
        {
            if (member == null)
            {
                throw new Exception("Invalid value.");
            }
        }

        /// <summary>
        /// Arguments the not null.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void ArgumentNotNull(object argument, string name, string message)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name, message);
            }
        }

        /// <summary>
        /// Arguments the has text.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void ArgumentHasText(string argument, string name)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, "Argument '{0}' cannot be null or resolve to an empty string : '{1}'.", new object[]
                {
                    name,
                    argument
                }));
            }
        }

        /// <summary>
        /// Arguments the has text.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void ArgumentHasText(string argument, string name, string message)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(name, message);
            }
        }

        /// <summary>
        /// Arguments the assignable from.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="BaseType">Type of the base.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void ArgumentAssignableFrom(object argument, Type BaseType, string name)
        {
            if (!argument.GetType().IsAssignableFrom(BaseType))
            {
                throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, "Argument '{0}' cannot assignable from {1}.", new object[]
                {
                    name,
                    BaseType.FullName
                }));
            }
        }

        /// <summary>
        /// Arguments the assignable from.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="BaseType">Type of the base.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void ArgumentAssignableFrom(object argument, Type BaseType, string name, string message)
        {
            if (!argument.GetType().IsAssignableFrom(BaseType))
            {
                throw new ArgumentNullException(name, message);
            }
        }

        /// <summary>
        /// Arguments the type of the same.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="argumentType">Type of the argument.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void ArgumentSameType(object argument, Type argumentType, string name)
        {
            if (!argument.GetType().Equals(argumentType))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Type of argument '{0}' must be {1}.", new object[]
                {
                    name,
                    argumentType.FullName
                }), name);
            }
        }

        /// <summary>
        /// Arguments the type of the same.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="argumentType">Type of the argument.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void ArgumentSameType(object argument, Type argumentType, string name, string message)
        {
            if (!argument.GetType().Equals(argumentType))
            {
                throw new ArgumentException(message, name);
            }
        }
    }
}