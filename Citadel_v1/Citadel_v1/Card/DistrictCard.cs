using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public enum Color
    {
        Yellow = 0,
        Red,
        Green,
        Violet,
        Blue
    };

    public class DistrictCard : ICard
    {
        public string Name { get; private set; }
        public int Id { get; private set; }
        public int Cost { get; private set; }
        public Color Color { get; private set; }

        public DistrictCard(string name, Color color, int cost, int id)  // konstruktor dla CharacterCard
        {
            Name = name;
            Id = id;
            Cost = cost;
            Color = color;
        }
    }
}
