﻿using System;
using System.Collections.Generic;

namespace WebCakeTools.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;
}
