using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml;
using System.Xml.Serialization;
using Syncfusion.Core.Utilities;

namespace Syncfusion.Core
{
    /// <summary>
    /// This object features a number of methods that are useful in dealing with objects
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Loads a named object from an assembly
        /// </summary>
        /// <param name="className">Fully qualified name of the class</param>
        /// <param name="assemblyName">Assembly name (preferrably the fully or partially qualified name, or the file name)</param>
        /// <returns>Newly instantiated object</returns>
        /// <example>SqlDataService oService = (SqlDataService)ObjectHelper.CreateObject("EPS.Data.SqlClient","SqlDataService")</example>
        public static object CreateObject(string className, string assemblyName)
        {
            Assembly assembly = null;
            if (assemblyName.ToLower(CultureInfo.InvariantCulture).EndsWith(".dll") || assemblyName.ToLower(CultureInfo.InvariantCulture).EndsWith(".exe"))
            {
                try
                {
                    assembly = Assembly.LoadFrom(assemblyName);
                }
                catch
                {
                    if (assemblyName.IndexOf("\\", StringComparison.Ordinal) < 0)
                    {
                        string location;
                        try
                        {
                            location = ((Assembly.GetEntryAssembly() != null) ? Assembly.GetEntryAssembly().Location : Assembly.GetAssembly(typeof(ObjectHelper)).Location);
                        }
                        catch
                        {
                            try
                            {
                                location = Assembly.GetAssembly(typeof(ObjectHelper)).Location;
                            }
                            catch
                            {
                                throw new ObjectInstantiationException("Unable to find assemblie's location(" + assemblyName + ")");
                            }
                        }
                        string startPath = StringHelper.AddBS(StringHelper.JustPath(location));
                        assemblyName = startPath + assemblyName;
                        try
                        {
                            assembly = Assembly.LoadFrom(assemblyName);
                        }
                        catch (Exception ex)
                        {
                            throw new ObjectInstantiationException("Unable to load assembly " + assemblyName + ". Error: " + ex.Message, ex);
                        }
                    }
                }
            }
            else if (assemblyName.IndexOf(",", StringComparison.Ordinal) > -1)
            {
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch (Exception ex)
                {
                    throw new ObjectInstantiationException("Unable to load assembly " + assemblyName + ". Error: " + ex.Message, ex);
                }
            }
            else
            {
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch (Exception ex)
                {
                    throw new ObjectInstantiationException("Unable to load assembly " + assemblyName + ". Error: " + ex.Message, ex);
                }
            }
            Type objectType;
            try
            {
                if (!(assembly != null))
                {
                    throw new ObjectInstantiationException("Unable to create instance " + className + ". Error: Unable to load assembly.");
                }
                objectType = assembly.GetType(className, true);
            }
            catch (Exception ex)
            {
                throw new ObjectInstantiationException("Unable to get type " + className + ". Error: " + ex.Message, ex);
            }
            object result;
            try
            {
                result = Activator.CreateInstance(objectType);
            }
            catch (Exception ex)
            {
                throw new ObjectInstantiationException("Unable to create instance " + className + ". Error: " + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Serializes an object to its binary state
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>Stream of binary information for the object</returns>
        /// <remarks>
        /// For this to work, the provided object must be serializable.
        ///
        /// This method can be used as an extension method.
        /// </remarks>
        /// <example>
        /// // More code...
        /// var stream = customer.SerializeToBinaryStream();
        ///
        /// // Or
        ///
        /// var stream = ObjectHelper.SerializeToBinaryStream(customer);
        /// </example>
        public static Stream SerializeToBinaryStream(this object objectToSerialize)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objectToSerialize);
            return stream;
        }

        /// <summary>
        /// Deserializes an object from its state stored in a binary stream.
        /// </summary>
        /// <param name="stateStream">The state stream.</param>
        /// <returns>Object instance.</returns>
        /// <remarks>For this to work, the stream must contain a serialized object</remarks>
        /// <example>
        /// Customer customer = (Customer)ObjectHelper.DeserializeFromBinaryStream(stream);
        /// </example>
        public static object DeserializeFromBinaryStream(Stream stateStream)
        {
            return new BinaryFormatter().Deserialize(stateStream);
        }

        /// <summary>
        /// Deserializes the stream to an object
        /// </summary>
        /// <param name="stateStream">The state stream.</param>
        /// <returns>Object instance</returns>
        /// <remarks>
        /// For this to work, the stream must contain a serialized object
        /// This is an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// Customer customer = (Customer)stream.DeserializeFromBinary();
        /// </example>
        public static object DeserializeFromBinary(this Stream stateStream)
        {
            return ObjectHelper.DeserializeFromBinaryStream(stateStream);
        }

        /// <summary>
        /// Serializes an object to its binary state
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>
        /// Byte array of binary information for the object
        /// </returns>
        /// <remarks>
        /// For this to work, the provided object must be serializable.
        /// This method can be used as an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code...
        /// byte[] serialized = customer.SerializeToBinaryArray();
        /// // or
        /// byte[] serialized = ObjectHelper.SerializeToBinaryArray(customer);
        /// </example>
        public static byte[] SerializeToBinaryArray(this object objectToSerialize)
        {
            byte[] result;
            using (Stream stream = objectToSerialize.SerializeToBinaryStream())
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                result = buffer;
            }
            return result;
        }

        /// <summary>
        /// Serializes an object to its XML state
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>
        /// XML stream representing the object's state
        /// </returns>
        /// <remarks>
        /// For this to work, the provided object must be serializable.
        /// This method can be used as an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// Stream xmlStream = customer.SerializeToXmlStream();
        /// // or
        /// Stream xmlStream = ObjectHelper.SerializeToXmlStream(customer);
        /// </example>
        public static Stream SerializeToXmlStream(this object objectToSerialize)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType());
            serializer.Serialize(stream, objectToSerialize);
            return stream;
        }

        /// <summary>
        /// Serializes an object to its XML state
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>
        /// XML string representing the object's state
        /// </returns>
        /// <remarks>
        /// For this to work, the provided object must be serializable.
        /// This method can be used as an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// string xml = customer.SerializeToXmlString();
        /// // or
        /// string xml = ObjectHelper.SerializeToXmlString(customer);
        /// </example>
        public static string SerializeToXmlString(this object objectToSerialize)
        {
            return StreamHelper.ToString(objectToSerialize.SerializeToXmlStream());
        }

        /// <summary>
        /// Serializes an object to its XML state
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>
        /// XML document representing the object's state
        /// </returns>
        /// <remarks>
        /// For this to work, the provided object must be serializable.
        /// This method can be used as an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// XmlDocument xml = customer.SerializeToXmlDocument();
        /// // or
        /// XmlDocument xml = ObjectHelper.SerializeToXmlDocument(customer);
        /// </example>
        public static XmlDocument SerializeToXmlDocument(object objectToSerialize)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(objectToSerialize.SerializeToXmlString());
            return document;
        }

        /// <summary>
        /// Deserializes an object from its state stored in an xml stream.
        /// </summary>
        /// <param name="stateStream">The state stream.</param>
        /// <param name="expectedType">The expected type (which will be the returned type).</param>
        /// <returns>Object instance.</returns>
        /// <remarks>
        /// For this to work, the XML Stream must contain a seralized object
        /// </remarks>
        /// <example>
        /// Customer customer = (Customer)ObjectHelper.DeserializeFromXmlStream(stream, typeof(Customer));
        /// </example>
        public static object DeserializeFromXmlStream(Stream stateStream, Type expectedType)
        {
            XmlSerializer serializer = new XmlSerializer(expectedType);
            return serializer.Deserialize(stateStream);
        }

        /// <summary>
        /// Deserializes an object from its state stored in an xml stream.
        /// </summary>
        /// <param name="stateStream">The state stream.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <returns>Object instance</returns>
        /// <remarks>
        /// For this to work, the XML Stream must contain a seralized object
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// Customer customer = (Customer)stream.DeserializeFromXmlStream(typeof(Customer));
        /// </example>
        public static object DeserializeFromXml(this Stream stateStream, Type expectedType)
        {
            return ObjectHelper.DeserializeFromXmlStream(stateStream, expectedType);
        }

        /// <summary>
        /// Serializes an object to its SOAP representation
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>
        /// XML stream representing the object's state
        /// </returns>
        /// <remarks>
        /// For this to work, the provided object must be serializable.
        /// This method can be used as an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// Stream stream = customer.SerializeToSoapStream();
        /// // or
        /// Stream stream = ObjectHelper.SerializeToSoapStream(customer);
        /// </example>
        public static Stream SerializeToSoapStream(this object objectToSerialize)
        {
            MemoryStream stream = new MemoryStream();
            SoapFormatter formatter = new SoapFormatter();
            formatter.Serialize(stream, objectToSerialize);
            return stream;
        }

        /// <summary>
        /// Serializes an object to its SOAP state
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>
        /// XML string representing the object's state
        /// </returns>
        /// <remarks>
        /// For this to work, the provided object must be serializable.
        /// This method can be used as an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// string state = customer.SerializeToSoapString();
        /// // or
        /// string state = ObjectHelper.SerializeToSoapString(customer);
        /// </example>
        public static string SerializeToSoapString(this object objectToSerialize)
        {
            return StreamHelper.ToString(objectToSerialize.SerializeToSoapStream());
        }

        /// <summary>
        /// Serializes an object to its SOAP state
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>
        /// XML document representing the object's state
        /// </returns>
        /// <remarks>
        /// For this to work, the provided object must be serializable.
        /// This method can be used as an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// XmlDocument xml = customer.SerializeToSoapDocument();
        /// // or
        /// XmlDocument xml =ObjectHelper.SerializeToSoapDocument(customer);
        /// </example>
        public static XmlDocument SerializeToSoapDocument(object objectToSerialize)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(objectToSerialize.SerializeToSoapString());
            return document;
        }

        /// <summary>
        /// Compares the values of two objects and returns true if the values are different
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>True if values DIFFER</returns>
        /// <example>
        /// object o1 = "Hello";
        /// object o2 = "World";
        /// object o3 = 25;
        /// ObjectHelper.ValuesDiffer(o1, o2); // returns true;
        /// ObjectHelper.ValuesDiffer(o1, o3); // returns true;
        /// </example>
        /// <remarks>
        /// This method has been created to be easily able to compare objects of unknown types.
        /// In particular, this is useful when comparing two fields in a DataSet.
        /// This method can even handle byte arrays.
        /// </remarks>
        public static bool ValuesDiffer(object value1, object value2)
        {
            if (value1 == null)
            {
                throw new ArgumentNullException("value1");
            }
            if (value2 == null)
            {
throw new ArgumentNullException("value2");            }
            bool fieldsDiffer = false;
            IComparable comparableValue = value1 as IComparable;
            IComparable comparableValue2 = value2 as IComparable;
            bool result;
            if (comparableValue != null && comparableValue2 != null)
            {
                result = !comparableValue.Equals(comparableValue2);
            }
            else
            {
                byte[] array = value1 as byte[];
                byte[] array2 = value2 as byte[];
                if (array != null && array2 != null)
                {
                    if (array.Length != array2.Length)
                    {
                        result = true;
                    }
                    else
                    {
                        result = array.Where((byte t, int arrayCounter) => t != array2[arrayCounter]).Any<byte>();
                    }
                }
                else
                {
                    try
                    {
                        bool v1IsNull = value1 is DBNull;
                        bool v2IsNull = value2 is DBNull;
                        if ((v1IsNull && !v2IsNull) || (!v1IsNull && v2IsNull))
                        {
                            result = true;
                            return result;
                        }
                    }
                    catch (InvalidCastException)
                    {
                        fieldsDiffer = true;
                    }
                    result = fieldsDiffer;
                }
            }
            return result;
        }

        /// <summary>Dynamically retrieves a property value from the specified object</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="valueObject">The value object.</param>
        /// <param name="path">Name of the property.</param>
        /// <returns>Property value or default value</returns>
        /// <remarks>
        /// The property must be a readable instance property.
        /// This method can be called as an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// var customer = this.GetCustomerObject();
        /// string name = customer.GetPropertyValue&lt;string&gt;("LastName");
        /// </example>
        public static TResult GetPropertyValue<TResult>(this object valueObject, string path)
        {
            TResult result;
            if (!path.Contains(".") && !path.Contains("["))
            {
                result = ObjectHelper.GetSimplePropertyValue<TResult>(valueObject, path);
            }
            else
            {
                try
                {
                    object parentObject;
                    PropertyInfo property = ObjectHelper.GetPropertyByPath(valueObject, path, out parentObject);
                    if (property == null)
                    {
                        result = default(TResult);
                    }
                    else
                    {
                        object propertyValue = property.GetValue(parentObject, null);
                        result = (TResult)((object)propertyValue);
                    }
                }
                catch
                {
                    result = default(TResult);
                }
            }
            return result;
        }

        private static TResult GetSimplePropertyValue<TResult>(object valueObject, string propertyName)
        {
            TResult result;
            try
            {
                Type type = valueObject.GetType();
                PropertyInfo propertyInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (propertyInfo != null)
                {
                    result = (TResult)((object)propertyInfo.GetValue(valueObject, null));
                }
                else
                {
                    result = default(TResult);
                }
            }
            catch
            {
                result = default(TResult);
            }
            return result;
        }

        /// <summary>
        /// Returns property information based on the provided path (path can be a simple property name or a more complex path)
        /// </summary>
        /// <param name="valueObject">The value object.</param>
        /// <param name="path">The path.</param>
        /// <param name="parentObject">The parent object.</param>
        /// <returns>PropertyInfo.</returns>
        public static PropertyInfo GetPropertyByPath(object valueObject, string path, out object parentObject)
        {
            string[] parts = path.Split(new char[]
			{
				'.'
			});
            parentObject = valueObject;
            PropertyInfo result;
            for (int propertyCounter = 0; propertyCounter < parts.Length; propertyCounter++)
            {
                string part = parts[propertyCounter];
                Type valueObjectType = valueObject.GetType();
                if (!part.Contains("["))
                {
                    PropertyInfo propertyInfo = valueObjectType.GetProperty(part, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);
                    if (propertyInfo == null)
                    {
                        result = null;
                        return result;
                    }
                    if (propertyCounter == parts.Length - 1)
                    {
                        result = propertyInfo;
                        return result;
                    }
                    valueObject = propertyInfo.GetValue(valueObject, null);
                    parentObject = valueObject;
                }
                else
                {
                    string partName = part.Substring(0, part.IndexOf("[", StringComparison.Ordinal));
                    string indexExpression = part.Substring(part.IndexOf("[", StringComparison.Ordinal) + 1);
                    indexExpression = indexExpression.Substring(0, indexExpression.IndexOf("]", StringComparison.Ordinal));
                    int index;
                    if (!int.TryParse(indexExpression, out index))
                    {
                        result = null;
                        return result;
                    }
                    PropertyInfo propertyInfo = valueObjectType.GetProperty(partName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);
                    if (propertyInfo == null)
                    {
                        result = null;
                        return result;
                    }
                    valueObject = propertyInfo.GetValue(valueObject, null);
                    if (valueObject == null)
                    {
                        result = null;
                        return result;
                    }
                    valueObjectType = valueObject.GetType();
                    List<PropertyInfo> indexerProperties = (from p in valueObjectType.GetProperties()
                                                            where p.Name == "Item"
                                                            select p).ToList<PropertyInfo>();
                    PropertyInfo indexerPropertyInfo = null;
                    if (indexerProperties.Count < 1)
                    {
                        result = null;
                        return result;
                    }
                    if (indexerProperties.Count == 1)
                    {
                        indexerPropertyInfo = indexerProperties[0];
                    }
                    else
                    {
                        foreach (PropertyInfo indexerProperty in indexerProperties)
                        {
                            ParameterInfo[] indexerParameters = indexerProperty.GetIndexParameters();
                            if (indexerParameters.Length == 1 && !(indexerParameters[0].ParameterType != typeof(int)))
                            {
                                indexerPropertyInfo = indexerProperty;
                                break;
                            }
                        }
                    }
                    if (indexerPropertyInfo == null)
                    {
                        result = null;
                        return result;
                    }
                    if (propertyCounter == parts.Length - 1)
                    {
                        result = indexerPropertyInfo;
                        return result;
                    }
                    valueObject = indexerPropertyInfo.GetValue(valueObject, new object[]
					{
						index
					});
                    parentObject = valueObject;
                }
            }
            result = null;
            return result;
        }

        /// <summary>
        /// Dynamically retrieves a property value from the specified object
        /// </summary>
        /// <typeparam name="TValue">The type of the value that is to be set.</typeparam>
        /// <param name="valueObject">The value object.</param>
        /// <param name="path">Name of the property.</param>
        /// <param name="value">The value that is to be set.</param>
        /// <returns>True if the value was set successfully</returns>
        /// <remarks>
        /// The property must be a writable instance property.
        /// This method can be called as an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// var customer = this.GetCustomerObject();
        /// customer.SetPropertyValue("LastName", "Smith");
        /// </example>
        public static bool SetPropertyValue<TValue>(this object valueObject, string path, TValue value)
        {
            bool result;
            if (!path.Contains(".") && !path.Contains("["))
            {
                result = valueObject.SetSimplePropertyValue(path, value);
            }
            else
            {
                try
                {
                    object parentObject;
                    PropertyInfo property = ObjectHelper.GetPropertyByPath(valueObject, path, out parentObject);
                    if (property == null)
                    {
                        result = false;
                    }
                    else
                    {
                        property.SetValue(parentObject, value, null);
                        result = true;
                    }
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        private static bool SetSimplePropertyValue<TValue>(this object valueObject, string propertyName, TValue value)
        {
            bool result;
            try
            {
                Type type = valueObject.GetType();
                PropertyInfo propertyInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (propertyInfo == null)
                {
                    result = false;
                }
                else
                {
                    propertyInfo.SetValue(valueObject, value, null);
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Dynamically invokes the specified method on the defined object
        /// </summary>
        /// <typeparam name="TResult">The expected return type for the method</typeparam>
        /// <param name="valueObject">The value object (object that contains the method).</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The method's return value</returns>
        /// <remarks>
        /// The method must be an instance method
        /// This method can be called as an extension method.
        /// </remarks>
        /// <example>
        /// using Syncfusion.Addin.Core;
        /// // more code
        /// var customer = this.GetCustomerObject();
        /// object[] parameters = { "John", "M.", "Smith" };
        /// string fullName = customer.InvokeMethod&lt;string&gt;("GetFullName", parameters);
        /// </example>
        public static TResult InvokeMethod<TResult>(this object valueObject, string methodName, object[] parameters)
        {
            TResult result;
            try
            {
                Type type = valueObject.GetType();
                MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (methodInfo != null)
                {
                    result = (TResult)((object)methodInfo.Invoke(valueObject, parameters));
                }
                else
                {
                    result = default(TResult);
                }
            }
            catch
            {
                result = default(TResult);
            }
            return result;
        }
    }
}
