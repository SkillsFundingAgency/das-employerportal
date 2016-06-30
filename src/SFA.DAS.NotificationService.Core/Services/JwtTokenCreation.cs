using System;
using JWT;
using Newtonsoft.Json;
using SFA.DAS.NotificationService.Application.DataEntities;
using SFA.DAS.NotificationService.Application.Extensions;

namespace SFA.DAS.NotificationService.Application.Services
{
    public class JwtTokenCreation 
    {
        public static string CreateToken(string serviceId, string apiKey)
        {

            JsonWebToken.JsonSerializer = new NewtonsoftJsonSerializer();

            var payLoad = new GovNotifyPayload
            {
                Iss = serviceId,
                Iat = DateTime.UtcNow.ToUnixTime()
            };

            var token = JsonWebToken.Encode(payLoad, apiKey, JwtHashAlgorithm.HS256);

            return token;
        }

        internal class NewtonsoftJsonSerializer : IJsonSerializer
        {
            public string Serialize(object obj)
            {
                return JsonConvert.SerializeObject(obj);
            }

            public T Deserialize<T>(string json)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
