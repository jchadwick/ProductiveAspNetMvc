using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPlusSports
{
    public static class TempDataExtensions
    {
        const string ErrorMessageKey = "ErrorMessage";
        const string SuccessMessageKey = "SuccessMessage";

        public static bool HasSuccessMessage(this System.Web.Mvc.TempDataDictionary tempData)
        {
            return !string.IsNullOrWhiteSpace(tempData[SuccessMessageKey] as string);
        }

        public static string SuccessMessage(this System.Web.Mvc.TempDataDictionary tempData)
        {
            return tempData[SuccessMessageKey] as string;
        }

        public static void SuccessMessage(this System.Web.Mvc.TempDataDictionary tempData, string successMessage)
        {
            tempData[SuccessMessageKey] = successMessage;
        }


        public static bool HasErrorMessage(this System.Web.Mvc.TempDataDictionary tempData)
        {
            return !string.IsNullOrWhiteSpace(tempData[ErrorMessageKey] as string);
        }

        public static string ErrorMessage(this System.Web.Mvc.TempDataDictionary tempData)
        {
            return tempData[ErrorMessageKey] as string;
        }

        public static void ErrorMessage(this System.Web.Mvc.TempDataDictionary tempData, string errorMessage)
        {
            tempData[ErrorMessageKey] = errorMessage;
        }
    }
}