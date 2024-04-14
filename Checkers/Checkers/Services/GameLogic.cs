using Checkers.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Documents;

namespace Checkers.Services
{
    public class GameLogic : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ObservableCollection<GameSquare>> board;
        public PlayerTurn Turn { get; set; }
        private Winner winner;
        private int _whitePiecesRemaining;
        private int _redPiecesRemaining;

        private bool allowMultipleJumps;

        private bool gameStarted;

        public bool GameStarted
        {
            get => gameStarted;
            set
            {
                if (gameStarted != value)
                {
                    gameStarted = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool AllowMultipleJumps
        {
            get { return allowMultipleJumps; }
            set
            {
                allowMultipleJumps = value;
                NotifyPropertyChanged();
            }
        }

        public int WhitePiecesRemaining
        {
            get => _whitePiecesRemaining;
            set
            {
                _whitePiecesRemaining = value;
                NotifyPropertyChanged();
            }
        }

        public int RedPiecesRemaining
        {
            get => _redPiecesRemaining;
            set
            {
                _redPiecesRemaining = value;
                NotifyPropertyChanged();
            }
        }

        public GameLogic(ObservableCollection<ObservableCollection<GameSquare>> board, PlayerTurn turn, Winner winner)
        {
            this.board = board;
            this.Turn = turn;
            this.winner = winner;
            this.winner.RedWins = Utility.getScore().RedWins;
            this.winner.WhiteWins = Utility.getScore().WhiteWins;
            WhitePiecesRemaining = 12;
            RedPiecesRemaining = 12;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    
    #region Logics
    private void SwitchTurns(GameSquare square)
        {
            Turn.PlayerColor = Turn.PlayerColor == PieceColor.Red ? PieceColor.White : PieceColor.Red;

            // This also needs to update any UI bindings or related properties
            NotifyPropertyChanged(nameof(Turn));
            if (square.Piece.Color == PieceColor.Red)
            {
                Utility.Turn.PlayerColor = PieceColor.White;
                Utility.Turn.TurnImage = Utility.whitePiece;
                Turn.PlayerColor = PieceColor.White;
                Turn.TurnImage = Utility.whitePiece;
            }
            else
            {
                Utility.Turn.PlayerColor = PieceColor.Red;
                Utility.Turn.TurnImage = Utility.redPiece;
                Turn.PlayerColor = PieceColor.Red;
                Turn.TurnImage = Utility.redPiece;
            }
        }

        private void FindNeighbours(GameSquare square)
        {
            var neighboursToCheck = new HashSet<Tuple<int, int>>();

            Utility.initializeNeighboursToBeChecked(square, neighboursToCheck);

            foreach (Tuple<int, int> neighbour in neighboursToCheck)
            {
                if (Utility.isInBounds(square.Row + neighbour.Item1, square.Column + neighbour.Item2))
                {
                    if (board[square.Row + neighbour.Item1][square.Column + neighbour.Item2].Piece == null)
                    {
                        if (!Utility.ExtraMove)
                        {
                            Utility.CurrentNeighbours.Add(board[square.Row + neighbour.Item1][square.Column + neighbour.Item2], null);
                        }
                    }
                    else if (Utility.isInBounds(square.Row + neighbour.Item1 * 2, square.Column + neighbour.Item2 * 2) &&
                        board[square.Row + neighbour.Item1][square.Column + neighbour.Item2].Piece.Color != square.Piece.Color &&
                        board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2].Piece == null)
                    {
                        Utility.CurrentNeighbours.Add(board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2], board[square.Row + neighbour.Item1][square.Column + neighbour.Item2]);
                        Utility.ExtraPath = true;
                    }
                }
            }
        }

        private void DisplayRegularMoves(GameSquare square)
        {
            if (Utility.CurrentSquare != square)
            {
                if (Utility.CurrentSquare != null)
                {
                    board[Utility.CurrentSquare.Row][Utility.CurrentSquare.Column].Texture = Utility.redSquare;

                    foreach (GameSquare selectedSquare in Utility.CurrentNeighbours.Keys)
                    {
                        selectedSquare.LegalSquareSymbol = null;
                    }
                    Utility.CurrentNeighbours.Clear();
                }

                FindNeighbours(square);

                if (Utility.ExtraMove && !Utility.ExtraPath)
                {
                    Utility.ExtraMove = false;
                    SwitchTurns(square);
                }
                else
                {

                    foreach (GameSquare neighbour in Utility.CurrentNeighbours.Keys)
                    {
                        board[neighbour.Row][neighbour.Column].LegalSquareSymbol = Utility.hintSquare;
                    }

                    Utility.CurrentSquare = square;
                    Utility.ExtraPath = false;
                }
            }
            else
            {
                board[square.Row][square.Column].Texture = Utility.redSquare;

                foreach (GameSquare selectedSquare in Utility.CurrentNeighbours.Keys)
                {
                    selectedSquare.LegalSquareSymbol = null;
                }
                Utility.CurrentNeighbours.Clear();
                Utility.CurrentSquare = null;
            }
        }
        public void UpdateTurn(PlayerTurn newTurn)
        {
            Turn = newTurn;
            NotifyPropertyChanged(nameof(Turn));  // This notifies the UI that the Turn property has changed.
        }

        #endregion
        #region ClickCommands

        public void ResetGame()
        {
            GameStarted = false; // Indicates that a new game has started
            AllowMultipleJumps = false; // Reset the "Allow Multiple Jumps" option

            // Reset game board and piece counts
            Utility.ResetGame(board, this);

            // Notify UI about the change in the 'AllowMultipleJumps' property
            NotifyPropertyChanged(nameof(AllowMultipleJumps));
        }

        public void SaveGame()
        {
            Utility.SaveGame(board,this);
        }

        public void LoadGame()
        {
            Utility.LoadGame(board,this);
            if (Turn.PlayerColor == PieceColor.Red)
            {
                Turn.TurnImage = Utility.redPiece;
            }
            if(Turn.PlayerColor == PieceColor.White)
            {
                Turn.TurnImage = Utility.whitePiece;
            }
            CheckForWin();
        }

        public void About()
        {
            Utility.About();
        }
        public void ClickPiece(GameSquare square)
        {
            if ((Utility.Turn.PlayerColor == PieceColor.Red && square.Piece.Color == PieceColor.Red ||
                Utility.Turn.PlayerColor == PieceColor.White && square.Piece.Color == PieceColor.White) &&
                !Utility.ExtraMove)
            {
                DisplayRegularMoves(square);
            }
        }

        public void MovePiece(GameSquare square)
        {
            if (!GameStarted)
            {
                GameStarted = true; // Set game as started when the first move is made
            }

            if (Utility.CurrentSquare == null || Utility.CurrentSquare.Piece == null)
            {
                MessageBox.Show("No piece selected to move, or the selected piece is invalid.");
                return;  // Exit the method as there is no piece to move.
            }

            // Transfer the piece to the new square
            square.Piece = Utility.CurrentSquare.Piece;
            square.Piece.Square = square;

            // Handle capturing
            if (Utility.CurrentNeighbours.ContainsKey(square) && Utility.CurrentNeighbours[square] != null)
            {
                // Remove the captured piece
                Utility.CurrentNeighbours[square].Piece = null;
                Utility.ExtraMove = true; // Indicates an extra move is possible

                // Decrement the count of the captured pieces
                if (square.Piece.Color == PieceColor.Red)
                    WhitePiecesRemaining--;  // Capturing a white piece by a red piece
                else if (square.Piece.Color == PieceColor.White)
                    RedPiecesRemaining--;  // Capturing a red piece by a white piece
            }
            else
            {
                Utility.ExtraMove = false;
                SwitchTurns(Utility.CurrentSquare);
            }

            // Clear board visuals
            board[Utility.CurrentSquare.Row][Utility.CurrentSquare.Column].Texture = Utility.redSquare;
            foreach (GameSquare selectedSquare in Utility.CurrentNeighbours.Keys)
            {
                selectedSquare.LegalSquareSymbol = null;
            }

            // Cleanup after move
            Utility.CurrentNeighbours.Clear();
            Utility.CurrentSquare.Piece = null;
            Utility.CurrentSquare = null;

            // Promote to King if applicable
            if (square.Piece.Type == PieceType.Regular)
            {
                if ((square.Piece.Color == PieceColor.Red && square.Row == 0) ||
                    (square.Piece.Color == PieceColor.White && square.Row == board.Count - 1))
                {
                    square.Piece.Type = PieceType.King;
                    square.Piece.Texture = square.Piece.Color == PieceColor.Red ? Utility.redKingPiece : Utility.whiteKingPiece;
                }
            }

            // Continue extra moves or check for game over
            if (Utility.ExtraMove && this.AllowMultipleJumps) // Only continue if multiple jumps are allowed
            {
                if (Turn.TurnImage == Utility.redPiece)
                {
                    Utility.CollectedWhitePieces++;
                }
                else if (Turn.TurnImage == Utility.whitePiece)
                {
                    Utility.CollectedRedPieces++;
                }
                DisplayRegularMoves(square);
            }

                if (Utility.CollectedRedPieces == 12 || Utility.CollectedWhitePieces == 12)
            {
                GameOver();
            }
        }

        public void ResetPieceCounts()
        {
            RedPiecesRemaining = 12;
            WhitePiecesRemaining = 12;
        }

        public void CheckForWin()
        {
            if (WhitePiecesRemaining == 0 || Utility.CollectedRedPieces == 12)
            {
                GameOver();
            }
            else if (RedPiecesRemaining == 0 || Utility.CollectedWhitePieces == 12)
            {
                GameOver();
            }
        }



        public void GameOver()
        {
            Winner stats = Utility.getScore(); // Get current scores and max pieces left
            if (Utility.CollectedRedPieces == 12)
            {
                stats.WhiteWins++; // White wins
                int piecesLeft = WhitePiecesRemaining; // Number of pieces left for white
                if (piecesLeft > stats.MaxWhitePiecesLeft)
                {
                    stats.MaxWhitePiecesLeft = piecesLeft;
                }
                MessageBox.Show("Game Over! The Winner is Player White!");
            }
            else if (Utility.CollectedWhitePieces == 12)
            {
                stats.RedWins++; // Red wins
                int piecesLeft = RedPiecesRemaining; // Number of pieces left for red
                if (piecesLeft > stats.MaxRedPiecesLeft)
                {
                    stats.MaxRedPiecesLeft = piecesLeft;
                }
                MessageBox.Show("Game Over! The Winner is Player Red!");
            }

            Utility.writeScore(stats); // Update scores
            Utility.ResetGame(board, this);
        }


        #endregion 
    }
}