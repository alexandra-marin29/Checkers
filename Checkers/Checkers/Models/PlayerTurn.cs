using Checkers.Models;
using Checkers.Services;
using Checkers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Models
{
    public class PlayerTurn : BaseNotification
    {
        private PieceColor color;
        private string image;

        public PlayerTurn(PieceColor color)
        {
            this.color = color;
            TurnImage = color == PieceColor.White ? Utility.whitePiece : Utility.redPiece;

            //loadImages();
        }

        public void loadImages()
        {
            if (color == PieceColor.Red)
            {
                image = Utility.redPiece;
                return;
            }
            image = Utility.whitePiece;
        }

        public PieceColor PlayerColor
        {
            get { return color; }
            set
            {
                color = value;
               OnPropertyChanged("PlayerColor");
            }
        }

        public string TurnImage
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged("TurnImage");
            }
        }
    }
}
