namespace TicTacToeProject
{
    internal class Program
    {
        private static int ACTUALPLAYER = 1;
        private static int counter = 0;
        private static int FIELD = 0;
        private static int WINNING_CONDITION = 3;

        static void Main(string[] args)
        {
            init();
            run();
        }

        static void restart()
        {
            Console.WriteLine("Press 'R' to restart the game or press any keys to escape from the Game");
            string exitOrRestert = Console.ReadLine();
            if (exitOrRestert == "r" || exitOrRestert == "R")
            {
                init();
                run();
            }

        }

        static void run()
        {
            while (!Checker2(WINNING_CONDITION) && counter < Math.Pow(FIELD,2))
            {
                Console.Write("Player {0}: Choose your field!", ACTUALPLAYER);
                string input = Console.ReadLine();
                step(input);

            }

            if (Checker2(WINNING_CONDITION))
            {
                Console.WriteLine("Player {0} won!", ACTUALPLAYER);
                restart();
            }

            if (counter == Math.Pow(FIELD,2))
            {
                Console.WriteLine("Nobody won.");
                restart();
            }
        }

        static string[,] board;

        static void init()
        {
            Console.WriteLine("Type the size of the field (3-10): \n3: 3x3, \n4: 4x4 \n5: 5x5 etc.\n ");
            bool isFieldCorrect = int.TryParse(Console.ReadLine(), out FIELD);
            isFieldCorrect = FIELD < 10 && FIELD > 2 ? true : false;
            Console.WriteLine("How many mark 'X' or '0' have to come in a row to win? \n Min 3 but not greater than the field");
            bool isWinningCondCorrect = int.TryParse(Console.ReadLine(),out WINNING_CONDITION);
            isWinningCondCorrect = WINNING_CONDITION <= FIELD? true : false;
            if (isFieldCorrect && isWinningCondCorrect)
            {
                createBoard(FIELD);
                ACTUALPLAYER = 1;
                counter = 0;
                drawTable();
            }
            else
            {
                Console.WriteLine("Something went wrong. Try again passing the values.");
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
                Console.WriteLine("Enter a valid number b/w 0-9 which are not signed yet!");
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

        static bool Checker2(int winningCondition) {

            int differentTablesToCheck = FIELD - winningCondition;
           
            if (differentTablesToCheck == 0)
            {
               return Checker(board, 0,0);
            }
            else
            {
                for (int i = 0; i < differentTablesToCheck; i++)
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

            //Check if all the symbol in the diagonal is the same
            //If it is not, the counter is set back to 0.
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
                if (board[0, WINNING_CONDITION-1] == board[j, (WINNING_CONDITION-1)-j])
                {
                    counter++;
                }
            }
            //Check if all the symbol in the diagonal is the same
            //If it is not, the counter is set back to 0.
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
                //int counter = 0;
                
                //horizontal check
                for (int k = startingColumn; k < WINNING_CONDITION+startingColumn; k++)
                {
                    if (board[i, startingColumn] == board[i, k])
                    {
                        counter++;
                    
                    }
                }
                //Check if all the symbol in the row is the same
                //If it is not, the counter is set back to 0.
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
                //Check if all the symbol in the column is the same
                //If it is not, the counter is set back to 0.
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