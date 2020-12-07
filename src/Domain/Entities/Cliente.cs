using Domain.Entities.Interfaces;
using System;
namespace Domain.Entities
{
    public class Cliente:IEntity
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
    }
}
