using System;
using System.Collections.Generic;

namespace WebCakeTools.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string? BlogName { get; set; }

    public string? BlogDescribe { get; set; }

    public byte[]? BlogImage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
