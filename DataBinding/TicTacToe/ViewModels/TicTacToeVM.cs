using System;
using System.Linq;
using System.Windows.Input;
using TicTacToe.Behaviour;
using TicTacToe.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TicTacToe.ViewModels
{
    public class TicTacToeVM : BindableBase
    {
        private string playerInTurn;

        private Random rand;

        private  ObservableCollection<string> marks;

        private ICommand init;

        private ICommand setMark;

        public TicTacToeVM()
        {
            this.rand = new Random();
            this.Initialize();
        }

        public IList<string> Marks{
            get
            {
                if (this.marks == null)
                {
                    this.marks = new ObservableCollection<string>();
                }
                return this.marks;
            }
            set
            {
                if (this.marks == null)
                {
                    this.marks = new ObservableCollection<string>();
                }
                this.marks.Clear();
                foreach (var item in value)
                {
                    this.marks.Add(item);
                }
            }
        }

        public string PlayerInTurn
        {
            get
            {
                return this.playerInTurn;
            }
            set
            {
                this.playerInTurn = value;
                this.OnPropertyChanged("PlayerInTurn");
            }
        }

        public ICommand SetMark
        {
            get
            {
                if (this.setMark == null)
                {
                    this.setMark = 
                        new DelegateCommand<string>(this.HandleSetMarkCommand);
                }

                return this.setMark;
            }
        }

        public ICommand Init
        {
            get
            {
                if (true)
                {
                    this.init = new DelegateCommand<string>(this.HandleInitCommand);
                }
                return this.init;
            }
        }

        private void HandleInitCommand(string paramater)
        {
            this.Initialize();
        }
        
        private void HandleSetMarkCommand(string parameter)
        {
            var index = int.Parse(parameter);

            Mark(index);

            this.ChangePlayerInTurn();
        }

        private void Initialize()
        {
            if (this.rand.Next(100) < 50)
            {
                this.PlayerInTurn = "X";
            }
            else
            {
                this.PlayerInTurn = "O";
            }
            this.Marks = new ObservableCollection<string>() { " ", " ", " ", " ", " ", " ", " ", " ", " ", };
        }
  
        private void Mark(int index)
        {
            if (this.Marks[index] == " ")
            {
                this.Marks[index] = this.PlayerInTurn;
            }
        }

        private void ChangePlayerInTurn()
        {
            if (this.PlayerInTurn == "X")
            {
                this.PlayerInTurn = "O";
            }
            else
            {
                this.PlayerInTurn = "X";
            }
        }
       

    }
}
