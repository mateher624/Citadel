﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    class ResultPhase : Phase
    {
        private const int WinningDistrictAmount = 1;
        private const int MaxWinningPlayerAmount = 2;
        private const int FirstPlayerId = 1;
        public bool IsEnd { get; protected set; }             // określa, czy rozgrywka powinna się już zakończyć
        private readonly Player[] _winningPlayers = new Player[2];

        public ResultPhase(List<Player> players, Phase phase, Decks deck, List<CharacterCard> fullCharacterCardList, SynchronizationController synchronizationController, IUserAdapter userAdapter) : this(players, phase.Round, deck, fullCharacterCardList, synchronizationController, userAdapter)
        {

        }

        public ResultPhase(List<Player> players, Round round, Decks deck, List<CharacterCard> fullCharacterCardList, SynchronizationController synchronizationController, IUserAdapter userAdapter) : base(players, deck, fullCharacterCardList, synchronizationController, userAdapter)
        {
            Round = round;
            IsEnd = false;
        }

        public override void DoPhase()
        {
            Player winner = null;
            foreach (var player in Players)
            {
                if (player.Table.Count >= WinningDistrictAmount) IsEnd = true;
                winner = player;
                break;
            }
            /*if (IsWinningConditionSatisfied())
            {
                int playerId = FindFirstPlayerInThisRound();
                FindWinningPlayers(_winningPlayers, playerId);
                // uwzględnienie dodatkowych pkt za zwycięstwo
                CountPlayersPoints();
                
                IsEnd = true;
            }*/
            if(!IsEnd)
            {
                SetNewKingPlayer();
                UpdatePhase();
            }
            else
            {

                _userAdapter.EndGame(winner);
                // koniec rozgrywki, podsumowanie
            }
            ReturnCardsToOriginalDecks();
        }

        private void SetNewKingPlayer()     // ustawienie znacznika króla dla gracza, który w tej turze miał kartę króla
        {
            for (int i = 0; i < Players.Count(); i++)
            {
                Players[i].IsKing = Players[i].CharacterCard.Name == "King";
            }
        }

        public override void UpdatePhase()
        {
            Round.CurrentPhase = new CharacterChoicePhase(Players, this, Deck, FullCharacterCardList, SynchronizationController, _userAdapter);
        }

        private void CountPlayersPoints()    // podliczenie pkt na koniec rundy
        {
            foreach (var player in Players)
            {
                player.CountPoints();
            }
        }

        private void ReturnCardsToOriginalDecks()
        {
            ReturnFromDiscardedDeck(Deck.DiscardedCharacterDeck);
            ReturnFromDiscardedDeck(Deck.DiscardedDistrictDeck);
            ReturnFromPlayers();
            _userAdapter.ResetPanels();
            _userAdapter.UpdatePanels(Players);
        }

        private void ReturnFromDiscardedDeck<T>(List<T> list) where T: ICard
        {

            foreach (var card in list)
            {
                var cardToReturn = card;
                //list.Remove(cardToReturn);
                var discardedDistrictCard = cardToReturn as DistrictCard;
                if(discardedDistrictCard != null)
                {
                    Deck.DistrictDeck.Add(discardedDistrictCard);
                    continue;
                }

                var discardedCharacterCard = cardToReturn as CharacterCard;
                if (discardedCharacterCard != null)
                {
                    Deck.CharacterDeck.Add(discardedCharacterCard);
                }
            }
            list.RemoveRange(0, list.Count());
        } 

        private void ReturnFromPlayers()
        {
            foreach (var player in Players)
            {
                player.ReturnCards();
            }
        }

        private int FindFirstPlayerInThisRound()    // odnalezienie gracza, który rozpoczynał aktualną rundę
        {
            int playerId = FirstPlayerId;
            foreach (var player in Players)
            {
                if (player.IsKing)
                {
                    playerId = player.PlayerId;
                }
            }
            return playerId;
        }

        private void FindWinningPlayers(Player[] winningPlayers, int playerId)  // odnalezienie gracza/graczy(max dwóch) spełniających warunek zwycięstwa
        {
            int winningPlayersCounter = 0, listIndex = playerId, arrayIndex = 0;
            while(listIndex != Players.Count() || winningPlayersCounter != MaxWinningPlayerAmount)  // przeszukanie pierwszej części listy graczy
            {
                if(Players[listIndex].Table.Count() == WinningDistrictAmount)
                {
                    AcknowledgeWinningPlayer(winningPlayers, ref arrayIndex, ref listIndex, ref winningPlayersCounter);
                }
            }
            if(winningPlayersCounter != MaxWinningPlayerAmount && playerId != FirstPlayerId)
            {
                listIndex = 0;
                while(listIndex != playerId || winningPlayersCounter != MaxWinningPlayerAmount)
                {
                    AcknowledgeWinningPlayer(winningPlayers, ref arrayIndex, ref listIndex, ref winningPlayersCounter);
                }
            }
        }

        // przyjęcie nowego gracza, jako wygrywającego
        private void AcknowledgeWinningPlayer(Player[] winningPlayers, ref int arrayIndex, ref int listIndex, ref int winningPlayersCounter)
        {
            winningPlayers[arrayIndex] = Players[listIndex];
            arrayIndex++;
            winningPlayersCounter++;
        }

        private bool IsWinningConditionSatisfied()  // sprawdzenie, czy któryś z graczy spełnił warunek zwycięstwa (ukończone miasto)
        {
            foreach (var player in Players)
            {
                if(player.Table.Count() == WinningDistrictAmount)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
