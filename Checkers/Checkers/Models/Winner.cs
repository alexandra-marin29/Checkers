using Checkers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Models
{
    public class Winner : BaseNotification
    {
        private int redWins;
        private int whiteWins;
        private int maxRedPiecesLeft; 
        private int maxWhitePiecesLeft;
        public Winner(int redWins, int whiteWins)
        {
            this.redWins = redWins;
            this.whiteWins = whiteWins;
        }

        public int RedWins
        {
            get
            {
                return redWins;
            }
            set
            {
                redWins = value;
                OnPropertyChanged("RedWins");
            }
        }

        public int WhiteWins
        {
            get
            {
                return whiteWins;
            }
            set
            {
                whiteWins = value;
                OnPropertyChanged("WhiteWins");
            }
        }
        public int MaxRedPiecesLeft
        {
            get => maxRedPiecesLeft;
            set => SetProperty(ref maxRedPiecesLeft, value);
        }

        public int MaxWhitePiecesLeft
        {
            get => maxWhitePiecesLeft;
            set => SetProperty(ref maxWhitePiecesLeft, value);
        }
    }
}
