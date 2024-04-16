﻿namespace OzonCard.Customer.Api.Models.Organizations;

public class ProgramModel
{
    public ProgramModel(Guid id, Guid name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; private set; }
    public Guid Name { get; private set; }
}