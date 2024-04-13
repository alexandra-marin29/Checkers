using Checkers.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Checkers.Services
{
    public class Utility
    {
        #region constValues
        //const image paths
        public const string redSquare = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\squareRed.png";
        public const string whiteSquare = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\squareWhite.png";
        public const string redPiece = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\pieceRed.png";
        public const string whitePiece = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\pieceWhite.png";
        public const string hintSquare = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\squareHint.png";
        public const string redKingPiece = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\pieceRedKing.png";
        public const string whiteKingPiece = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\pieceWhiteKing.png";
        public const string SQUARE_HIGHLIGHT = "NULL";
        //const values for serialization
        public const char NO_PIECE = 'N';
        public const char WHITE_PIECE = 'W';
        public const char RED_PIECE = 'R';
        public const char RED_KING = 'K';
        public const char WHITE_KING = 'L';
        public const char WHITE_TURN = '2';
        public const char RED_TURN = '1';
        public const char HAD_COMBO = 'H';
        public const char EXTRA_PATH = 'E';
        //board constants
        public const int boardSize = 8;
        #endregion
        #region staticValues
        public static GameSquare CurrentSquare { get; set; }
        private static Dictionary<GameSquare, GameSquare> currentNeighbours = new Dictionary<GameSquare, GameSquare>();
        private static PlayerTurn turn = new PlayerTurn(PieceColor.Red);
        private static bool extraMove = false;
        private static bool extraPath = false;
        private static int collectedRedPieces = 0;
        private static int collectedWhitePieces = 0;

        public static GameLogic GameLogicInstance { get; set; }


        #endregion


        public static Dictionary<GameSquare, GameSquare> CurrentNeighbours
        {
            get
            {
                return currentNeighbours;
            }
            set
            {
                currentNeighbours = value;
            }
        }

        public static PlayerTurn Turn
        {
            get
            {
                return turn;
            }
            set
            {
                turn = value;
            }
        }

        public static bool ExtraMove
        {
            get
            {
                return extraMove;
            }
            set
            {
                extraMove = value;
            }
        }

        public static bool ExtraPath
        {
            get
            {
                return extraPath;
            }
            set
            {
                extraPath = value;
            }
        }

        public static int CollectedWhitePieces
        {
            get { return collectedWhitePieces; }
            set { collectedWhitePieces = value; }
        }

        public static int CollectedRedPieces
        {
            get { return collectedRedPieces; }
            set { collectedRedPieces = value; }
        }
        #region UtilityMethods
        public static ObservableCollection<ObservableCollection<GameSquare>> initBoard()
        {
            ObservableCollection<ObservableCollection<GameSquare>> board = new ObservableCollection<ObservableCollection<GameSquare>>();
            const int boardSize = 8;

            for (int row = 0; row < boardSize; ++row)
            {
                board.Add(new ObservableCollection<GameSquare>());
                for (int column = 0; column < boardSize; ++column)
                {
                    if ((row + column) % 2 == 0)
                    {
                        board[row].Add(new GameSquare(row, column, SquareShade.Light, null));
                    }
                    else if (row < 3)
                    {
                        board[row].Add(new GameSquare(row, column, SquareShade.Dark, new GamePiece(PieceColor.White)));
                    }
                    else if (row > 4)
                    {
                        board[row].Add(new GameSquare(row, column, SquareShade.Dark, new GamePiece(PieceColor.Red)));
                    }
                    else
                    {
                        board[row].Add(new GameSquare(row, column, SquareShade.Dark, null));
                    }
                }
            }

            return board;
        }

        public static void ResetGameBoard(ObservableCollection<ObservableCollection<GameSquare>> squares)
        {
            for (int index1 = 0; index1 < boardSize; index1++)
            {
                for (int index2 = 0; index2 < boardSize; index2++)
                {
                    if ((index1 + index2) % 2 == 0)
                    {
                        squares[index1][index2].Piece = null;
                    }
                    else
                        if (index1 < 3)
                    {
                        squares[index1][index2].Piece = new GamePiece(PieceColor.White);
                        squares[index1][index2].Piece.Square = squares[index1][index2];
                        //pieces
                    }
                    else
                        if (index1 > 4)
                    {
                        squares[index1][index2].Piece = new GamePiece(PieceColor.Red);
                        squares[index1][index2].Piece.Square = squares[index1][index2];
                        //pieces
                    }
                    else
                    {
                        squares[index1][index2].Piece = null;
                    }
                }
            }
        }
        public static bool isInBounds(int row, int column)
        {
            return row >= 0 && column >= 0 && row < boardSize && column < boardSize;
        }

        public static void initializeNeighboursToBeChecked(GameSquare square, HashSet<Tuple<int, int>> neighboursToCheck)
        {
            if (square.Piece.Type == PieceType.King)
            {
                neighboursToCheck.Add(new Tuple<int, int>(-1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(-1, 1));
                neighboursToCheck.Add(new Tuple<int, int>(1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(1, 1));
            }
            else if (square.Piece.Color == PieceColor.Red)
            {
                neighboursToCheck.Add(new Tuple<int, int>(-1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(-1, 1));
            }
            else
            {
                neighboursToCheck.Add(new Tuple<int, int>(1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(1, 1));
            }
        }
        #endregion
        #region Serialization
        public static void LoadGame(ObservableCollection<ObservableCollection<GameSquare>> squares, GameLogic gameLogic)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == true)
            {
                using (StreamReader reader = new StreamReader(openDialog.FileName))
                {
                    string playerTurn = reader.ReadLine();
                    gameLogic.Turn = new PlayerTurn(playerTurn == "Red" ? PieceColor.Red : PieceColor.White);

                    string text;
                    gameLogic.RedPiecesRemaining = int.Parse(reader.ReadLine());
                    gameLogic.WhitePiecesRemaining = int.Parse(reader.ReadLine());


                    //current
                    if (CurrentSquare != null)
                    {
                        CurrentSquare.Texture = redSquare;
                    }
                    text = reader.ReadLine();
                    if (text != NO_PIECE.ToString())
                    {
                        CurrentSquare = squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])];
                        CurrentSquare.Texture = SQUARE_HIGHLIGHT;
                    }
                    else
                    {
                        CurrentSquare = null;
                    }

                    text = reader.ReadLine();
                    if (text != NO_PIECE.ToString())
                    {
                        ExtraMove = true;
                    }
                    else
                    {
                        ExtraMove = false;
                    }
                    text = reader.ReadLine();
                    if (text != NO_PIECE.ToString())
                    {
                        ExtraPath = true;
                    }
                    else
                    {
                        ExtraPath = false;
                    }
                    //to_do_multi_JUMP
                    text = reader.ReadLine();
                    if (text == RED_TURN.ToString())
                    {
                        Turn.PlayerColor = PieceColor.Red;
                        Turn.TurnImage = redPiece;
                    }
                    else
                    {
                        Turn.PlayerColor = PieceColor.White;
                        Turn.TurnImage = redPiece;
                    }
                    //board
                    for (int index1 = 0; index1 < boardSize; index1++)
                    {
                        text = reader.ReadLine();
                        for (int index2 = 0; index2 < boardSize; index2++)
                        {
                            squares[index1][index2].LegalSquareSymbol = null;
                            char pieceType = text[index2];

                            if (pieceType == NO_PIECE)
                            {
                                squares[index1][index2].Piece = null;
                            }
                            else if (pieceType == RED_PIECE)
                            {
                                squares[index1][index2].Piece = new GamePiece(PieceColor.Red, PieceType.Regular);
                                squares[index1][index2].Piece.Square = squares[index1][index2];
                                //to_DO
                            }
                            else if (pieceType == RED_KING)
                            {
                                squares[index1][index2].Piece = new GamePiece(PieceColor.Red, PieceType.King);
                                squares[index1][index2].Piece.Square = squares[index1][index2];
                                //todo
                            }
                            else if (pieceType == WHITE_PIECE)
                            {
                                squares[index1][index2].Piece = new GamePiece(PieceColor.White, PieceType.Regular);
                                squares[index1][index2].Piece.Square = squares[index1][index2];
                            }
                            else if (pieceType == WHITE_KING)
                            {
                                squares[index1][index2].Piece = new GamePiece(PieceColor.White, PieceType.King);
                                squares[index1][index2].Piece.Square = squares[index1][index2];
                            }
                        }
                    }

                    foreach (var square in CurrentNeighbours.Keys)
                    {
                        square.LegalSquareSymbol = null;
                    }

                    CurrentNeighbours.Clear();

                    do
                    {
                        text = reader.ReadLine();
                        if (text == "-")
                        {
                            if (text.Length == 1)
                            {
                                break;
                            }
                            CurrentNeighbours.Add(squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])], null);
                        }
                        else
                        {
                            CurrentNeighbours.Add(squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])],
                                squares[(int)char.GetNumericValue(text[2])][(int)char.GetNumericValue(text[3])]);
                            //TO-DO
                        }
                    } while (text != "-");

                }
            }
        }

        public static void SaveGame(ObservableCollection<ObservableCollection<GameSquare>> squares, GameLogic gameLogic)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            if (saveDialog.ShowDialog() == true)
            {
                var path = saveDialog.FileName;
                using (var writer = new StreamWriter(path))
                {
                    // Write the current state of the game from the gameLogic instance
                    writer.WriteLine(gameLogic.RedPiecesRemaining);
                    writer.WriteLine(gameLogic.WhitePiecesRemaining);
                    writer.WriteLine(gameLogic.Turn.PlayerColor == PieceColor.Red ? "Red" : "White");

                    // Write the current square if applicable
                    if (CurrentSquare != null)
                    {
                        writer.Write(CurrentSquare.Row.ToString() + CurrentSquare.Column.ToString());
                    }
                    else
                    {
                        writer.Write(NO_PIECE);
                    }
                    writer.WriteLine();

                    // Write flags for extra moves and paths
                    if (ExtraMove)
                    {
                        writer.Write(HAD_COMBO);
                    }
                    else
                    {
                        writer.Write(NO_PIECE);
                    }
                    writer.WriteLine();

                    if (ExtraPath)
                    {
                        writer.Write(EXTRA_PATH);
                    }
                    else
                    {
                        writer.Write(NO_PIECE);
                    }
                    writer.WriteLine();

                    // Write the board state
                    foreach (var line in squares)
                    {
                        foreach (var square in line)
                        {
                            if (square.Piece == null)
                            {
                                writer.Write(NO_PIECE);
                            }
                            else if (square.Piece.Color.Equals(PieceColor.Red) && square.Piece.Type == PieceType.Regular)
                            {
                                writer.Write(RED_PIECE);
                            }
                            else if (square.Piece.Color.Equals(PieceColor.White) && square.Piece.Type == PieceType.Regular)
                            {
                                writer.Write(WHITE_PIECE);
                            }
                            else if (square.Piece.Color.Equals(PieceColor.White) && square.Piece.Type == PieceType.King)
                            {
                                writer.Write(WHITE_KING);
                            }
                            else if (square.Piece.Color.Equals(PieceColor.Red) && square.Piece.Type == PieceType.King)
                            {
                                writer.Write(RED_KING);
                            }
                        }
                        writer.WriteLine();
                    }

                    // Write neighbors
                    foreach (var square in CurrentNeighbours.Keys)
                    {
                        if (CurrentNeighbours[square] == null)
                        {
                            writer.Write(square.Row.ToString() + square.Column.ToString() + NO_PIECE);
                        }
                        else
                        {
                            writer.Write(square.Row.ToString() + square.Column.ToString() + CurrentNeighbours[square].Row.ToString() + CurrentNeighbours[square].Column.ToString());
                        }
                        writer.WriteLine();
                    }
                    writer.Write("-\n");
                }
            }
        }

        public static void EnsureGameLogicInstance()
        {
            if (GameLogicInstance == null)
            {
                // Assuming you want to start scores from zero
                Winner initialWinner = new Winner(0, 0);
                PlayerTurn initialTurn = new PlayerTurn(PieceColor.Red); // or some logic to decide the starting color
                GameLogicInstance = new GameLogic(initBoard(), initialTurn, initialWinner);
            }
        }

        public static void ResetGame(ObservableCollection<ObservableCollection<GameSquare>> squares, GameLogic gameLogic)
        {
            foreach (var square in CurrentNeighbours.Keys)
            {
                square.LegalSquareSymbol = null;
            }

            if (CurrentSquare != null)
            {
                CurrentSquare.Texture = redSquare;
            }

            currentNeighbours.Clear();
            CurrentSquare = null;
            ExtraMove = false;
            ExtraPath = false;
            CollectedWhitePieces = 0;
            CollectedRedPieces = 0;
            Turn.PlayerColor = PieceColor.Red;

            // Reset the game board to its initial state
            ResetGameBoard(squares);

            // Reset the piece counts via GameLogic
            if (gameLogic != null)
            {
                gameLogic.ResetPieceCounts();
            }
        }

        #endregion

        #region Credits
        public static void About()
        {
            //string PATH = Path.GetDirectoryName(System.Environment.CurrentDirectory);
            //PATH = Directory.GetParent(PATH).FullName + @"\Resources\about.txt";
            string PATH = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\about.txt";

            using (var reader = new StreamReader(PATH))
            {
                MessageBox.Show(reader.ReadToEnd(), "About", MessageBoxButton.OKCancel);
            }
        }
        #endregion

        #region textFileHandling
        public static void writeScore(int r, int w)
        {
            string PATH = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\winnerText.txt";
            using (var writer = new StreamWriter(PATH))
            {
                writer.WriteLine(r + "," + w);
            }
        }

        public static Winner getScore()
        {
            Winner aux = new Winner(0, 0);
            string PATH = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\winnerText.txt";
            using (var reader = new StreamReader(PATH))
            {
                string line = reader.ReadLine();
                var splitted = line.Split(',');
                aux.RedWins = int.Parse(splitted[0]);
                aux.WhiteWins = int.Parse(splitted[1]);
            }
            return aux;
        }
        #endregion
    }
}
