using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CallRecording.DAL.Models;

[Index("Id", IsUnique = true)]
[Index("Login", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("login")]
    public string Login { get; set; } = null!;

    [Column("password")]
    public string Password { get; set; } = null!;

    [Column("role")]
    public string Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Event> Events { get; } = new List<Event>();
}
