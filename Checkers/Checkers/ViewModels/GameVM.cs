using Checkers.Models;
using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.ViewModels
{
    public class GameVM
    {
        public ObservableCollection<ObservableCollection<GameSquareVM>> Board { get; set; }
        public GameLogic Logic { get; set; }
        public ButtonInteractionVM Interactions { get; set; }

        public WinnerVM WinnerVM { get; set; }

        public PlayerTurnVM PlayerTurnVM { get; set; }

        public string RED_PIECE { get; set; }
        public string WHITE_PIECE { get; set; }

        public GameVM()
        {
            ObservableCollection<ObservableCollection<GameSquare>> board = Utility.initBoard();
            PlayerTurn playerTurn = new PlayerTurn(PieceColor.Red);
            Winner winner = new Winner(0, 0);
            Logic = new GameLogic(board, playerTurn, winner);
            PlayerTurnVM = new PlayerTurnVM(Logic, playerTurn);
            WinnerVM = new WinnerVM(Logic, winner);
            Board = CellBoardToCellVMBoard(board);
            Interactions = new ButtonInteractionVM(Logic);
            RED_PIECE = Utility.redPiece;
            WHITE_PIECE = Utility.whitePiece;
        }


        private ObservableCollection<ObservableCollection<GameSquareVM>> CellBoardToCellVMBoard(ObservableCollection<ObservableCollection<GameSquare>> board)
        {
            ObservableCollection<ObservableCollection<GameSquareVM>> result = new ObservableCollection<ObservableCollection<GameSquareVM>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<GameSquareVM> line = new ObservableCollection<GameSquareVM>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    GameSquare c = board[i][j];
                    GameSquareVM cellVM = new GameSquareVM(c, Logic);
                    line.Add(cellVM);
                }
                result.Add(line);
            }
            return result;
        }
    }
}