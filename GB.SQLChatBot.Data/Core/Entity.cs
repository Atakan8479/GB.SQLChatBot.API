using System;
using System.ComponentModel.DataAnnotations;
using Pars.Core.Data;
using Pars.Core.Utils;

namespace GB.SQLChatBot.Data.Core;

public abstract class Entity : Entity<int>
{
}

public class Entity<T> : IEntity<T>
{
    [Key]
    public T Id { get; set; }
    public DateTimeOffset CreateOn { get; set; } = DateTimeOffset.Now.ToDefaultZone();
    public DateTimeOffset? ChangeOn { get; set; }
    public bool IsDeleted { get; set; } = false;
    public int CreatedBy { get; set; }

}
