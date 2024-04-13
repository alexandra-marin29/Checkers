using Checkers.Commands;
using Checkers.Models;
using Checkers.Services;
using Checkers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Checkers.ViewModels
{
    public class GameSquareVM : BaseNotification
    {
        private GameLogic gameLogic;
        private GameSquare genericSquare;
        private ICommand clickPieceCommand;
        private ICommand movePieceCommand;

        public GameSquareVM(GameSquare square, GameLogic gameLogic)
        {
            genericSquare = square;
            this.gameLogic = gameLogic;
        }

        public GameSquare GenericSquare
        {
            get
            {
                return genericSquare;
            }
            set
            {
                genericSquare = value;
                OnPropertyChanged("GenericSquare");
            }
        }

        public ICommand ClickPieceCommand
        {
            get
            {
                if (clickPieceCommand == null)
                {
                    clickPieceCommand = new RelayCommand<GameSquare>(gameLogic.ClickPiece);
                }
                return clickPieceCommand;
            }
        }

        public ICommand MovePieceCommand
        {
            get
            {
                if (movePieceCommand == null)
                {
                    movePieceCommand = new RelayCommand<GameSquare>(gameLogic.MovePiece);
                }
                return movePieceCommand;
            }
        }
    }
}
