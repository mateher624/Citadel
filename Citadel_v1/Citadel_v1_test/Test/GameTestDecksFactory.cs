using System;
using System.Collections.Generic;
using Citadel_v1;
using NUnit.Framework;

namespace Citadel_v1_test
{
    public class GameTestDecksFactory: IDecksFactory
    {
        public Decks Create()
        {
            return new Decks(
                new List<CharacterCard>()
                {
                    new CharacterCard("Thief", 2),
                    new CharacterCard("Magician", 3),
                    new CharacterCard("Merchant", 6),
                    new CharacterCard("Warlord", 8),
                    new CharacterCard("Architect", 7),
                    new CharacterCard("Assassin", 1),
                    new CharacterCard("Bishop", 5),
                    new CharacterCard("King", 4)
                },
                new List<DistrictCard>()
                {
                    new DistrictCard("Armory", Color.Yellow, 3, 1),
                    new DistrictCard("Factory", Color.Red, 6, 2),
                    new DistrictCard("Tavern ", Color.Green, 1, 3),
                    new DistrictCard("Throne Room ", Color.Violet, 6, 4),
                    new DistrictCard("Armory", Color.Yellow, 3, 5),
                    new DistrictCard("Factory", Color.Red, 6, 6),
                    new DistrictCard("Tavern ", Color.Green, 1, 7),
                    new DistrictCard("Throne Room ", Color.Violet, 6, 8),
                    new DistrictCard("Armory", Color.Yellow, 3, 9),
                    new DistrictCard("Factory", Color.Red, 6, 10),
                    new DistrictCard("Tavern ", Color.Green, 1, 11),
                    new DistrictCard("Throne Room ", Color.Violet, 6, 12),
                    new DistrictCard("Armory", Color.Yellow, 3, 13),
                    new DistrictCard("Factory", Color.Red, 6, 14),
                    new DistrictCard("Tavern ", Color.Green, 1, 15),
                    new DistrictCard("Throne Room ", Color.Violet, 6, 16),
                    new DistrictCard("Armory", Color.Yellow, 3, 17),
                    new DistrictCard("Factory", Color.Red, 6, 18),
                    new DistrictCard("Tavern ", Color.Green, 1, 19),
                    new DistrictCard("Throne Room ", Color.Violet, 6, 20),
                    new DistrictCard("Armory", Color.Yellow, 3, 21),
                    new DistrictCard("Factory", Color.Red, 6, 22),
                    new DistrictCard("Tavern ", Color.Green, 1, 23),
                    new DistrictCard("Throne Room ", Color.Violet, 6, 24),
                    new DistrictCard("Armory", Color.Yellow, 3, 25),
                    new DistrictCard("Factory", Color.Red, 6, 26),
                    new DistrictCard("Tavern ", Color.Green, 1, 27),
                    new DistrictCard("Throne Room ", Color.Violet, 6, 28),
                    new DistrictCard("Armory", Color.Yellow, 3, 29),
                    new DistrictCard("Factory", Color.Red, 6, 30),
                    new DistrictCard("Tavern ", Color.Green, 1, 31),
                    new DistrictCard("Throne Room ", Color.Violet, 6, 32),
                }
            );
        }
    }
}