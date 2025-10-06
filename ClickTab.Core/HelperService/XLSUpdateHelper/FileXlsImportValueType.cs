using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.HelperService.XLSUpdateHelper
{
    public static class FileXlsImportValueType
    {
        public static Dictionary<string, Type> Providers;

        static FileXlsImportValueType()
        {
            Providers = new Dictionary<string, Type>();
            Load();
        }

        private static IEnumerable<Type> GetTypesImplements(Type type)
        {
            IEnumerable<Type> types=AppDomain.CurrentDomain.GetAssemblies().SelectMany(s=>s.GetTypes()).Where(p=>type.IsAssignableFrom(p));
            return types;
        }

        private static void Load()
        {
           IEnumerable<Type> types=GetTypesImplements(typeof(IXlsUpdate));
            foreach (var type in types)
            {
                FileXlsImportAttribute[] attributes = (FileXlsImportAttribute[])type.GetCustomAttributes(typeof(FileXlsImportAttribute),true);
                foreach (FileXlsImportAttribute attribute in attributes)
                {
                    if (Providers.ContainsKey(attribute.Filename))
                    {
                        throw new Exception("Develope Error: Chiave file duplicata nel dizionario  FileXlsImportValueType");
                    }
                    Providers.Add(attribute.Filename, type);
                }
            }
        }

    }
}
