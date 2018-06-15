using System.Collections;

namespace Syncfusion.Addin.Core.Services
{
    public class PreferencesProxy
    {
        private Hashtable prefSubscribers;
        private Preferences prefs;
        private Hashtable currentlyHandling;

        public PreferencesProxy(Preferences prefs)
        {
            prefSubscribers = new Hashtable();
            currentlyHandling = new Hashtable();
            this.prefs = prefs;
            this._enable = true;
        }

        public void Subscribe(string pref, string id, PreferencesChangedHandler handler)
        {
            if (!prefSubscribers.Contains(pref))
                prefSubscribers[pref] = new Hashtable();

            Hashtable hashtable = prefSubscribers[pref] as Hashtable;
            if (hashtable != null) hashtable.Add(id, handler);
        }

        public void Unsubscribe(string pref, string id)
        {
            if (!prefSubscribers.Contains(pref))
                return;

            Hashtable hashtable = prefSubscribers[pref] as Hashtable;
            if (hashtable != null) hashtable.Remove(id);
        }

        public void Change(string pref, string val, string id)
        {
            if (_enable == false)
                return;

            if (currentlyHandling.Contains(pref))
                return;

            if (id != "__Preferences__")
            {
                prefs.SetWithoutNotify(pref, val);
            }

            if (!prefSubscribers.Contains(pref))
                return;

            currentlyHandling.Add(pref, null);

            Hashtable subscribers = prefSubscribers[pref] as Hashtable;
            if (subscribers != null)
                foreach (DictionaryEntry subscriber in subscribers)
                    if ((subscriber.Key as string) != id)
                    {
                        PreferencesChangedHandler preferencesChangedHandler = subscriber.Value as PreferencesChangedHandler;
                        if (preferencesChangedHandler != null)
                            preferencesChangedHandler(prefs);
                    }

            currentlyHandling.Remove(pref);
        }

        public void NotifyAll()
        {
            if (_enable == false)
                return;

            foreach (DictionaryEntry prefSub in prefSubscribers)
            {
                currentlyHandling.Add(prefSub.Key, null);
                var subscribers = prefSub.Value as Hashtable;
                if (subscribers != null)
                    foreach (DictionaryEntry subscriber in subscribers)
                    {
                        var preferencesChangedHandler = subscriber.Value as PreferencesChangedHandler;
                        if (preferencesChangedHandler != null)
                            preferencesChangedHandler(prefs);
                    }
                currentlyHandling.Remove(prefSub.Key);
            }
        }

        private bool _enable;

        ///<summary>
        /// Enable or disable emission of the Changed event
        ///</summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }
    }
}