using System;
namespace MySmoothieTry2.Validations
{
public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
