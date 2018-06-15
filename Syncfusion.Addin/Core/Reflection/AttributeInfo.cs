using System;

namespace Syncfusion.Addin.Core.Reflection
{
    /// <summary>
    /// Class AttributeInfo.
    /// </summary>
    [Serializable]
    public class AttributeInfo
    {
        /// <summary>
        /// The _owner
        /// </summary>
        private Type _owner;

        public Type Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        /// <summary>
        /// The _value
        /// </summary>
        private Attribute _value;

        public Attribute Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeInfo"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="attribute">The attribute.</param>
        public AttributeInfo(Type type, Attribute attribute)
        {
            this._owner = type;
            this._value = attribute;
        }
    }
}