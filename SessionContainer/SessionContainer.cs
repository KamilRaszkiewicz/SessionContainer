using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SessionContainer
{
    public abstract class SessionContainer
    {
        private readonly Type _type;
        private readonly PropertyInfo[] _properties;
        private readonly string _sessionKeyPrefix;

        private readonly ISession _session;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SessionContainer(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
            _jsonSerializerOptions = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

            _type = this.GetType();
            _properties = _type.GetProperties();
            _sessionKeyPrefix = _type.Name;

            foreach (var property in _properties)
            {
                var value = GetPropertyValueFromSession(property);

                property.SetValue(this, value);
            }

        }

        private string GetSessionKey(PropertyInfo property)
        {
            return _sessionKeyPrefix + "." + property.Name;
        }

        private object? GetPropertyValueFromSession(PropertyInfo property)
        {
            var sessionKey = GetSessionKey(property);
            var json = _session.GetString(sessionKey);

            if (string.IsNullOrEmpty(json)) return default;

            return JsonSerializer.Deserialize(json, property.PropertyType, _jsonSerializerOptions);
        }

        public void Save()
        {
            foreach (var property in _properties)
            {
                var sessionKey = GetSessionKey(property);
                var serialized = JsonSerializer.Serialize(property.GetValue(this, null), _jsonSerializerOptions);

                _session.SetString(sessionKey, serialized);
            }
        }
    }
}
