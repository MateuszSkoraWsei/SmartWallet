using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmartWallet.Attributes
{
    public class MustBeTrueAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object value)
        {
            if (value is bool b) return b;
            return false;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) return;
            MergeAttribute(context.Attributes, "data-val", "true");
            var error = ErrorMessage ?? "Musisz zaakceptować politykę prywatności";
            MergeAttribute(context.Attributes, "data-val-mustbetrue", error);
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key)) return false;
            attributes.Add(key, value);
            return true;
        }
    }
}
