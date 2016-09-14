using RabbitMQCommon.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace RabbitMQCommon.ConnectionServices
{
    /// <summary>
    /// Base collection class that collects & stores property values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The method 'GetSettings' will iterate through all properties of a class that are annotated 
    /// with the 'PropertySetting' Attribute. It will then try to set the proprty with a matching 
    /// value from the config file. If one cannot be found, it will attempt to use the 'DefaultValue'
    /// specified in the attribute.    
    /// </para>
    /// <para>
    /// The 'TryValidate' method will check an 'Required' properties to make sure they have a value set.
    /// </para>
    /// </remarks>
    public abstract class PropertyBase : IEnumerable
    {
        #region Fields
        // List used to store the configuration settings for a particular object
        protected Dictionary<string, object> settings = new Dictionary<string, object>();

        #endregion

        #region Constructor
        public PropertyBase()
        {
            GetSettings();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get proerty values from config
        /// </summary>
        protected virtual void GetSettings()
        {
            /// Iterate through all properties with the 'PropertySettingAttribute' and 
            /// either assign values to them from app.config or use their default values.

            foreach (PropertyInfo prop in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var attr = Attribute.GetCustomAttribute(prop, typeof(PropertySettingAttribute));
                if (attr != null)
                {
                    string name = ((PropertySettingAttribute)attr).Name;

                    // Try to get values from configuration file
                    object configSetting = ConfigurationManager.AppSettings[name];

                    if (configSetting == null)
                    {
                        object defaultValue = ((PropertySettingAttribute)attr).DefaultValue;
                        settings.Add(name, defaultValue);
                    }
                    else
                    {
                        settings.Add(name, configSetting);
                    }
                }
            }
        }

        /// <summary>
        /// Validate settings
        /// </summary>
        /// <returns>True if valid, false otherwise.</returns>
        public virtual bool TryValidate(out string propertyName)
        {
            propertyName = "";
            bool isValid = true;

            // Iterate through all the properties with the 'PropertySettingAttribute' and validate their values if 'IsRequired' is true.
            foreach (PropertyInfo prop in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var attr = Attribute.GetCustomAttribute(prop, typeof(PropertySettingAttribute));
                if (attr != null)
                {
                    if (((PropertySettingAttribute)attr).IsRequired)
                    {
                        // Check for null value
                        var value = prop.GetValue(this, null);
                        if (value == null)
                        {
                            isValid = false;
                        }

                        // If a string, check for whitespace
                        else if (prop.PropertyType.Name == "String" && string.IsNullOrWhiteSpace(value as string))
                        {
                            isValid = false;
                        }

                        // If a char, check for whitespace
                        else if (prop.PropertyType.Name == "Char" && char.IsWhiteSpace((Char)value))
                        {
                            isValid = false;
                        }

                        // Break if invalid
                        if (!isValid)
                        {
                            propertyName = prop.Name;
                            break;
                        }
                    }
                }
            }

            return isValid;
        }
        #endregion

        #region IEnumerable
        /// <summary>
        /// Gets or sets a property setting
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <returns></returns>
        public object this[string name]
        {
            get { return settings[name]; }
            set { settings[name] = value; }
        }

        /// <summary>
        /// Property Settings Enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return settings.GetEnumerator();
        }
        #endregion
    }
}
