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
                    new DistrictCard(1, "Town Hall", Color.Green, 5, 0, 0),
                    new DistrictCard(2, "Town Hall", Color.Green, 5, 1, 0),
                    new DistrictCard(3, "University", Color.Violet, 6, 2, 0),
                    new DistrictCard(4, "Dragon Gate", Color.Violet, 6, 3, 0),
                    new DistrictCard(5, "Watchtower", Color.Red, 1, 4, 0),
                    new DistrictCard(6, "Watchtower", Color.Red, 1, 5, 0),
                    new DistrictCard(7, "Watchtower", Color.Red, 1, 6, 0),
                    new DistrictCard(8, "Armory", Color.Violet, 3, 7, 0),

                    new DistrictCard(9, "Keep", Color.Violet, 3, 0, 1),
                    new DistrictCard(10, "Keep", Color.Violet, 3, 1, 1),
                    new DistrictCard(11, "Haunted City", Color.Violet, 2, 2, 1),
                    new DistrictCard(12, "Magic School", Color.Violet, 6, 3, 1),
                    new DistrictCard(13, "Prison", Color.Red, 2, 4, 1),
                    new DistrictCard(14, "Prison", Color.Red, 2, 5, 1),
                    new DistrictCard(15, "Prison", Color.Red, 2, 6, 1),
                    new DistrictCard(16, "Ball Room", Color.Violet, 6, 7, 1),

                    new DistrictCard(17, "Trading Post", Color.Green, 2, 0, 2),
                    new DistrictCard(18, "Trading Post", Color.Green, 2, 1, 2),
                    new DistrictCard(19, "Trading Post", Color.Green, 2, 2, 2),
                    new DistrictCard(20, "Great Wall", Color.Violet, 6, 3, 2),
                    new DistrictCard(21, "Battlefield", Color.Red, 3, 4, 2),
                    new DistrictCard(22, "Battlefield", Color.Red, 3, 5, 2),
                    new DistrictCard(23, "Battlefield", Color.Red, 3, 6, 2),
                    new DistrictCard(24, "Bell Tower", Color.Red, 5, 7, 2),

                    new DistrictCard(25, "Docks", Color.Green, 3, 0, 3),
                    new DistrictCard(26, "Docks", Color.Green, 3, 1, 3),
                    new DistrictCard(27, "Docks", Color.Green, 3, 2, 3),
                    new DistrictCard(28, "Graveyard", Color.Violet, 5, 3, 3),
                    new DistrictCard(29, "Temple", Color.Blue, 1, 4, 3),
                    new DistrictCard(30, "Temple", Color.Blue, 1, 5, 3),
                    new DistrictCard(31, "Temple", Color.Blue, 1, 6, 3),
                    new DistrictCard(32, "Factory", Color.Violet, 6, 7, 3),

                    new DistrictCard(33, "Harbor", Color.Green, 4, 0, 4),
                    new DistrictCard(34, "Harbor", Color.Green, 4, 1, 4),
                    new DistrictCard(35, "Harbor", Color.Green, 4, 2, 4),
                    new DistrictCard(36, "Labratory", Color.Violet, 5, 3, 4),
                    new DistrictCard(37, "Church", Color.Blue, 2, 4, 4),
                    new DistrictCard(38, "Church", Color.Blue, 2, 5, 4),
                    new DistrictCard(39, "Church", Color.Blue, 2, 6, 4),
                    new DistrictCard(40, "Hospital", Color.Violet, 6, 7, 4),

                    new DistrictCard(41, "Palace", Color.Yellow, 5, 0, 5),
                    new DistrictCard(42, "Palace", Color.Yellow, 5, 1, 5),
                    new DistrictCard(43, "Palace", Color.Yellow, 5, 2, 5),
                    new DistrictCard(44, "Smithy", Color.Violet, 5, 3, 5),
                    new DistrictCard(45, "Monastery", Color.Blue, 3, 4, 5),
                    new DistrictCard(46, "Monastery", Color.Blue, 3, 5, 5),
                    new DistrictCard(47, "Monastery", Color.Blue, 3, 6, 5),
                    new DistrictCard(48, "Treasury", Color.Violet, 4, 7, 5),

                    new DistrictCard(49, "Market", Color.Green, 2, 0, 6),
                    new DistrictCard(50, "Market", Color.Green, 2, 1, 6),
                    new DistrictCard(51, "Market", Color.Green, 2, 2, 6),
                    new DistrictCard(52, "Market", Color.Green, 2, 3, 6),
                    new DistrictCard(53, "Library", Color.Violet, 6, 4, 6),
                    new DistrictCard(54, "Fortress", Color.Red, 5, 5, 6),
                    new DistrictCard(55, "Fortress", Color.Red, 5, 6, 6),
                    new DistrictCard(56, "Lighthouse", Color.Violet, 3, 7, 6),

                    new DistrictCard(57, "Castle", Color.Yellow, 4, 0, 7),
                    new DistrictCard(58, "Castle", Color.Yellow, 4, 1, 7),
                    new DistrictCard(59, "Castle", Color.Yellow, 4, 2, 7),
                    new DistrictCard(60, "Castle", Color.Yellow, 4, 3, 7),
                    new DistrictCard(61, "Observatory", Color.Violet, 5, 4, 7),
                    new DistrictCard(62, "Cathedral", Color.Blue, 5, 5, 7),
                    new DistrictCard(63, "Cathedral", Color.Blue, 5, 6, 7),
                    new DistrictCard(64, "Map Room", Color.Violet, 5, 7, 7),

                    new DistrictCard(65, "Tavern", Color.Green, 1, 0, 8),
                    new DistrictCard(66, "Tavern", Color.Green, 1, 1, 8),
                    new DistrictCard(67, "Tavern", Color.Green, 1, 2, 8),
                    new DistrictCard(68, "Tavern", Color.Green, 1, 3, 8),
                    new DistrictCard(69, "Tavern", Color.Green, 1, 4, 8),
                    new DistrictCard(70, "Park", Color.Violet, 6, 5, 8),
                    new DistrictCard(71, "Museum", Color.Violet, 4, 6, 8),
                    new DistrictCard(72, "Poor House", Color.Violet, 5, 7, 8),

                    new DistrictCard(73, "Manor", Color.Yellow, 3, 0, 9),
                    new DistrictCard(74, "Manor", Color.Yellow, 3, 1, 9),
                    new DistrictCard(75, "Manor", Color.Yellow, 3, 2, 9),
                    new DistrictCard(76, "Manor", Color.Yellow, 3, 3, 9),
                    new DistrictCard(77, "Manor", Color.Yellow, 3, 4, 9),
                    new DistrictCard(78, "Wishing Well", Color.Violet, 5, 5, 9),
                    new DistrictCard(79, "Throne Room", Color.Violet, 6, 6, 9),
                    new DistrictCard(80, "Quarry", Color.Violet, 5, 7, 9)
                }
            );
        }
    }
}