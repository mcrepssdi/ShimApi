using System.Security.Cryptography;
using SdiUtility;
using ShimApi.DataProviders;
using ShimApi.Models.ResponseModels;
using ShimApi.ShimConfigMgr;

namespace ShimApi.Utilities
{
    public class SupplierShipmentRefNo
    {
        private readonly IMySqlMillProvider _mysqlProvider;
        private readonly int MaxAttempts;
        private readonly string _integrationConnectionStr;
        private readonly string _saiSupportonnectionStr;
    
        public SupplierShipmentRefNo(IShimConfigurations config, IMySqlMillProvider mysqlProvider)
        {
            MaxAttempts = config.Environment.MaxAttemptsForControlNo;
            _integrationConnectionStr = config.SaiConfigurations.MillLocations.IntegrationConnectionStr;
            _saiSupportonnectionStr = config.SaiConfigurations.MillLocations.ConnectionStr;
            _mysqlProvider = mysqlProvider;
        }

        public string RimasSixCharacterSuppShipNo(string dataEnvironmentName)
        {
            int attempts = 0;
            bool unique = false;
            while (!unique)
            {
                string suppShipNo = Random();
                unique = _mysqlProvider.IsUnique(suppShipNo, dataEnvironmentName);
                if (unique) return suppShipNo;
            
                attempts++;
                if (attempts > MaxAttempts) break;
            }

            return string.Empty;
        }


        private static string Random()
        {
            string key = Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..5];
            string prefix = Random(1, false);

            return $"{prefix}{key}";
        }
    
        private static string Random(int length, bool useNumber)
        {
            string alphanumericCharacters = useNumber switch
            {
                true => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz",
                _ => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
            };
        
            return GetRandomString(length, alphanumericCharacters);
        }
    
        private static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0) throw new ArgumentException("length must not be negative", nameof(length));
            if (length > int.MaxValue / 8) throw new ArgumentException("length is too big", nameof(length));
            if (characterSet == null) throw new ArgumentNullException(nameof(characterSet));
        
            char[] characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
            {
                throw new ArgumentException("characterSet must not be empty", nameof(characterSet));
            }
        
            byte[] bytes = new byte[length * 8];
            char[] result = new char[length];
            RandomNumberGenerator cryptoProvider = RNGCryptoServiceProvider.Create();
            cryptoProvider.GetBytes(bytes);
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
        
            return new string(result);
        }

        public static ResponseWrapper SetResponseOnRimasIdentifier(ResponseWrapper response, string rimasIdentifier)
        {
            if (!rimasIdentifier.IsEmpty()) return response;
        
            response.Response.Status = "ERROR";
            response.Response.Errors = new List<Error>()
            {
                new()
                {
                    Field = "RIMASIdentifier",
                    Message = "Unable to determine unique control number."
                }
            };
        
            return response;
        }
    
        public ResponseWrapper SetControllNo(ResponseWrapper response, string saiSuppShipNo, string rimasSuppShipNo, string dataEnvironmentName)
        {
            if (rimasSuppShipNo.IsEmpty()) return response;

            bool success = _mysqlProvider.AddControlNo(_integrationConnectionStr,saiSuppShipNo, rimasSuppShipNo, dataEnvironmentName);
            if (success) return response;
            response.Response.Status = "ERROR";
            response.Response.Errors = new List<Error>()
            {
                new()
                {
                    Field = "RIMASIdentifier",
                    Message = "Failed to insert record in the Omni Integrations Table."
                }
            };
        
            return response;
        }
    }
}