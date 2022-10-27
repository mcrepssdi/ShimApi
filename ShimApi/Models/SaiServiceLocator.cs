using System.Reflection;

namespace ShimApi.Models
{
    public class SaiServiceLocator
    {
        public string InternalServerName { get; set; } = string.Empty;
        public string InternalServicePath { get; set; } = string.Empty;
    
        public SaiServiceLocator(){}
        public SaiServiceLocator(IEnumerable<SaiWebserviceProp> rows)
        {
            PropertyInfo[] props = typeof(SaiServiceLocator).GetProperties();
            foreach(PropertyInfo prop  in props)
            {
                string value = (from s in rows where s.Property.Equals(prop.Name) select s.Value).FirstOrDefault() ?? string.Empty;
                prop.SetValue(this, value);
            }
        }

    }
}