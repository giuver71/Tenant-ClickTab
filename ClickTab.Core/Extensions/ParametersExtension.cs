using Microsoft.Data.SqlClient;
using System;

namespace ClickTab.Core.Extensions
{
    public static class ParametersExtension
    {
        public static SqlParameter AddWithValueNullable(this SqlParameterCollection Parameters, string parameterName, object value)
        {



            if (value != null )
            {
                return Parameters.AddWithValue(parameterName, value);
            }
            else
            {
                return Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }
    }
}
