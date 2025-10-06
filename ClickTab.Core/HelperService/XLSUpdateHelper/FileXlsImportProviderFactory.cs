using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.HelperService.XLSUpdateHelper
{
    public class FileXlsImportProviderFactory
    {
        public static IXlsUpdate ActivatorServiceSyncroToXls(string filename)
        {
            Type type;
            if (FileXlsImportValueType.Providers.TryGetValue(filename, out type)) {
                if (type == null) {
                    return null;
                }
                return (IXlsUpdate)Activator.CreateInstance(type);
            }
            return null;
            //throw new Exception($"File Xls {filename} non supoprttato ");
        }
    }
}
