using System;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Checkout
{
    public class InvalidCheckoutLimitException : Exception
    {
        public InvalidCheckoutLimitException(decimal limit) : base($"Invalid limit '{limit}' - must be greater than or equal to zero!")
        { }
    }
}
