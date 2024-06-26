﻿using Checkers.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static System.Net.Mime.MediaTypeNames;

namespace Checkers.Services
{
    public class Utility
    {
        #region constValues
        public const string redSquare = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\squareRed.png";
        public const string whiteSquare = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\squareWhite.png";
        public const string redPiece = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\pieceRed.png";
        public const string whitePiece = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\pieceWhite.png";
        public const string hintSquare = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\squareHint.png";
        public const string redKingPiece = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\pieceRedKing.png";
        public const string whiteKingPiece = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\pieceWhiteKing.png";
        public const string SQUARE_HIGHLIGHT = "NULL";

        public const char NO_PIECE = 'N';
        public const char WHITE_PIECE = 'W';
        public const char RED_PIECE = 'R';
        public const char RED_KING = 'K';
        public const char WHITE_KING = 'L';
        public const char WHITE_TURN = '2';
        public const char RED_TURN = '1';
        public const char HAD_COMBO = 'H';
        public const char EXTRA_PATH = 'E';

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
                       
                    }
                    else
                        if (index1 > 4)
                    {
                        squares[index1][index2].Piece = new GamePiece(PieceColor.Red);
                        squares[index1][index2].Piece.Square = squares[index1][index2];
                        
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
                string path = openDialog.FileName;
                try
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        if (!int.TryParse(reader.ReadLine(), out int redPiecesRemaining))
                        {
                            MessageBox.Show("Error parsing red pieces remaining.");
                            return;
                        }
                        gameLogic.RedPiecesRemaining = redPiecesRemaining;

                        if (!int.TryParse(reader.ReadLine(), out int whitePiecesRemaining))
                        {
                            MessageBox.Show("Error parsing white pieces remaining.");
                            return;
                        }
                        gameLogic.WhitePiecesRemaining = whitePiecesRemaining;

                        string playerTurn = reader.ReadLine();
                        if (!Enum.TryParse(playerTurn, true, out PieceColor turnColor))
                        {
                            MessageBox.Show("Error parsing player turn.");
                            return;
                        }
                        PlayerTurn newTurn = new PlayerTurn(turnColor);
                        gameLogic.UpdateTurn(newTurn);
                        newTurn.TurnImage = turnColor == PieceColor.White ? Utility.whitePiece : Utility.redPiece;

                        gameLogic.Turn = newTurn;
                        Utility.Turn = newTurn;

                        string text = reader.ReadLine();
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

                        string extraMoveLine = reader.ReadLine();
                        if (extraMoveLine == HAD_COMBO.ToString())
                        {
                            ExtraMove = true;
                            string[] position = reader.ReadLine().Split(',');
                            int row = int.Parse(position[0]);
                            int column = int.Parse(position[1]);
                            CurrentSquare = squares[row][column];
                            gameLogic.FindNeighbours(CurrentSquare);
                        }
                        else
                        {
                            ExtraMove = false;
                        }
                            Utility.CollectedRedPieces = int.Parse(reader.ReadLine()); 
                        Utility.CollectedWhitePieces = int.Parse(reader.ReadLine());
                        gameLogic.AllowMultipleJumps = reader.ReadLine() == "1";
                        gameLogic.GameStarted = true;

                        for (int i = 0; i < boardSize; i++)
                        {
                            string line = reader.ReadLine();
                            if (line == null || line.Length != boardSize)
                            {
                                MessageBox.Show($"Board line {i + 1} is corrupt or not in the expected format.");
                                return;
                            }

                            for (int j = 0; j < boardSize; j++)
                            {
                                char pieceType = line[j];
                                if (pieceType != NO_PIECE)
                                {
                                    GamePiece piece = ParsePieceType(pieceType, i, j);
                                    squares[i][j].Piece = piece;
                                }
                                else
                                {
                                    squares[i][j].Piece = null;
                                }
                            }
                        }

                        if (reader.ReadLine() != "-")
                        {
                            MessageBox.Show("End of data marker '-' not found.");
                            return;
                        }
                        gameLogic.CheckForWin();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception encountered during file load: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private static GamePiece ParsePieceType(char pieceType, int row, int col)
        {
            switch (pieceType)
            {
                case NO_PIECE: return null;
                case RED_PIECE: return new GamePiece(PieceColor.Red, PieceType.Regular);
                case WHITE_PIECE: return new GamePiece(PieceColor.White, PieceType.Regular);
                case RED_KING: return new GamePiece(PieceColor.Red, PieceType.King);
                case WHITE_KING: return new GamePiece(PieceColor.White, PieceType.King);
                default: throw new FormatException($"Unknown piece type '{pieceType}' at row {row}, column {col}");
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
                    writer.WriteLine(gameLogic.RedPiecesRemaining);
                    writer.WriteLine(gameLogic.WhitePiecesRemaining);
                    writer.WriteLine(gameLogic.Turn.PlayerColor == PieceColor.Red ? "Red" : "White");

                   
                    if (ExtraMove)
                    {
                        writer.Write(HAD_COMBO);
                        writer.WriteLine($"{CurrentSquare.Row},{CurrentSquare.Column}");

                    }
                    else
                    {
                        writer.Write(NO_PIECE);
                        writer.WriteLine();

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

                    int collectedRed = 12 - gameLogic.RedPiecesRemaining;
                    int collectedWhite = 12 - gameLogic.WhitePiecesRemaining;
                    writer.WriteLine(collectedRed); 
                    writer.WriteLine(collectedWhite);
                    writer.WriteLine(gameLogic.AllowMultipleJumps ? "1" : "0");

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

              
                    writer.Write("-\n");
                }
            }
        }

        public static void EnsureGameLogicInstance()
        {
            if (GameLogicInstance == null)
            {
                Winner initialWinner = new Winner(0, 0);
                PlayerTurn initialTurn = new PlayerTurn(PieceColor.Red); 
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
            Turn.TurnImage = Utility.redPiece;
            gameLogic.GameStarted = false;
            gameLogic.AllowMultipleJumps = false;

            gameLogic.NotifyPropertyChanged(nameof(gameLogic.GameStarted));
            gameLogic.NotifyPropertyChanged(nameof(gameLogic.AllowMultipleJumps));
            ResetGameBoard(squares);

            if (gameLogic != null)
            {
                gameLogic.ResetPieceCounts();
            }
        }

        #endregion

        #region Credits
        public static void About()
        {
           
            string PATH = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\about.txt";

            using (var reader = new StreamReader(PATH))
            {
                MessageBox.Show(reader.ReadToEnd(), "About", MessageBoxButton.OKCancel);
            }
        }
        #endregion

        #region textFileHandling
        public static void writeScore(Winner winner)
        {
            string PATH = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\winnerText.txt";
            using (var writer = new StreamWriter(PATH))
            {
                writer.WriteLine($"{winner.RedWins},{winner.WhiteWins},{winner.MaxRedPiecesLeft},{winner.MaxWhitePiecesLeft}");
            }
        }


        public static Winner getScore()
        {
            Winner aux = new Winner(0, 0);
            string PATH = "D:\\FACULTATE_AN_2\\MVP\\CheckersGame_Tema2\\Checkers\\Checkers\\Resources\\winnerText.txt";
            using (var reader = new StreamReader(PATH))
            {
                string line = reader.ReadLine();
                var parts = line.Split(',');
                aux.RedWins = int.Parse(parts[0]);
                aux.WhiteWins = int.Parse(parts[1]);
                aux.MaxRedPiecesLeft = int.Parse(parts[2]);
                aux.MaxWhitePiecesLeft = int.Parse(parts[3]);
            }
            return aux;
        }
        #endregion
    }
}
