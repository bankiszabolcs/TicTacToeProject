﻿namespace TicTacToeProject
{
    internal class Program
    {
        private static int ACTUALPLAYER = 1;
        private static int counter = 0;
        private static int FIELD = 0;
        private static int WINNING_CONDITION = 3;
        private static bool AGAINST_COMPUTER = false;
        private static string MARK_X = "\x1b[31;40mX\x1b[0m ";
        private static string MARK_O = "\x1b[32;40mO\x1b[0m ";
        static string[,] board;

        static void Main(string[] args)
        {
            Init();
            Run();
        }

        static void Restart()
        {
            Console.WriteLine("Nyomj egy 'R' hogy újra kezd a játékot.");
            string exitOrRestart = Console.ReadLine();
            if (exitOrRestart == "r" || exitOrRestart == "R")
            {
                Init();
                Run();
            }

        }

        static void Run()
        {
            while (!BaseChecker(WINNING_CONDITION) && counter < Math.Pow(FIELD,2))
            {
                Console.Write("Játékos {0}: Válassz egy mezőt!", ACTUALPLAYER);
                if (AGAINST_COMPUTER)
                {
                    if(ACTUALPLAYER == 1)
                    {
                        string input = Console.ReadLine();
                        Step(input);
                    }
                    else
                    {
                        Step(PCStep());
                    }
                }
                else
                {
                string input = Console.ReadLine();
                Step(input);
                }
            }

            if (BaseChecker(WINNING_CONDITION))
            {
                Console.WriteLine("Játékos {0} nyert!", ACTUALPLAYER);
                Restart();
            }

            if (counter == Math.Pow(FIELD,2))
            {
                Console.WriteLine("Döntetlen!");
                Restart();
            }
        }

        

        static void Init()
        {
            char yourCharAscii;
            bool isCorrectInput;
            char[] rightInputs = { 'Y', 'y', 'N', 'n' };
            do
            {
                Console.WriteLine("Számítógép ellen akarsz játszani? Nyomj egy 'Y'-t ha igen 'N'-t ha nem.");
                isCorrectInput = char.TryParse(Console.ReadLine(), out yourCharAscii);
            } while (!rightInputs.Contains(yourCharAscii) && !isCorrectInput);
          
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
            isWinningCondCorrect = WINNING_CONDITION <= FIELD && WINNING_CONDITION > 2 ? true : false;
            if (isWinningCondCorrect)
            {
                CreateBoard(FIELD);
                ACTUALPLAYER = 1;
                counter = 0;
                DrawTable();
            }
            else
            {
                Console.WriteLine("Hiba az adatok bevitelébe. Próbáld újra.");
                Init();
            }
           
        }

        /// <summary>
        ///  Feltölti a táblát (board) üres mezőkkel (0-tól növekvő számokkal).
        /// </summary>
        /// <param name="numb">Mező mérete pl. 3-> 3x3 mező</param>
        static void CreateBoard(int numb)
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

        /// <summary>
        /// A mindenkori táblának (board) megfelelően vizualizálja az adatokat.
        /// </summary>
        static void DrawTable()
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
                Console.WriteLine("\n"+DrawRow(FIELD));
            }
        }

        /// <summary>
        /// Sorok közé vonalat "rajzol" a konzolra a könnyebb áttekinthetőség érdekében.
        /// </summary>
        /// <param name="numberOfRow">Tábla sorainak száma</param>
        /// <returns></returns>
        static string DrawRow(int numberOfRow) {
            string line="";
            for (int i = 0; i < numberOfRow; i++)
            {
                line += " - - -";
            }

            return line;
        }

        /// <summary>
        /// A paraméterként átadott információk alapján módosítja az adattáblát (board) és újrarajzoltatja azt.
        /// </summary>
        /// <param name="coord">"Koordináta" a 2 dimenziós tömbhöz. [0] = sor, [1] = hanyadik elem</param>
        /// <param name="value">"X" vagy "0" lehet</param>
        static void ModifyOneItem(int[] coord, string value)
        {
            board[coord[0], coord[1]] = value;
            Console.Clear();
            counter++;
            DrawTable();
        }

        /// <summary>
        /// 1. Validálja a beírt értéket 
        ///     - átkonvertálható integer adattípusba
        ///     - 0 vagy annál nagyobb de nem nagyobb mint a mező maximális értéke
        ///     - Nem "foglalt" mező
        /// 2.  Meghívja a ModifyOneItem függvényt
        /// 3.  Ha nincs találat akkor meghívja a ChangePlayer függvényt
        /// 
        /// Ha hibás a beviteli érték hibaüzenetet ad.
        /// </summary>
        /// <param name="input">Felhasználó által begépelt - validálatlan - érték</param>
        static void Step(string input)
        {
            bool isInputCorrect = int.TryParse(input, out int inputNumb);
            double maxInput = Math.Pow(FIELD,2);
            if (isInputCorrect && inputNumb >= 0 && inputNumb < maxInput && CheckTheField(GetTheCoord(inputNumb)))
            {
                ModifyOneItem(GetTheCoord(inputNumb), GetTheActualTicTac());
                if (!BaseChecker(WINNING_CONDITION)) ChangePlayer();
            }
            else
            {
                Console.WriteLine("Üres mezőt válassz!");
            }
        }

        /// <summary>
        ///  Megvizsgálja, hogy az adott "koordináta" nem foglalt-e egy másik játékos által.
        /// </summary>
        /// <param name="coord">"Koordináta" a 2 dimenziós tömbhöz. [0] = sor, [1] = hanyadik elem</param>
        /// <returns></returns>
        static bool CheckTheField(int[] coord)
        {
            string actualField = board[coord[0], coord[1]];
            return (actualField != MARK_X && actualField != MARK_O) ? true : false;
        }

        static void ChangePlayer()
        {
            ACTUALPLAYER = ACTUALPLAYER == 1 ? 2 : 1;
        }

        /// <summary>
        /// Visszaadja az adott játékos jelét. X/O
        /// </summary>
        /// <returns></returns>
        static string GetTheActualTicTac()
        {
            return ACTUALPLAYER == 1 ? MARK_X : MARK_O;
        }

        /// <summary>
        /// Visszaadja, hogy az adott sorszámű mező a kétdimenziós tömbben hol található.
        /// </summary>
        /// <param name="inputNumb"></param>
        /// <returns></returns>
        static int[] GetTheCoord(int inputNumb)
        {
            var row = Math.DivRem(inputNumb, FIELD, out int column);
            return new int[] { row, column };
        }

        /// <summary>
        /// Visszaadja a számítógép által választott mezőt.
        /// </summary>
        /// <returns></returns>
        static string PCStep()
        {
            Random dice = new Random();
            int nextStep = dice.Next(0,(FIELD*FIELD-1));
            return nextStep.ToString();

        }

        /// <summary>
        /// Végig iterál az adattáblát. Ha a nyerési feltételeknek megfelelő számú X / O jel van egymás mellett, horizontálisan, vertikálisan és átlósan, akkor 'true' értékkel tér vissza.
        /// </summary>
        /// <param name="winningCondition">Ennyi jelnek kell egymás után következnie, hogy győzzön a játékos.</param>
        /// <returns></returns>
        static bool BaseChecker(int winningCondition) {
            /*Ha a nyerési feltétel és a mező nagysága eltérő. Pl. 6x6 a mező de 3 db nyerési feltétel,
            akkor a 6x6-os mezőt 3x3-as négyzetekre kell bontani,
            és azokon kell vizsgálni a nyerési feltétel teljesülését. Ennek a logikáját tartalmazza az alábbi kód.*/
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

        /// <summary>
        /// Az adattábla egy bizonyos részében ellenőrzi a nyerési feltétel teljesülését.
        /// Ha a mező és a nyerési feltétel egyenlő, akkor a startingRow és startingColumn paraméterek egyaránt nullák
        /// Tehát nem bontja fel részekre az adattáblát.
        /// </summary>
        /// <param name="board">2 dimenziós tömb</param>
        /// <param name="startingRow">Kezdő sora az adattáblának ('board'), ahonnan a vizsgálatot kezdi.</param>
        /// <param name="startingColumn">Az adattábla ('board') adott sor, hanyadik oszlopától kezdje a vizsgálatot kezdi.</param>
        /// <returns></returns>
        static bool Checker(string[,] board, int startingRow, int startingColumn)
        {
            int counter = 0;
            //Átlós ellenőrzés 1
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

            //Átlós ellenőrzés 2
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

                //Vízszintes ellenőrzés
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
                //Függőleges ellenőrzés
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