﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SessionContainer
{
    public abstract class SessionContainer
    {
        private PropertyInfo[] _properties;
        private string _sessionKeyPrefix;

        private readonly ISession _session;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
        protected static readonly IReadOnlyDictionary<string, (PropertyInfo[] Properties, string SessionKeyPrefix)> InfoDictionary;

        public SessionContainer(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;

            (_properties, _sessionKeyPrefix) = InfoDictionary[GetType().Name];

            if (!IsSessionInitialized())
            {
                _session.Set(_sessionKeyPrefix, new byte[] { 1 });
                Save();

                return;
            }

            foreach (var property in _properties)
            {
                var value = GetPropertyValueFromSession(property);

                property.SetValue(this, value);
            }

        }

        static SessionContainer()
        {
            var types = typeof(SessionContainer).Assembly
                .GetReferencingAssemblies()
                .SelectMany(a => a.DefinedTypes)
                .Where(t =>
                    t.IsAssignableTo(typeof(SessionContainer)) &&
                    !t.IsAbstract &&
                    !t.IsInterface);

            var dictionary = new Dictionary<string, (PropertyInfo[] Properties, string SessionKeyPrefix)>();

            foreach(var type in types)
            {
                dictionary.Add(type.Name, ( type.GetProperties(), type.Name));
            }

            InfoDictionary = dictionary.AsReadOnly();
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

        private bool IsSessionInitialized()
        {
            var value = _session.Get(_sessionKeyPrefix);

            return value != null;
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
