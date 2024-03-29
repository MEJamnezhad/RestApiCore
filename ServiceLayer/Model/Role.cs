﻿using System.ComponentModel;

namespace ServiceLayer
{
    public enum Role
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("System Administrator")]
        Admin = 1,
        [Description("System Expert")]
        Expert = 2,
        [Description("Normal User")]
        User = 2,
    }
}
