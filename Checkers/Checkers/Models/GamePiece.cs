using Checkers.Services;
using Checkers.ViewModels;
using System.ComponentModel;

namespace Checkers.Models
{
    public class GamePiece : BaseNotification
    {
        private PieceColor color;
        private PieceType type;
        private string texture;
        private GameSquare square;

        public GamePiece(PieceColor color, PieceType type = PieceType.Regular)
        {
            this.color = color;
            this.type = type;
            UpdateTexture();
        }

        public PieceColor Color => color;

        public PieceType Type
        {
            get => type;
            set
            {
                if (SetProperty(ref type, value))
                {
                    UpdateTexture();
                }
            }
        }

        public string Texture
        {
            get => texture;
            set => SetProperty(ref texture, value);
        }

        public GameSquare Square
        {
            get => square;
            set => SetProperty(ref square, value);
        }

        private void UpdateTexture()
        {
            if (color == PieceColor.Red)
            {
                Texture = type == PieceType.King ? Utility.redKingPiece : Utility.redPiece;
            }
            else
            {
                Texture = type == PieceType.King ? Utility.whiteKingPiece : Utility.whitePiece;
            }
        }
    }
}
