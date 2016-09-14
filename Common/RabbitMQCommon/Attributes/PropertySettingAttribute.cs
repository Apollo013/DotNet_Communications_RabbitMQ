using System;

namespace RabbitMQCommon.Attributes
{
    /// <summary>
    /// Attribute used by property settings
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertySettingAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Gets or sets the application settings name/key 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// A flag that determines whether or not this setting is required
        /// </summary>
        public virtual bool IsRequired { get; set; }

        /// <summary>
        /// Specifies the default value for this setting
        /// </summary>
        public virtual object DefaultValue { get; set; }
        #endregion

        #region Constructors
        public PropertySettingAttribute()
        { }

        public PropertySettingAttribute(string name, bool isrequired = false, string defaultvalue = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Please specify the application setting name");
            }
            Name = name;
            IsRequired = isrequired;
            DefaultValue = defaultvalue;
        }
        #endregion
    }
}
