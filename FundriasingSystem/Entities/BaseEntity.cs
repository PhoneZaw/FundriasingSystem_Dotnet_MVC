using FundraisingApp.Enums;
using System;

namespace FundraisingApp.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Status { get; set; } = StatusEnum.Active.ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
