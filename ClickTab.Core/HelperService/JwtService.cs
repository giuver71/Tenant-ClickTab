using ClickTab.Core.DAL.Models.Generics;
using JWT;
using JWT.Algorithms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.HelperService
{
    public class JwtService
    {
        public string SecretKey { get; set; }

        public string PayloadToToken(Dictionary<string, object> payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new SerializerJSON();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            string token = encoder.Encode(payload, SecretKey);
            return token;
        }

        public Dictionary<string, object> TokenToPayload(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var payload = new Dictionary<string, object>();

            try
            {
                IJsonSerializer serializer = new SerializerJSON();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();

                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                payload = decoder.DecodeToObject<Dictionary<string, object>>(token, SecretKey, verify: true);
            }
            catch (Exception ex)
            {
                payload = null;
            }

            return payload;
        }

        public User PayloadToUser(Dictionary<string, object> payload)
        {
            User user = new User();
            user.ID = int.Parse(payload["ID"].ToString());
            user.Name = payload["Name"].ToString();
            user.Email = payload["Email"].ToString();

            return user;
        }
    }

    public class SerializerJSON : IJsonSerializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public object Deserialize(Type type, string json)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
