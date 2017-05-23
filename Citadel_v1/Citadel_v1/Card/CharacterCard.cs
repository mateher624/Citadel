using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public class CharacterCard : ICard
    {
        public string Name { get; private set; }
        public int Id { get; private set; }
        public bool Active { get; set; }    // określa, czy karta jest aktywna w danej rundzie

        public CharacterCard(string name, int id)  // konstruktor dla CharacterCard
        {
            Active = true;
            Name = name;
            Id = id;
        }

    }
}
