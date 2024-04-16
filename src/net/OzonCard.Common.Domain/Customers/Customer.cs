﻿using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Domain.Customers;

public class Customer : AggregateRoot
{
    private readonly ICollection<CustomerWallet> _wallets = new List<CustomerWallet>();
    private readonly ICollection<Card> _cards = new List<Card>();
    public string Name { get; private set; }
    public string? Phone { get; private set; }
    public string? TabNumber { get; private set; }
    public string? Position { get; private set; }
    public string? Division { get; private set; }
    public bool IsActive { get; private set; }
    public string? Comment { get; private set; }
    public Guid BizId { get; private set; }

    public IEnumerable<Card> Cards => _cards;
    public IEnumerable<CustomerWallet> Wallets => _wallets;

    public Customer(
        string name, 
        Guid bizId,
        bool isActive = true,
        string? phone = null, 
        string? tabNumber  = null, 
        string? position = null,
        string? division = null, 
        string? comment = null)
    {
        Name = name;
        Phone = phone;
        TabNumber = tabNumber;
        Position = position;
        Division = division;
        IsActive = isActive;
        Comment = comment;
        BizId = bizId;
    }
}