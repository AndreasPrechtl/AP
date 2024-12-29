using AP.Reflection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace AP.ComponentModel.Validation;

public class DataAnnotationsValidator<TTarget> : Validator<TTarget>
{
    private static volatile DataAnnotationsValidator<TTarget> _default;       

    public static new DataAnnotationsValidator<TTarget> Default
    {
        get
        {
            DataAnnotationsValidator<TTarget> d = _default;

            if (d == null)
                _default = d = new DataAnnotationsValidator<TTarget>();

            return d;
        }
    }

    private void ValidateProperty(object container, PropertyInfo property, MemberPath basePath, AP.Collections.List<ValidationMessage> messages)
    {
        Attribute[] attributes = Attribute.GetCustomAttributes(property.PropertyType, typeof(ValidationAttribute), true);
        MemberPath currentPath = new(basePath.Segments.Concat([property.Name]));
        
        if (attributes != null && attributes.Length > 0)
        {
            // get the property value from the container
            object value = property.GetValue(container);

            foreach (ValidationAttribute attribute in attributes)
            {   
                string msg = null;
                try
                {
                    msg = attribute.GetValidationResult(value, new ValidationContext(new object())).ErrorMessage;
                }
                catch (ValidationException e)
                {
                    msg = e.Message;
                }

                ValidationMessageType mt;

                // create a dummy validation context - with a dummy object, it'll be ignored anyway                        
                if (string.IsNullOrWhiteSpace(msg))
                {
                    msg = null;
                    mt = ValidationMessageType.Success;
                }
                else
                    mt = ValidationMessageType.Error;
                                    
                messages.Add(new ValidationMessage(currentPath, value, mt, msg));
            }

            // validate the sub-properties
            if (value != null)
            {
                foreach (PropertyInfo p in property.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty))
                {
                    this.ValidateProperty(value, p, currentPath, messages);
                }
            }
        }                        
    }

    public override ValidationResult<TTarget> Validate(TTarget value)
    {
        AP.Collections.List<ValidationMessage> messages = new();
        
        foreach (PropertyInfo property in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty))
        {
            // use the empty basepath - otherwise it'll create false results
            this.ValidateProperty(value, property, MemberPath.Empty, messages);
        }

        return new ValidationResult<TTarget>(value, messages);
    }
}
