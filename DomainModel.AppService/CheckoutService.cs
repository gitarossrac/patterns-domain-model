﻿using Dawn;
using DomainModel.Domain.Checkout;
using DomainModel.Domain.Products;

namespace DomainModel.AppService
{
    public class CheckoutService
    {
        private readonly IResolver _resolver;
        private OutChecker _outChecker;

        public CheckoutService(IResolver resolver)
        {
            _resolver = resolver;
        }

        public void Start()
        {
            var repository = _resolver.Resolve<IProductRepository>();
            _outChecker = new OutChecker(repository);
            _outChecker.Start();
        }

        public void Scan(string code)
        {
            Guard.Operation(_outChecker != null);
            var barCode = new BarCode(code);
            _outChecker.Scan(barCode);
        }

        public void Close()
        {
            Guard.Operation(_outChecker != null);
            _outChecker.Close();
        }

        public string GetCurrentBill()
        {
            Guard.Operation(_outChecker != null);
            return _outChecker.ShowBill().PrintableText;
        }
    }
}