using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    //class AssassinPlayerActionArgs : CharacterActionArgs
    //{
    //    public int cardToEliminateId { get; set; }

    //}

    class AssassinPlayerAction : PlayerAction
    {
        protected override void DoCharacterAction(List<Player> players, Player currentPlayer)
        {
            //var assassinPlayerActionArgs = args as AssassinPlayerActionArgs;
            //if(assassinPlayerActionArgs == null)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(args));
            //}
            
            ////kod do testowania
            //int cardToEliminateId;
            //Console.WriteLine("Eliminate character of id (1, 2, 3, 4, 5, 6)");
            //ConsoleKeyInfo keyPressed = Console.ReadKey();
            //cardToEliminateId = (int)keyPressed.Key;
            ////koniec kodu testowego

            List<CharacterCard> availableCards = FullCharacterCardList.ToList();
            availableCards.RemoveAll(x => x.Id == 1);   // usunięcie z listy karty Assassin
            var cardToEliminate = UserAdapter.ChooseCharacterCard(availableCards, 1);  
            for(int i = 0; i < players.Count; i++)
            {
                if(players[i].CharacterCard.Id == cardToEliminate.Id)     // jeżeli któryś z graczy posiada tę kartę
                {
                    players[i].CharacterCard.Active = false;        // to ten gracz traci obecną turę
                    break;
                }
            }
            if (UserAdapter.DecideToBuildDistrict())
            {
                BuildDistricts(currentPlayer);
                UserAdapter.UpdateCurrentPanel(currentPlayer);
            }
        }

        public AssassinPlayerAction(IUserAdapter userAdapter, Decks deck, List<CharacterCard> fullCharacterCardList) : base(userAdapter, deck, fullCharacterCardList)
        {
        }
    }
}
