﻿using Microsoft.AspNetCore.Identity;

namespace OzonCard.Common.Infrastructure.Security;

public class UserRole : IdentityRole
{
    public const string Basic = "Basic";
    public const string Report = "Report";
    public const string Admin = "Admin";

}