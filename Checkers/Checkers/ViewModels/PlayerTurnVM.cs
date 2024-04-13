using Checkers.Models;
using Checkers.Services;
using Checkers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.ViewModels
{
    public class PlayerTurnVM : BaseNotification
    {
        private GameLogic gameLogic;
        private PlayerTurn playerTurn;

        public PlayerTurnVM(GameLogic gameLogic, PlayerTurn playerTurn)
        {
            this.gameLogic = gameLogic;
            this.playerTurn = gameLogic.Turn;
        }

        public PlayerTurn PlayerIcon
        {
            get
            {
                return playerTurn;
            }
            set
            {
                playerTurn = value;
                OnPropertyChanged("PlayerIcon");
            }
        }
    }
}