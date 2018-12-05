using System;
namespace MySmoothieTry2.Validations
{
    public interface IValidationRule<T>
    {
        bool Check(T Value);
    }
}
