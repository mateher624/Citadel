using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citadel_v1;

namespace Citadel_v1_test
{
    [TestFixture]
    public class GameTest
    {
        [Test]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
        public void ShouldGameBeDone()
        {
            IUserAdapter userAdapter = new GameTestUserAdapter();
            IDecksFactory decksFactory = new GameTestDecksFactory();

            const int playersAmount = 6;
            SynchronizationController synchronizationController = new SynchronizationController();

            Game game = new Game(playersAmount, userAdapter, decksFactory.Create(), synchronizationController);
            game.StartNewRound();
        }
    }
}
