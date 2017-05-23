using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    public class Decks
    {
        public List<CharacterCard> CharacterDeck = new List<CharacterCard>();       // talia kart postaci
        public List<DistrictCard> DistrictDeck = new List<DistrictCard>();         // talia kart dzielnic 
        public List<DistrictCard> DiscardedDistrictDeck = new List<DistrictCard>();       // talia kart odrzuconych dzielnic
        public List<CharacterCard> DiscardedCharacterDeck = new List<CharacterCard>();  // talia kart odrzuconych postaci

        public Decks(List<CharacterCard> characterDeck, List<DistrictCard> districtDeck)
        {
            CharacterDeck = characterDeck;
            DistrictDeck = districtDeck;
        }

        private void Swap<T>(IList<T> list, int i, int j)    // metoda zamieniająca pozycjami dwa elementy listy
        {                                                               // potrzebna do mieszania zawartości listy
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public void Shuffle<T>(IList<T> list, Random rnd)   // metoda mieszająca zawartość listy
        {
            for (var i = 0; i < list.Count; i++)
            {
                Swap(list, i, rnd.Next(i, list.Count));
            }
        }

        //public static CharacterCard ChooseCardFromDeck(List<CharacterCard> list)       // wybór kartu przez użytkownika, zwraca wybraną kartę
        //{                                                                   // czemu tak Citadel_v1.ICard, a nie samo ICard?
        //    ConsoleKeyInfo keyPressed = Console.ReadKey();
        //    int listIndex = 0;
        //    while(keyPressed.Key != ConsoleKey.Enter)
        //    {
        //        if (keyPressed.Key == ConsoleKey.LeftArrow)
        //        {
        //            listIndex = GoLeft(list, ref listIndex);
        //        }
        //        if (keyPressed.Key == ConsoleKey.RightArrow)
        //        {
        //            listIndex = GoRight(list, ref listIndex);
        //        }
        //        keyPressed = Console.ReadKey();
        //    }
        //    return list[listIndex];
        //}

        //private static int GoLeft(List<CharacterCard> list, ref int listIndex)         // przejście o jeden element w lewo w liście
        //{
        //    if (listIndex > 0)
        //    {
        //        listIndex--;
        //    }
        //    return listIndex;
        //}

        //private static int GoRight(List<CharacterCard> list, ref int listIndex)       // przejście o jeden element w prawo w liście
        //{
        //    if (listIndex < list.Count() - 1)
        //    {
        //        listIndex++;
        //    }
        //    return listIndex;
        //}

        //public static void ShowCardOnScreen(List<CharacterCard> list)       // testowa funkcja, wypisanie nazwy wybranej karty w ekranie konsoli
        //{
        //    var card = ChooseCardFromDeck(list);
        //    Console.WriteLine(card.Name);
        //}

        public bool Found(List<CharacterCard> list, int id)
        {
            foreach (var item in list)
            {
                if (item.Id == id)
                    return true;
            }
            return false;
        }
    }
}
