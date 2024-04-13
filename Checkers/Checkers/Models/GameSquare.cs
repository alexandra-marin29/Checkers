using Checkers.Models;
using Checkers.Services;
using Checkers.ViewModels;
using System;
using System.ComponentModel;

namespace Checkers.Models
{
    public class GameSquare : BaseNotification
    {
        private int row;
        private int column;
        private SquareShade shade;
        private string texture;
        private GamePiece piece;
        private string legalSquareSymbol;

        public GameSquare(int row, int column, SquareShade shade, GamePiece piece)
        {
            this.row = row;
            this.column = column;
            this.shade = shade;
            if (shade == SquareShade.Dark)
            {
                texture = Utility.redSquare;
            }
            else
            {
                texture = Utility.whiteSquare;
            }
            this.piece = piece;
        }

        public int Row => row;
        public int Column => column;
        public SquareShade Shade => shade;

        public string Texture
        {
            get => texture;
            set => SetProperty(ref texture, value);
        }

        public GamePiece Piece
        {
            get => piece;
            set => SetProperty(ref piece, value);
        }

        public string LegalSquareSymbol
        {
            get => legalSquareSymbol;
            set => SetProperty(ref legalSquareSymbol, value);
        }
    }
}
