﻿using OzonCard.Common.Core;
using OzonCard.Common.Domain.Files;

namespace OzonCard.Common.Application.Files.Commands;

public record SaveFileCommand(
    Guid Id,
    string FileName,
    string? User
) : ICommand<SaveFile>;
