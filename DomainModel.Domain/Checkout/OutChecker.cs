﻿using Dawn;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Checkout
{
    /// <summary>
    /// Domain service doing the checkout process.
    /// </summary>
    /// <remarks>
    /// Hopefully, my English isn't so bad...
    /// See also: https://www.grammarphobia.com/blog/2019/05/checkout.html
    /// </remarks>
    public sealed class OutChecker
    {
        private readonly IProductRepository _repository;
        private ProcessState _state = ProcessState.NotStartedYet;
        private Bill _bill = Bill.NoBill;

        public OutChecker(IProductRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Starts the checkout process.
        /// </summary>
        public void Start()
        {
            Guard.Operation(_state == ProcessState.NotStartedYet, $"You cannot start a checkout process when {_state}");
            _bill = Bill.EmptyBill;
            _state = ProcessState.InProgress;
        }

        public void Scan(BarCode barCode)
        {
            Guard.Operation(_state == ProcessState.InProgress, $"You mustn't scan a bought product when checkout process {_state}");
            var product = FindProductBy(barCode);
            _bill = _bill.Add(product);
        }

        public Bill ShowBill() => _bill;

        public void Close()
        {
            Guard.Operation(_state == ProcessState.InProgress, $"You cannot close the checkout process when {_state}");
            _state = ProcessState.Closed;
        }

        private Product FindProductBy(BarCode barCode)
        {
            var product = _repository.FindBy(barCode);
            Guard.Operation(product != null, "Repository error: null reference received");
            return product;
        }
    }
}