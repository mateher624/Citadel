using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public class Player
    {
        private readonly IUserAdapter _userAdapter;

        private const int MaxPlayersAmount = 6;
        public int PlayerId { get; private set; }       // identyfikator gracza
        public string Name { get; set; }            // nazwa gracza w rozgrywce
        public int Points { get; private set; }     // określa ilość pkt gracza   
        public int Gold { get; set; }               // określa ilość złota posiadanego przez gracza
        public bool IsKing { get; set; }            // określa, czy gracz aktualnie posiada znacznik króla
        public int CanBuild { get; set; } = 1;
        private readonly Decks _deck;
        protected List<CharacterCard> FullCharacterCardList { get; private set; }
        public PlayerAction PlayerAction { get; private set; }          // określa, jakie akcje ma do dyspozycji gracz
        public CharacterCard CharacterCard { get; set; }                    // karta postaci gracza w danej rundzie
        public List<DistrictCard> Hand = new List<DistrictCard>();          // karty na ręce gracza
        public List<DistrictCard> Table = new List<DistrictCard>();         // karty gracza na stole
        private readonly Dictionary<string, PlayerAction> _characterDictionary = new Dictionary<string, PlayerAction>();

        public Player(IUserAdapter userAdapter, string name, List<Player> players, Decks deck, List<CharacterCard> fullCharacterCardList)     // konstruktor dla Player
        {
            _userAdapter = userAdapter;
            Name = name;
            _deck = deck;
            Gold = 0;
            Points = 0;
            CharacterCard = null;
            IsKing = false;
            FullCharacterCardList = fullCharacterCardList;
            if (players.Count() <= MaxPlayersAmount)
            {
                PlayerId = players.Count() + 1;
            }
            FillCharacterDictionary();
        }

        private void FillCharacterDictionary()
        {
            var assassin = new AssassinPlayerAction(_userAdapter, _deck, FullCharacterCardList);
            _characterDictionary.Add("Assassin", assassin);
            var thief = new ThiefPlayerAction(_userAdapter, _deck, FullCharacterCardList);
            _characterDictionary.Add("Thief", thief);
            var magician = new MagicianPlayerAction(_userAdapter, _deck, FullCharacterCardList);
            _characterDictionary.Add("Magician", magician);
            var king = new KingPlayerAction(_userAdapter, _deck, FullCharacterCardList);
            _characterDictionary.Add("King", king);
            var bishop = new BishopPlayerAction(_userAdapter, _deck, FullCharacterCardList);
            _characterDictionary.Add("Bishop", bishop);
            var merchant = new MerchantPlayerAction(_userAdapter, _deck, FullCharacterCardList);
            _characterDictionary.Add("Merchant", merchant);
            var architect = new ArchitectPlayerAction(_userAdapter, _deck, FullCharacterCardList);
            _characterDictionary.Add("Architect", architect);
            var warlord = new WarlordPlayerAction(_userAdapter, _deck, FullCharacterCardList);
            _characterDictionary.Add("Warlord", warlord);
        }

        public void ReturnCards()
        {
            //foreach (var card in Hand)
            //{
            //    var cardToReturn = card;
            //    Hand.Remove(cardToReturn);
            //    _deck.DistrictDeck.Add(cardToReturn);
            //}
            //foreach (var card in Table)
            //{
            //    var cardToReturn = card;
            //    Table.Remove(cardToReturn);
            //    _deck.DistrictDeck.Add(cardToReturn);
            //}
            var characterCardToReturn = CharacterCard;
            CharacterCard = null;
            _deck.CharacterDeck.Add(characterCardToReturn);
        }

        public void PickCharacterCard()     // wybór karty postaci dla danego gracza
        {
            var chosenCard = _userAdapter.ChooseCharacterCard(_deck.CharacterDeck, 0, this);  //Decks.ChooseCardFromDeck(Decks.CharacterDeck);
            _deck.CharacterDeck.Remove(chosenCard);
            CharacterCard = chosenCard;
        }

        public void DoAction(List<Player> players)      // wykonuje akcję odpowiednią dla danej postaci
        {
            SetPlayerAction(FullCharacterCardList);
            PlayerAction.DoPlayerAction(this, players);
        }

        public void SetPlayerAction(List<CharacterCard> fullCharacterCardList)   // ustawia odpowiedni zestaw akcji PlayerAction na podstawie posiadanej karty postaci
        {
            PlayerAction playerAction;
            _characterDictionary.TryGetValue(CharacterCard.Name, out playerAction);
            PlayerAction = playerAction;
        }

        public void AddCardsToHand(int amount)    // dodaje karty dzielnic do ręki gracza
        {
            for (int i = 0; i < amount; i++)
            {
                var cardToAdd = _deck.DistrictDeck.First();
                _deck.DistrictDeck.Remove(cardToAdd);
                Hand.Add(cardToAdd);

                _userAdapter.DrawCardFromDeck(cardToAdd, this);
                _userAdapter.UpdateCurrentPanel(this);
            }
        }

        public void CountPoints()
        {

        }

        public int TakeAwayGold()
        {
            int goldToSteal = Gold;
            Gold = 0;
            _userAdapter.UpdateCurrentPanel(this);
            return goldToSteal;
        }

        public void ExchangeCards(Player currentPlayer)
        {
            List<DistrictCard> tmpHand = Hand;
            Hand = currentPlayer.Hand;
            currentPlayer.Hand = tmpHand;

            _userAdapter.HandsExchange(this, currentPlayer);
            _userAdapter.UpdateCurrentPanel(this);
            _userAdapter.UpdateCurrentPanel(currentPlayer);
        }

        public void DiscardAndDrawCards()
        {
            List<DistrictCard> cardsToDiscard = _userAdapter.ChooseCardsToDiscard(Hand);  //przypisuje karty do odrzucenia w liście cardsToDiscard
            int cnt = cardsToDiscard.Count();
            DiscardChosenDistrictCards(cardsToDiscard);
            DrawDistrictCards(cnt);

        }

        private void DrawDistrictCards(int cardsToDiscardAmount)
        {
            for (int i = 0; i < cardsToDiscardAmount; i++)
            {
                var districtCard = _deck.DistrictDeck.First();
                _deck.DistrictDeck.Remove(districtCard);
                Hand.Add(districtCard);

                _userAdapter.DrawCardFromDeck(districtCard, this);
                _userAdapter.UpdateCurrentPanel(this);
            }
        }

        private void DiscardChosenDistrictCards(List<DistrictCard> cardsToDiscard)
        {
            //foreach (var card in cardsToDiscard)
            //{
            //    hand.Remove(card);
            //}
            int i = 0, j = 0;
            foreach (var districtCard in cardsToDiscard)
            {
                _userAdapter.HandDiscard(districtCard, this);
                i++;
            }
            foreach (var districtCard in cardsToDiscard)
            {
                Hand.Remove(districtCard);
                j++;
            }
            if (i != j) throw new NotImplementedException();
            //Hand.RemoveAll(cardsToDiscard.Contains);

            _userAdapter.UpdateCurrentPanel(this);
        }

        public void BuildDistrict(DistrictCard districtToBuild)
        {
            Table.Add(districtToBuild);
            Hand.Remove(districtToBuild);
            CanBuild--;
            Gold -= districtToBuild.Cost;
            //_userAdapter.HandToPlayground(districtToBuild, this);
        }

        public void LooseDistrict(DistrictCard districtCard)
        {
            Table.Remove(districtCard);
            _deck.DiscardedDistrictDeck.Add(districtCard);

            _userAdapter.PlaygroundDiscard(districtCard, this);
            _userAdapter.UpdateCurrentPanel(this);
        }
    }
}
