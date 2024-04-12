using OzonCard.Common.Domain.Abstractions;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Domain.Cards;

public class Card : AggregateRoot
{
    public string Track { get; private set; }
    public string Number { get; private set; }
    public Customer Customer { get; private set; }

    public Card(string track, string number, Customer customer)
    {
        Track = track;
        Number = number;
        Customer = customer;
    }
}