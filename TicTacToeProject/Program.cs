﻿namespace TicTacToeProject
{
    internal class Program
    {
        private static int ACTUALPLAYER = 1;
        private static int counter = 0;
        private static int FIELD = 0;
        private static int WINNING_CONDITION = 3;
        private static bool AGAINST_COMPUTER = false;
        static string[,] board;

        static void Main(string[] args)
        {
            init();
            run();
        }

        static void restart()
        {
            Console.WriteLine("Nyomj egy 'R' hogy újra kezd a játékot.");
            string exitOrRestart = Console.ReadLine();
            if (exitOrRestart == "r" || exitOrRestart == "R")
            {
                init();
                run();
            }

        }

        static void run()
        {
            while (!Checker2(WINNING_CONDITION) && counter < Math.Pow(FIELD,2))
            {
                Console.Write("Játékos {0}: Válassz egy mezőt!", ACTUALPLAYER);
                if (AGAINST_COMPUTER)
                {
                    if(ACTUALPLAYER == 1)
                    {
                        string input = Console.ReadLine();
                        step(input);
                    }
                    else
                    {
                        step(PCStep());
                    }
                }
                else
                {
                string input = Console.ReadLine();
                step(input);
                }
            }

            if (Checker2(WINNING_CONDITION))
            {
                Console.WriteLine("Játékos {0} nyert!", ACTUALPLAYER);
                restart();
            }

            if (counter == Math.Pow(FIELD,2))
            {
                Console.WriteLine("Döntetlen!");
                restart();
            }
        }

        

        static void init()
        {
            char yourCharAscii;
            char[] rightInputs = { 'Y', 'y', 'N', 'n' };
            do
            {
                Console.WriteLine("Számítógép ellen akarsz játszani? Nyomj egy 'Y'-t ha igen 'N'-t ha nem.");
                 yourCharAscii = char.Parse(Console.ReadLine());
            } while (!rightInputs.Contains(yourCharAscii));
          
            if(yourCharAscii == 'y' || yourCharAscii == 'Y')
            {
                AGAINST_COMPUTER = true;
            }

            do
            {
                Console.WriteLine("Hányszor-hányas mezőbe játszanál? (3-10): \n3: 3x3, \n4: 4x4 \n5: 5x5 etc.\n ");
            }
            while (!(int.TryParse(Console.ReadLine(), out FIELD) && FIELD < 11 && FIELD > 2));

            Console.WriteLine("Mennyi jel kelljen a győzelemhez? \n Minimum 3 de nem nagyobb mint az aktuális mező méret.");
            bool isWinningCondCorrect = int.TryParse(Console.ReadLine(),out WINNING_CONDITION);
            isWinningCondCorrect = WINNING_CONDITION <= FIELD? true : false;
            if (isWinningCondCorrect)
            {
                createBoard(FIELD);
                ACTUALPLAYER = 1;
                counter = 0;
                drawTable();
            }
            else
            {
                Console.WriteLine("Hiba az adatok bevitelébe. Próbáld újra.");
                init();
            }
           
        }

        /// <summary>
        /// Function to create the field. You can pass the value how deep field do you want.
        /// </summary>
        /// <remarks>
        /// e.g. numb = 3. It means the board sets to 3x3.
        /// </remarks>
        /// <param name="numb">
        /// </param>
        static void createBoard(int numb)
        {
            board = new string[numb, numb];

            int counter = 0;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int k = 0; k < board.GetLength(1); k++)
                {
                    board[i, k] = Convert.ToString(counter);

                    counter++;
                }
            }
        }

        static void drawTable()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int k = 0; k < board.GetLength(1); k++)
                {
                    if (board[i, k].ToString().Length < 2) {
                        Console.Write(" "+board[i, k] + "  |  ");
                    }
                    else
                    {
                        Console.Write(" "+board[i, k] + " |  ");
                    }
                }
                Console.WriteLine("\n"+drawRow(FIELD));
            }
        }

        static string drawRow(int numberOfRow) {
            string line="";
            for (int i = 0; i < numberOfRow; i++)
            {
                line += " - - -";
            }

            return line;
        }

        static void modifyOneItem(int[] coord, string value)
        {
            board[coord[0], coord[1]] = value;
            Console.Clear();
            counter++;
            drawTable();
        }

        static void step(string input)
        {
            bool isInputCorrect = int.TryParse(input, out int inputNumb);
            double maxInput = Math.Pow(FIELD,2);
            if (isInputCorrect && inputNumb >= 0 && inputNumb < maxInput && checkTheField(getTheCoord(inputNumb, FIELD)))
            {
                modifyOneItem(getTheCoord(inputNumb, FIELD), getTheActualTicTac());
                if (!Checker2(WINNING_CONDITION)) changePlayer();
            }
            else
            {
                Console.WriteLine("Enter a valid number which are not marked yet!");
            }
        }

        //Check if the field is not signed yet
        static bool checkTheField(int[] coord)
        {
            string actualField = board[coord[0], coord[1]];
            return (actualField != "X" && actualField != "O") ? true : false;
        }

        static void changePlayer()
        {
            ACTUALPLAYER = ACTUALPLAYER == 1 ? 2 : 1;
        }

        static string getTheActualTicTac()
        {
            return ACTUALPLAYER == 1 ? "X" : "O";
        }

        static int[] getTheCoord(int inputNumb, int field)
        {
            int row = inputNumb/field;
            int column = inputNumb%field;
            return new int[] { row, column };
        }

        static string PCStep()
        {
            Random dice = new Random();
            int nextStep = dice.Next(0,(FIELD*FIELD-1));
            return nextStep.ToString();

        }

        static bool Checker2(int winningCondition) {

            int differentTablesToCheck = FIELD - winningCondition;
           
            if (differentTablesToCheck == 0)
            {
               return Checker(board, 0,0);
            }
            else
            {
                for (int i = 0; i <= differentTablesToCheck; i++)
                {
                    for (int k = 0; k <= differentTablesToCheck; k++)
                    {
                        if(Checker(board, i, k))
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                return false;
            }
        }

        static bool Checker(string[,] board, int startingRow, int startingColumn)
        {
            int counter = 0;
            //Diagonal check 1
            for (int l = 0; l < WINNING_CONDITION; l++)
            {
                if (board[startingRow, startingColumn] == board[l+startingRow,l+startingColumn])
                {
                    counter++;
                }
            }

            if (counter == WINNING_CONDITION)
            {
                return true;
            }
            else
            {
                counter = 0;
            }

            //Diagonal check 2
            for (int j = 0; j < WINNING_CONDITION; j++)
            {
                int newStartingColumn = startingColumn + (WINNING_CONDITION-1);
                if (board[startingRow, newStartingColumn] == board[startingRow+j, newStartingColumn - j])
                {
                    counter++;
                }
                
            }

            if (counter == WINNING_CONDITION)
            {
                return true;
            }
            else
            {
                counter = 0;
            }

            for (int i = startingRow; i < WINNING_CONDITION+startingRow; i++)
            {

                //horizontal check
                for (int k = startingColumn; k < WINNING_CONDITION+startingColumn; k++)
                {
                    if (board[i, startingColumn] == board[i, k])
                    {
                        counter++;
                    
                    }
                }

                if(counter == WINNING_CONDITION )
                {
                    return true;
                }
                else
                {
                    counter = 0;
                }   
            }

            for (int k = startingColumn; k < WINNING_CONDITION+startingColumn; k++)
            {
                //vertical check
                for (int i = startingRow; i < WINNING_CONDITION+startingRow; i++)
                {
                    if (board[startingRow, k] == board[i, k])
                    {
                        counter++;
                    }
                }

                if (counter == WINNING_CONDITION)
                {
                    return true;
                }
                else
                {
                    counter = 0;
                }
            }

            return false;
        }
    }
}