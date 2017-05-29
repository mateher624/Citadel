using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citadel_v1;

namespace Citadel_v1_test.Test
{
    class GameTestDecksFactoryFromDb : IDecksFactory
    {
        public Decks Create()
        {
            Decks deck = null;
            using (var context = new CardDbContext())
            {
                deck = new Decks(
                    context.CharacterCards.ToList(),
                    context.DistrictCards.ToList());
            }
            if (deck == null)
            {
                throw new Exception("Database has not been read properly!");
            }
            return deck;

        }
    }
}
