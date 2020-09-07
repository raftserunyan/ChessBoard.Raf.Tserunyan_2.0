using System;
using System.Threading;
using System.Collections.Generic;

namespace ChessBoard.Raf.Tserunyan_2._0
{
    class Program
    {
        static Board board;
        public static bool isMate = false;

        static void Main(string[] args)
        {
            board = new Board();
            board.Show();

            AskForCoordinates();

            bool kingMovedSuccessfully = true;

            //Playing logic
            while (!isMate)
            {
                try
                {
                    if (kingMovedSuccessfully) //Ete black kingy qayl katarec, xaxum a systemy
                    {
                        Console.WriteLine();
                        Console.Write("Waiting for the system to make a move...");

                        Thread.Sleep(2200);

                        SystemMakeMove();
                        board.Show();
                    }

                    //Checking for mate
                    if (!board.Pieces[0].HasSomewhereToGo)
                        Mate();

                    //board.Show();
                    Console.WriteLine();
                    Console.Write("Enter new coordinates for the black king (example: 7 F): ");
                    string coordinates = Console.ReadLine();
                    board.Pieces[0].Move(coordinates);
                    kingMovedSuccessfully = true;
                    board.Show();


                    if (board.WhitePieces.Count < 2)
                    {
                        Program.isMate = true;

                        board.Pieces[0].AvailableCells.Clear();
                        Console.Clear();
                        board.Show();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You Won! Congratulations!!");
                        Console.ReadKey();
                    }

                    //Check for shakh
                    if (IsShakh())
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("System> Shakh!");
                        Console.ResetColor();
                    }
                }

                catch (Exception e)
                {
                    if (e.Message != "raf")
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ResetColor();
                    }
                    kingMovedSuccessfully = false;
                }
            }
        }

        public static void Mate()
        {
            isMate = true;

            board.Pieces[0].AvailableCells.Clear();
            Console.Clear();
            board.Show();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("MATE!");
            Console.ReadKey();
        }

        private static byte i = 1;
        private static byte ind
        {
            get
            {
                return i;
            }
            set
            {
                if (value >= board.WhitePieces.Count)
                    i = 0;
                else if (value < 0)
                    i = 0;
                else
                    i = value;
            }
        }
        private static void SystemMakeMove()
        {
            bool isEmergency = false;

            for (byte w = 0; w < board.WhitePieces.Count; w++)
            {
                if (board.Pieces[0].CanEat(board.WhitePieces[w]))
                {
                    ind = w;
                    isEmergency = true;
                }
            }

            Piece piece = board.WhitePieces[ind++];

            if (isEmergency) //Paxnum enq
            {
                List<int> lst = new List<int>();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        for (int c = 0; c < piece.AvailableCells.Count; c++)
                        {
                            if (board.Matrix[i, j] == piece.AvailableCells[c])
                            {
                                if (Math.Abs(i - board.Pieces[0].I) > 1)
                                    lst.Add(c);
                            }
                        }
                    }
                }

                Random rnd = new Random();
                int indx = rnd.Next(0, lst.Count);

                if (lst.Count > 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (board.Matrix[i, j] == piece.AvailableCells[lst[indx]])
                            {
                                piece.Move(i, j);
                                return;
                            }
                        }
                    }
                }
                else
                    SystemMakeMove();
            }
            else //gnum enq mat anelu
            {
                List<int> lst = new List<int>();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        for (int c = 0; c < piece.AvailableCells.Count; c++)
                        {
                            if (board.Matrix[i, j] == piece.AvailableCells[c])
                            {
                                bool letgo = true;
                                foreach (object kcell in board.Pieces[0].EatableCells)
                                {
                                    if (kcell == board.Matrix[i, j])
                                    {
                                        letgo = false;
                                        break;
                                    }
                                }

                                if (letgo)
                                {
                                    if (Math.Abs(i - board.Pieces[0].I) < 2)
                                    {
                                        bool g = true;
                                        foreach (Piece item in board.WhitePieces)
                                        {
                                            if (item.I == i)
                                            {
                                                if (item.Name == "King")
                                                {
                                                    if (item.CanEat(board.Pieces[0]))
                                                    {
                                                        g = false;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    g = false;
                                                    break;
                                                }
                                            }
                                        }
                                        if (g)
                                        {
                                            lst.Add(c);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (lst.Count > 0)
                {
                    Random rnd = new Random();
                    int indx = rnd.Next(0, lst.Count);

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (board.Matrix[i, j] == piece.AvailableCells[lst[indx]])
                            {
                                piece.Move(i, j);
                                board.Show();
                                return;
                            }
                        }
                    }
                }
                else
                    SystemMakeMove();
            }
        }

        private static bool IsShakh()
        {
            foreach (Piece piece in board.Pieces)
            {
                if (piece.CanEat(board.Pieces[0]))
                    return true;
            }
            return false;
        }

        private static void AskForCoordinates()
        {
            for (int t = board.Pieces.Count - 1; t >= 0; t--)
            {
                Console.WriteLine();
                Console.Write($"Where do we put the {board.Pieces[t].Color} {board.Pieces[t].Name}? : ");
                string coordinates = Console.ReadLine();

                try
                {
                    board.Pieces[t].Validate(coordinates);
                    Piece.SetEatableAndAvailableCells();
                    board.Show();
                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();

                    Piece.ClearAvailableAndEatableCells();

                    t++;
                    continue;
                }
            }
        }
    }
}