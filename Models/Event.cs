﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CallRecording.Models;

[Index("Id", IsUnique = true)]
public partial class Event
{
    [Column("user")]
    public long User { get; set; }

    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("added_time")]
    public string AddedTime { get; set; } = null!;

    [Column("sent_time")]
    public string SentTime { get; set; } = null!;

    [Column("event_type")]
    public string EventType { get; set; } = null!;

    [Column("key")]
    public string Key { get; set; } = null!;

    [Column("value")]
    public string Value { get; set; } = null!;

    [ForeignKey("User")]
    [InverseProperty("Events")]
    public virtual User UserNavigation { get; set; } = null!;
}
