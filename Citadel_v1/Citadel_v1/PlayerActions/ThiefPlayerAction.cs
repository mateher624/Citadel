using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    class ThiefPlayerAction : PlayerAction
    {
        protected override void DoCharacterAction(List<Player> players, Player currentPlayer)
        {
            List<CharacterCard> available = ListWithNoAssassinAndThiefCharacter(FullCharacterCardList.ToList());
            var cardToSteelFrom = UserAdapter.ChooseCharacterCard(available);   //mogą być wszystkie oprócz karty Assassin iThief
            if (Deck.DiscardedCharacterDeck.Any(card => cardToSteelFrom.Id == card.Id))
            {
                return;     //jeżeli wybrana karta jest odrzucona to nie ma żadnego efektu
            }
            foreach (var player in players)
            {
                if (player.CharacterCard.Id == cardToSteelFrom.Id)
                {
                    currentPlayer.Gold += player.TakeAwayGold();
                }
            }
            if (UserAdapter.DecideToBuildDistrict())
            {
                BuildDistricts(currentPlayer);
            }
        }

        private List<CharacterCard> ListWithNoAssassinAndThiefCharacter(List<CharacterCard> deckCharacterDeck)
        {
            deckCharacterDeck.RemoveAll(x => x.Id == 1 || x.Id == 2);    // usunięcie kart Assassin i Thief z listy (Id=1)
            return deckCharacterDeck;
        }

        public ThiefPlayerAction(IUserAdapter userAdapter, Decks deck, List<CharacterCard> fullCharacterCardList) : base(userAdapter, deck, fullCharacterCardList)
        {
        }
    }
}
