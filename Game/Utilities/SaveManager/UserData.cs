using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace WaffleStock
{
    public class UserData
    {
        #region serializable instance
        private static UserData _instance = null;
        public Dictionary<string, long> _statistics { get; set; }
        public Dictionary<string, DateTime> _achievements { get; set; }

        public UserData()
        {
            if (_instance != null)
                return;
            _statistics = new Dictionary<string, long>();
            _achievements = new Dictionary<string, DateTime>();

            _instance = this;
        }
        #endregion

        public static Dictionary<string, DateTime> Achievements { get { return _instance._achievements; } set { _instance._achievements = value; } }

        public static void Reset()
        {
            _instance = null;
            new UserData();
        }

        public static string Serialize()
        {
            return JsonSerializer.Serialize(_instance);
        }

        public static bool Deserialize(string json)
        {
            try
            {
                _instance = JsonSerializer.Deserialize<UserData>(json);
                PropertyInfo[] properties = typeof(UserData).GetProperties();

                // This makes sure that we recover from corrupted data where any property is set to `null`
                // by replacing it with a new empty object.
                foreach (PropertyInfo property in properties)
                {
                    if (typeof(UserData).GetProperty(property.Name).GetValue(_instance) == null)
                    {
                        typeof(UserData).GetProperty(property.Name).SetValue(_instance, property.PropertyType.GetConstructor(new Type[] { }).Invoke(new object[] { }));
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr(e);
                return false;
            }
        }

        #region helper methods
        public static long GetStatistic(string statName)
        {
            return _instance._statistics.GetValueOrDefault($"{statName}");
        }

        public static void SetHighStat(string statName, long stat)
        {
            long currentStat = _instance._statistics.GetValueOrDefault($"{statName}");
            if (currentStat >= stat) return;
            _instance._statistics[$"{statName}"] = stat;
        }

        public static void IncrementStatistic(string statName, long incr = 1)
        {
            long currentStat = _instance._statistics.GetValueOrDefault($"{statName}");
            _instance._statistics[$"{statName}"] = currentStat + incr;
        }

        #endregion
    }
}