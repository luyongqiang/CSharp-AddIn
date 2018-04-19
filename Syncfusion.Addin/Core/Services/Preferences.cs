using System;
using System.Collections;
using System.Xml;

namespace Syncfusion.Addin.Core.Services
{
    public delegate void PreferencesChangedHandler(Preferences prefs);

    ///<summary>
    /// A class that holds the application preferences (using singletons)
    ///</summary>
    public class Preferences
    {
        private readonly Hashtable _prefs;
        private string _autoSavePath;

        private static Preferences _instance = null;
        private static Preferences _defaultPrefs = null;
        private static PreferencesProxy _proxy = null;

        ///<summary>
        /// The current preferences
        ///</summary>
        static public Preferences Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Preferences();
                return _instance;
            }
        }

        ///<summary>
        /// The default preferences
        ///</summary>
        static public Preferences Default
        {
            get
            {
                if (_defaultPrefs == null)
                    _defaultPrefs = new Preferences();
                return _defaultPrefs;
            }
        }

        static public PreferencesProxy Proxy
        {
            get
            {
                if (_proxy == null)
                    _proxy = new PreferencesProxy(Preferences.Instance);
                return _proxy;
            }
        }

        private Preferences()
        {
            _prefs = new Hashtable();
            _autoSavePath = null;
            _notifyWhenSetting = true;
        }

        public T GetValue<T>(string key)
        {
            T s = (T)_prefs[key];
            return s;
        }

        public void SetValue<T>(string key, T value)
        {
            _prefs[key] = value;
            // save the preferences if autoSavePath is set
            // ignore exceptions
            try
            {
                if (_autoSavePath != null)
                    Save(_autoSavePath);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            if (_notifyWhenSetting)
                Preferences.Proxy.Change(key, value.ToString(), "__Preferences__");
        }

        ///<summary>
        /// Get or Set the value of a preference
        ///</summary>
        public string this[string key]
        {
            get
            {
                string s = (string)_prefs[key];
                if (s == null)
                    s = string.Empty;
                return s;
            }

            set
            {
                _prefs[key] = value;
                // save the preferences if autoSavePath is set
                // ignore exceptions
                try
                {
                    if (_autoSavePath != null)
                        Save(_autoSavePath);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }

                if (_notifyWhenSetting)
                    Preferences.Proxy.Change(key, value, "__Preferences__");
            }
        }

        public string AutoSavePath
        {
            get { return _autoSavePath; }
            set { _autoSavePath = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return _prefs.GetEnumerator();
        }

        public void SetWithoutNotify(string pref, string val)
        {
            _notifyWhenSetting = false;
            this[pref] = val;
            _notifyWhenSetting = true;
        }

        ///<summary>
        /// Save preferences to an Xml file
        ///</summary>
        public void Save(string path)
        {
            using (XmlTextWriter xml = new XmlTextWriter(path, null))
            {
                xml.Indentation = 1;
                xml.IndentChar = '\t';

                xml.WriteStartDocument();
                xml.WriteStartElement(null, "preferences", null);

                foreach (DictionaryEntry entry in _prefs)
                {
                    xml.WriteStartElement(null, "pref", null);
                    xml.WriteStartAttribute(null, "name", null);
                    xml.WriteString((string)entry.Key);
                    xml.WriteEndAttribute();
                    xml.WriteString((string)entry.Value);
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();
                xml.WriteEndDocument();
            }
        }

        ///<summary>
        /// Load preferences from an Xml file
        ///</summary>
        public void Load(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList prefList = xmlDoc.GetElementsByTagName("pref");

            foreach (XmlNode prefNode in prefList)
            {
                XmlAttributeCollection attrColl = prefNode.Attributes;
                string name = attrColl["name"].Value;
                _prefs[name] = prefNode.InnerText;
            }
        }

        ///<summary>
        /// Load preferences from another Preferences instance
        ///</summary>
        public void Load(Preferences p)
        {
            if (p != null)
            {
                foreach (DictionaryEntry entry in p)
                    _prefs[entry.Key] = entry.Value;
            }
        }

        ///<summary>
        /// Display preferences
        ///</summary>
        public void Display()
        {
            foreach (DictionaryEntry entry in _prefs)
            {
                System.Console.WriteLine("[{0}]: {1}", entry.Key, entry.Value);
            }
        }

        private bool _notifyWhenSetting;
    }
} // end namespace