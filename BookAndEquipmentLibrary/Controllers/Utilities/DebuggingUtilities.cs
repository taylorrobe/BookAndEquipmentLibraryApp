using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BookAndEquipmentLibrary.Controllers.Utilities
{
    public static class DebuggingUtilities
    {
        public static string GetExceptionString(Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Exception: ");
            stringBuilder.AppendLine(ex.Message.ToString());
            Exception innerException = ex.InnerException;

            while (innerException.InnerException != null)
            {
                innerException = innerException.InnerException;
                stringBuilder.Append("Inner exception: ");
                stringBuilder.AppendLine(innerException.Message.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}