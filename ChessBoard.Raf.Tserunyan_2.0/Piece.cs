using System;
using System.Collections.Generic;

namespace ChessBoard.Raf.Tserunyan_2._0
{
    public class Piece
    {
        public static Board board;
        public List<object> AvailableCells = new List<object>();
        public List<object> EatableCells = new List<object>();

        public Piece(string name, string color)
        {
            Name = name;
            Color = color;
        }
        public Piece(string name, string color, int _i, int _j)
        {
            Name = name;
            Color = color;
            i = _i;
            j = _j;
        }

        public string Color { get; set; }
        public string Name { get; set; }
        public int MinI { get; set; }
        public bool IsValid = false;

        public byte I;
        private int i
        {
            get { return I; }
            set
            {
                if (value < 0)
                    I = 0;
                else if (value > 7)
                    I = 7;
                else
                    I = (byte)value;
            }
        }

        public byte J;
        private int j
        {
            get { return J; }
            set
            {
                if (value < 0)
                    J = 0;
                else if (value > 7)
                    J = 7;
                else
                    J = (byte)value;
            }
        }


        public override string ToString()
        {
            return this.Name.Substring(0, 1);
        }

        public void SetEatableCells()
        {
            switch (this.ToString())
            {
                case "K":
                    {
                        for (int i = I - 1; i < I + 2; i++)
                        {
                            if (i < 0)
                                continue;
                            else if (i == board.Matrix.GetLength(0))
                                break;

                            for (int j = J - 1; j < J + 2; j++)
                            {
                                if (j < 0)
                                    continue;
                                else if (j == board.Matrix.GetLength(1))
                                    break;

                                if (i == this.I && j == this.J)
                                    continue;

                                if (board.Matrix[i, j] is Piece)
                                {
                                    Piece p = board.Matrix[i, j] as Piece;
                                    if (p.Color != this.Color)
                                        this.EatableCells.Add(board.Matrix[i, j]);
                                }
                                else
                                    this.EatableCells.Add(board.Matrix[i, j]);
                            }
                        }
                        break;
                    }
                case "Q":
                    {
                        bool CanGoRight = true, CanGoLeft = true, CanGoUp = true, CanGoDown = true;
                        bool CanGoUpLeft = true, CanGoUpRight = true, CanGoDownRight = true, CanGoDownLeft = true;
                        for (int t = 1; t < 8; t++)
                        {
                            if (I - t >= 0 && CanGoUp)
                                AddToEatListIfNeeded(board.Matrix[I - t, J], ref CanGoUp);
                            if (I + t < 8 && CanGoDown)
                                AddToEatListIfNeeded(board.Matrix[I + t, J], ref CanGoDown);
                            if (J + t < 8 && CanGoRight)
                                AddToEatListIfNeeded(board.Matrix[I, J + t], ref CanGoRight);
                            if (J - t >= 0 && CanGoLeft)
                                AddToEatListIfNeeded(board.Matrix[I, J - t], ref CanGoLeft);

                            if (I - t >= 0 && J - t >= 0 && CanGoUpLeft)
                                AddToEatListIfNeeded(board.Matrix[I - t, J - t], ref CanGoUpLeft);
                            if (I - t >= 0 && J + t < 8 && CanGoUpRight)
                                AddToEatListIfNeeded(board.Matrix[I - t, J + t], ref CanGoUpRight);
                            if (I + t < 8 && J + t < 8 && CanGoDownRight)
                                AddToEatListIfNeeded(board.Matrix[I + t, J + t], ref CanGoDownRight);
                            if (I + t < 8 && J - t >= 0 && CanGoDownLeft)
                                AddToEatListIfNeeded(board.Matrix[I + t, J - t], ref CanGoDownLeft);
                        }
                        break;
                    }
                case "R":
                    {
                        bool CanGoRight = true, CanGoLeft = true, CanGoUp = true, CanGoDown = true;
                        for (int t = 1; t < 8; t++)
                        {
                            if (I - t >= 0 && CanGoUp)
                                AddToEatListIfNeeded(board.Matrix[I - t, J], ref CanGoUp);
                            if (I + t < 8 && CanGoDown)
                                AddToEatListIfNeeded(board.Matrix[I + t, J], ref CanGoDown);
                            if (J + t < 8 && CanGoRight)
                                AddToEatListIfNeeded(board.Matrix[I, J + t], ref CanGoRight);
                            if (J - t >= 0 && CanGoLeft)
                                AddToEatListIfNeeded(board.Matrix[I, J - t], ref CanGoLeft);
                        }
                        break;
                    }
            }
        }
        private void AddToEatListIfNeeded(object newPiece, ref bool direction)
        {
            if (!(newPiece is Piece))
                EatableCells.Add(newPiece);
            else
            {
                Piece piece = newPiece as Piece;
                if (piece.Color == this.Color)
                    direction = false;
                else
                    EatableCells.Add(newPiece);
            }
        }

        public void SetAvailableCells()
        {
            switch (this.ToString())
            {
                case "K":
                    {
                        for (int i = I - 1; i < I + 2; i++)
                        {
                            if (i < 0)
                                continue;
                            else if (i == board.Matrix.GetLength(0))
                                break;

                            for (int j = J - 1; j < J + 2; j++)
                            {
                                if (j < 0)
                                    continue;
                                else if (j == board.Matrix.GetLength(1))
                                    break;

                                if (i == this.I && j == this.J)
                                    continue;

                                board.Matrix[I, J] = ' ';

                                //Cheking if going to that destination is not dangerous
                                bool isShax = false;
                                foreach (Piece item in board.WhitePieces)
                                {
                                    foreach (object cell in item.EatableCells)
                                    {
                                        if (board.Matrix[i, j] == cell)
                                            isShax = true;
                                    }
                                }

                                if (!isShax)
                                    AddToListIfNeeded(board.Matrix[i, j]);
                                board.Matrix[I, J] = this;
                            }
                        }

                        break;
                    }
                case "Q":
                    {
                        bool CanGoRight = true, CanGoLeft = true, CanGoUp = true, CanGoDown = true;
                        bool CanGoUpLeft = true, CanGoUpRight = true, CanGoDownRight = true, CanGoDownLeft = true;
                        for (int t = 1; t < 8; t++)
                        {
                            if (I - t >= 0 && J - t >= 0 && CanGoUpLeft)
                                AddToListIfNeeded(board.Matrix[I - t, J - t], ref CanGoUpLeft);
                            if (I - t >= 0 && J + t < 8 && CanGoUpRight)
                                AddToListIfNeeded(board.Matrix[I - t, J + t], ref CanGoUpRight);
                            if (I + t < 8 && J + t < 8 && CanGoDownRight)
                                AddToListIfNeeded(board.Matrix[I + t, J + t], ref CanGoDownRight);
                            if (I + t < 8 && J - t >= 0 && CanGoDownLeft)
                                AddToListIfNeeded(board.Matrix[I + t, J - t], ref CanGoDownLeft);

                            if (I - t >= 0 && CanGoUp)
                                AddToListIfNeeded(board.Matrix[I - t, J], ref CanGoUp);
                            if (I + t < 8 && CanGoDown)
                                AddToListIfNeeded(board.Matrix[I + t, J], ref CanGoDown);
                            if (J + t < 8 && CanGoRight)
                                AddToListIfNeeded(board.Matrix[I, J + t], ref CanGoRight);
                            if (J - t >= 0 && CanGoLeft)
                                AddToListIfNeeded(board.Matrix[I, J - t], ref CanGoLeft);
                        }
                        break;
                    }
                case "R":
                    {
                        bool CanGoRight = true, CanGoLeft = true, CanGoUp = true, CanGoDown = true;
                        for (int t = 1; t < 8; t++)
                        {
                            if (I - t >= 0 && CanGoUp)
                                AddToListIfNeeded(board.Matrix[I - t, J], ref CanGoUp);
                            if (I + t < 8 && CanGoDown)
                                AddToListIfNeeded(board.Matrix[I + t, J], ref CanGoDown);
                            if (J + t < 8 && CanGoRight)
                                AddToListIfNeeded(board.Matrix[I, J + t], ref CanGoRight);
                            if (J - t >= 0 && CanGoLeft)
                                AddToListIfNeeded(board.Matrix[I, J - t], ref CanGoLeft);
                        }
                        break;
                    }
            }
        }
        private void AddToListIfNeeded(object newPiece)
        {
            if (!(newPiece is Piece))
                AvailableCells.Add(newPiece);
            else
            {
                Piece piece = newPiece as Piece;
                if (piece.Color != this.Color)
                    AvailableCells.Add(piece);
            }
        }
        private void AddToListIfNeeded(object newPiece, ref bool direction)
        {
            if (!(newPiece is Piece))
                AvailableCells.Add(newPiece);
            else
            {
                Piece piece = newPiece as Piece;
                if (piece.Color != this.Color)
                    AvailableCells.Add(piece);
                direction = false;
            }
        }

        public void PutOnBoard()
        {
            board.Matrix[this.I, this.J] = this;
        }

        public void Validate(string coordinates)
        {
            byte row;
            byte column;

            try
            {
                row = (byte)(8 - byte.Parse(coordinates.Substring(0, 1)));
                column = (byte)(Convert.ToByte(Convert.ToChar(coordinates.Substring(2, 1).ToUpper())) - 65);
            }
            catch (Exception)
            {
                throw new Exception("You input was not in correct format. Here's an example for you: 7 E");
            }

            if (board.Matrix[row, column] is Piece)
            {
                throw new Exception("There already is a piece in that cell, try another one...");
            }
            else
            {
                board.Matrix[row, column] = this;

                #region Set available and eatable cells
                for (int a = board.Pieces.Count - 1; a >= 0; a--)
                {
                    if (board.Pieces[a].IsValid)
                    {
                        board.Pieces[a].SetEatableCells();
                    }
                }
                for (int a = board.Pieces.Count - 1; a >= 0; a--)
                {
                    if (board.Pieces[a].IsValid)
                    {
                        board.Pieces[a].SetAvailableCells();
                    }
                }
                #endregion

                //Esi mi ban en chi
                for (int a = board.Pieces.Count - 1; a > 0; a--)
                {
                    if (board.Pieces[a].IsValid)
                    {
                        if (board.Pieces[a].CanEat(row, column))
                        {
                            board.Matrix[row, column] = ' ';
                            throw new Exception("You can't put it in that cell, because you'll be under 'shakh' situation!\n Try another cell...");
                        }
                    }
                }

                board.Matrix[row, column] = this;
                this.I = row;
                this.J = column;
                this.IsValid = true;

                if (this.Color == "White")
                    board.WhitePieces.Add(this);

                board.Show();
            }
        }

        public void Move(string coordinates)
        {
            try
            {
                Piece piece = board.Matrix[I, J] as Piece;

                if (String.IsNullOrEmpty(coordinates))
                    throw new Exception("You haven't entered anything, try again...");
                byte i = (byte)(8 - byte.Parse(coordinates.Substring(0, 1)));
                byte j = (byte)(Convert.ToByte(Convert.ToChar(coordinates.Substring(2, 1).ToUpper())) - 65);

                //Checking for possible mistakes
                if (i == board.Pieces[0].I && j == board.Pieces[0].J)
                    throw new Exception("You've entered the black king's existing coordinates...");


                //Checking if this piece can go there
                bool contains = false;
                foreach (var item in this.AvailableCells)
                {
                    if (item == board.Matrix[i, j])
                    {
                        contains = true;
                        break;
                    }
                }
                if (contains)
                {
                    if (board.Matrix[i, j] is Piece)
                    {
                        Piece pc = board.Matrix[i, j] as Piece;
                        board.Pieces.Remove(pc);

                        if (pc.Color == "White")
                        {
                            board.WhitePieces.Remove(pc);
                            if (pc == board.Pieces[1])
                            {
                                Program.isMate = true;

                                board.Pieces[0].AvailableCells.Clear();
                                Console.Clear();
                                board.Show();

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("You Won! Congratulations!!");
                                Console.ReadKey();
                            }
                        }
                        else
                            Program.Mate();
                    }

                    board.Matrix[i, j] = this;
                }
                else
                    throw new Exception($"You cant move to the destination {coordinates.ToUpper()}, \n Something's blocking your way or your piece just can't go there.");


                board.Matrix[I, J] = ' ';
                I = i;
                J = j;

                #region Clearing and reloading lists

                foreach (Piece item in board.Pieces)
                {
                    item.EatableCells.Clear();
                    item.SetEatableCells();
                }
                foreach (Piece item in board.Pieces)
                {
                    item.AvailableCells.Clear();
                    item.SetAvailableCells();
                }

                #endregion

                board.Show();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Move(int i, int j)
        {
            try
            {
                Piece piece = board.Matrix[I, J] as Piece;

                //Checking if this piece can go there
                if (board.Matrix[i, j] is Piece)
                {
                    Piece pc = board.Matrix[i, j] as Piece;
                    board.Pieces.Remove(pc);

                    if (pc.Color == "White")
                        board.WhitePieces.Remove(pc);
                    else
                        Program.Mate();
                }

                board.Matrix[i, j] = this;


                board.Matrix[I, J] = ' ';
                I = (byte)i;
                J = (byte)j;

                #region Clearing and reloading lists

                foreach (Piece item in board.Pieces)
                {
                    item.EatableCells.Clear();
                    item.SetEatableCells();
                }
                foreach (Piece item in board.Pieces)
                {
                    item.AvailableCells.Clear();
                    item.SetAvailableCells();
                }

                #endregion

                board.Show();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CanEat(Piece piece)
        {
            foreach (object cell in this.AvailableCells)
            {
                if (cell == piece)
                    return true;
            }
            return false;
        }
        public bool CanEat(byte i, byte j)
        {
            foreach (object cell in this.AvailableCells)
            {
                if (board[i, j] == cell)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasSomewhereToGo
        {
            get
            {
                if (AvailableCells.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public static void ClearAvailableAndEatableCells()
        {
            foreach (Piece item in board.Pieces)
            {
                item.AvailableCells.Clear();
                item.EatableCells.Clear();
            }
        }
    }
}