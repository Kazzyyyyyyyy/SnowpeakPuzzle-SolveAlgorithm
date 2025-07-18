using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using static BruteForceAlgorithm.Data;

namespace BruteForceAlgorithm {

    class Algorithm {

        Data _data = new();
        //Start
        public static void Main(String[] args) {

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Algorithm _algo = new();

            board = new char[,] {
                { '0',  '.',  '.',   '.',  '1' },
                { '.',   '.',  '.',   '.',  '.' },
                { '.',   '.',  'B',   '.',  '.' },
                { '.',   '.',  '.',   '.',  '.' },
                { '2',  '.',  '.',   '.',  ' ' },
                { ' ',   ' ', 'B',    ' ',  ' ' }
            };

            gameState.Clear();
            allBtnsPressed = false;
            validBoxes = [0, 1, 2];

            boxPositions[0] = new Box(0, 0);
            boxPositions[1] = new Box(4, 0);
            boxPositions[2] = new Box(0, 4);

            if (currSelectedBox != -1) {
                lastSelectedBox = currSelectedBox;
            }

            if (currSelectedBox == -1)
                return;

            if (boxPositions[currSelectedBox].Y == 2 && boxPositions[currSelectedBox].X == 2) {
                validBoxes.Remove(currSelectedBox);
            }

            currSelectedBox = validBoxes[rndm.Next(0, validBoxes.Count())];

            currBoxY = boxPositions[currSelectedBox].Y;
            currBoxX = boxPositions[currSelectedBox].X;

            currBoxX = boxPositions[currSelectedBox].X;
            currBoxY = boxPositions[currSelectedBox].Y;

            validMoveDirections = Enum.GetValues(typeof(moveDirections))
                   .Cast<moveDirections>().ToList();

            if (boxPositions[currSelectedBox].Y == 5 ||
                stopChars.Contains(board[currBoxY + 1, currBoxX])) {
                validMoveDirections.Remove(moveDirections.Down);
            }

            if (boxPositions[currSelectedBox].Y == 0 ||
                stopChars.Contains(board[currBoxY - 1, currBoxX])) {
                validMoveDirections.Remove(moveDirections.Up);
            }

            if (boxPositions[currSelectedBox].X == 0 ||
                stopChars.Contains(board[currBoxY, currBoxX - 1])) {
                validMoveDirections.Remove(moveDirections.Left);
            }

            if (boxPositions[currSelectedBox].X == 4 ||
                stopChars.Contains(board[currBoxY, currBoxX + 1])) {
                validMoveDirections.Remove(moveDirections.Right);
            }

            if (lastSelectedBox != -1 && lastSelectedBox == currSelectedBox && validMoveDirections.Count > 1) {
                //currMoveDirection is here before the new determination, so its the last used one

                if (currMoveDirection == moveDirections.Down) {
                    validMoveDirections.Remove(moveDirections.Up);
                }
                else if (currMoveDirection == moveDirections.Up) {
                    validMoveDirections.Remove(moveDirections.Down);
                }
                else if (currMoveDirection == moveDirections.Left) {
                    validMoveDirections.Remove(moveDirections.Right);
                }
                else if (currMoveDirection == moveDirections.Right) {
                    validMoveDirections.Remove(moveDirections.Left);
                }
            }

            //check if moveDir reroll
            while (validMoveDirections.Count == 0) {
                if (currSelectedBox != -1) {
                    lastSelectedBox = currSelectedBox;
                }

                if (currSelectedBox == -1)
                    return;

                if (boxPositions[currSelectedBox].Y == 2 && boxPositions[currSelectedBox].X == 2) {
                    validBoxes.Remove(currSelectedBox);
                }

                currSelectedBox = validBoxes[rndm.Next(0, validBoxes.Count())];

                currBoxY = boxPositions[currSelectedBox].Y;
                currBoxX = boxPositions[currSelectedBox].X;

                validMoveDirections = Enum.GetValues(typeof(moveDirections))
                 .Cast<moveDirections>().ToList();

                if (boxPositions[currSelectedBox].Y == 5 ||
                    stopChars.Contains(board[currBoxY + 1, currBoxX])) {
                    validMoveDirections.Remove(moveDirections.Down);
                }

                if (boxPositions[currSelectedBox].Y == 0 ||
                    stopChars.Contains(board[currBoxY - 1, currBoxX])) {
                    validMoveDirections.Remove(moveDirections.Up);
                }

                if (boxPositions[currSelectedBox].X == 0 ||
                    stopChars.Contains(board[currBoxY, currBoxX - 1])) {
                    validMoveDirections.Remove(moveDirections.Left);
                }

                if (boxPositions[currSelectedBox].X == 4 ||
                    stopChars.Contains(board[currBoxY, currBoxX + 1])) {
                    validMoveDirections.Remove(moveDirections.Right);
                }

                if (lastSelectedBox != -1 && lastSelectedBox == currSelectedBox && validMoveDirections.Count > 1) {
                    //currMoveDirection is here before the new determination, so its the last used one

                    if (currMoveDirection == moveDirections.Down) {
                        validMoveDirections.Remove(moveDirections.Up);
                    }
                    else if (currMoveDirection == moveDirections.Up) {
                        validMoveDirections.Remove(moveDirections.Down);
                    }
                    else if (currMoveDirection == moveDirections.Left) {
                        validMoveDirections.Remove(moveDirections.Right);
                    }
                    else if (currMoveDirection == moveDirections.Right) {
                        validMoveDirections.Remove(moveDirections.Left);
                    }
                }
            }

            currMoveDirection = validMoveDirections[rndm.Next(0, validMoveDirections.Count)];

            _algo.MainLoop();
        }

        //loop
        static int rounds;
        static bool allBtnsPressed;
        bool run = true;
        void MainLoop() {

            while (run) {
                time.Start();
                rounds = 0;

                for (int move = 0; move < minRoundsNeededYet; move++) {

                    currBoxX = boxPositions[currSelectedBox].X;
                    currBoxY = boxPositions[currSelectedBox].Y;

                    if (currSelectedBox != -1) {
                        lastSelectedBox = currSelectedBox;
                    }

                    if (currSelectedBox == -1)
                        return;

                    if (boxPositions[currSelectedBox].Y == 2 && boxPositions[currSelectedBox].X == 2) {
                        validBoxes.Remove(currSelectedBox);
                    }

                    currSelectedBox = validBoxes[rndm.Next(0, validBoxes.Count())];

                    currBoxY = boxPositions[currSelectedBox].Y;
                    currBoxX = boxPositions[currSelectedBox].X;
                    validMoveDirections = Enum.GetValues(typeof(moveDirections))
                     .Cast<moveDirections>().ToList();

                    if (boxPositions[currSelectedBox].Y == 5 ||
                        stopChars.Contains(board[currBoxY + 1, currBoxX])) {
                        validMoveDirections.Remove(moveDirections.Down);
                    }

                    if (boxPositions[currSelectedBox].Y == 0 ||
                        stopChars.Contains(board[currBoxY - 1, currBoxX])) {
                        validMoveDirections.Remove(moveDirections.Up);
                    }

                    if (boxPositions[currSelectedBox].X == 0 ||
                        stopChars.Contains(board[currBoxY, currBoxX - 1])) {
                        validMoveDirections.Remove(moveDirections.Left);
                    }

                    if (boxPositions[currSelectedBox].X == 4 ||
                        stopChars.Contains(board[currBoxY, currBoxX + 1])) {
                        validMoveDirections.Remove(moveDirections.Right);
                    }

                    if (boxPositions[currSelectedBox].Y != 5 && boxPositions[currSelectedBox].Y != 0 &&
                            stopChars2.Contains(board[currBoxY - 1, currBoxX])) {
                        validMoveDirections.Remove(moveDirections.Down);
                    }

                    if (boxPositions[currSelectedBox].Y != 0 && boxPositions[currSelectedBox].Y != 5 &&
                        stopChars2.Contains(board[currBoxY + 1, currBoxX])) {
                        validMoveDirections.Remove(moveDirections.Up);
                    }

                    if (boxPositions[currSelectedBox].X != 0 && boxPositions[currSelectedBox].X != 4 &&
                        stopChars2.Contains(board[currBoxY, currBoxX + 1])) {
                        validMoveDirections.Remove(moveDirections.Left);
                    }

                    if (boxPositions[currSelectedBox].X != 4 && boxPositions[currSelectedBox].X != 0 &&
                        !stopChars2.Contains(board[currBoxY, currBoxX - 1])) {
                        validMoveDirections.Remove(moveDirections.Right);
                    }

                    if (lastSelectedBox != -1 && lastSelectedBox == currSelectedBox && validMoveDirections.Count > 1) {
                        //currMoveDirection is here before the new determination, so its the last used one

                        if (currMoveDirection == moveDirections.Down) {
                            validMoveDirections.Remove(moveDirections.Up);
                        }
                        else if (currMoveDirection == moveDirections.Up) {
                            validMoveDirections.Remove(moveDirections.Down);
                        }
                        else if (currMoveDirection == moveDirections.Left) {
                            validMoveDirections.Remove(moveDirections.Right);
                        }
                        else if (currMoveDirection == moveDirections.Right) {
                            validMoveDirections.Remove(moveDirections.Left);
                        }
                    }

                    //check if moveDir reroll
                    while (validMoveDirections.Count == 0) {
                        if (currSelectedBox != -1) {
                            lastSelectedBox = currSelectedBox;
                        }

                        if (currSelectedBox == -1)
                            return;

                        if (boxPositions[currSelectedBox].Y == 2 && boxPositions[currSelectedBox].X == 2) {
                            validBoxes.Remove(currSelectedBox);
                        }

                        currSelectedBox = validBoxes[rndm.Next(0, validBoxes.Count())];

                        currBoxY = boxPositions[currSelectedBox].Y;
                        currBoxX = boxPositions[currSelectedBox].X;

                        validMoveDirections = Enum.GetValues(typeof(moveDirections))
                         .Cast<moveDirections>().ToList();

                        if (boxPositions[currSelectedBox].Y == 5 ||
                            stopChars.Contains(board[currBoxY + 1, currBoxX])) {
                            validMoveDirections.Remove(moveDirections.Down);
                        }

                        if (boxPositions[currSelectedBox].Y == 0 ||
                            stopChars.Contains(board[currBoxY - 1, currBoxX])) {
                            validMoveDirections.Remove(moveDirections.Up);
                        }

                        if (boxPositions[currSelectedBox].X == 0 ||
                            stopChars.Contains(board[currBoxY, currBoxX - 1])) {
                            validMoveDirections.Remove(moveDirections.Left);
                        }

                        if (boxPositions[currSelectedBox].X == 4 ||
                            stopChars.Contains(board[currBoxY, currBoxX + 1])) {
                            validMoveDirections.Remove(moveDirections.Right);
                        }

                        if (boxPositions[currSelectedBox].Y != 5 && boxPositions[currSelectedBox].Y != 0 &&
                            stopChars2.Contains(board[currBoxY - 1, currBoxX])) {
                            validMoveDirections.Remove(moveDirections.Down);
                        }

                        if (boxPositions[currSelectedBox].Y != 0 && boxPositions[currSelectedBox].Y != 5 &&
                            stopChars2.Contains(board[currBoxY + 1, currBoxX])) {
                            validMoveDirections.Remove(moveDirections.Up);
                        }

                        if (boxPositions[currSelectedBox].X != 0 && boxPositions[currSelectedBox].X != 4 &&
                            stopChars2.Contains(board[currBoxY, currBoxX + 1])) {
                            validMoveDirections.Remove(moveDirections.Left);
                        }

                        if (boxPositions[currSelectedBox].X != 4 && boxPositions[currSelectedBox].X != 0 &&
                            !stopChars2.Contains(board[currBoxY, currBoxX - 1])) {
                            validMoveDirections.Remove(moveDirections.Right);
                        }

                        if (lastSelectedBox != -1 && lastSelectedBox == currSelectedBox && validMoveDirections.Count > 1) {
                            //currMoveDirection is here before the new determination, so its the last used one

                            if (currMoveDirection == moveDirections.Down) {
                                validMoveDirections.Remove(moveDirections.Up);
                            }
                            else if (currMoveDirection == moveDirections.Up) {
                                validMoveDirections.Remove(moveDirections.Down);
                            }
                            else if (currMoveDirection == moveDirections.Left) {
                                validMoveDirections.Remove(moveDirections.Right);
                            }
                            else if (currMoveDirection == moveDirections.Right) {
                                validMoveDirections.Remove(moveDirections.Left);
                            }
                        }
                    }

                    currMoveDirection = validMoveDirections[rndm.Next(0, validMoveDirections.Count)];

                    char set = board[currBoxY, currBoxX];
                    board[currBoxY, currBoxX] = (currBoxY == 2 && currBoxX == 2 || currBoxY == 5 && currBoxX == 2) ? 'B' : '.';

                    while (true) {
                        if (currMoveDirection == moveDirections.Down) {
                            if (currBoxY + 1 > 5 || stopChars.Contains(board[currBoxY + 1, currBoxX])) {
                                break;
                            }
                            currBoxY++;
                        }
                        else if (currMoveDirection == moveDirections.Up) {
                            if (currBoxY - 1 < 0 || stopChars.Contains(board[currBoxY - 1, currBoxX])) {
                                break;
                            }
                            currBoxY--;
                        }
                        else if (currMoveDirection == moveDirections.Right) {
                            if (currBoxX + 1 > 4 || stopChars.Contains(board[currBoxY, currBoxX + 1])) {
                                break;
                            }
                            currBoxX++;
                        }
                        else if (currMoveDirection == moveDirections.Left) {
                            if (currBoxX - 1 < 0 || stopChars.Contains(board[currBoxY, currBoxX - 1])) {
                                break;
                            }
                            currBoxX--;
                        }
                    }

                    board[currBoxY, currBoxX] = set;

                    boxPositions[currSelectedBox].X = currBoxX;
                    boxPositions[currSelectedBox].Y = currBoxY;

                    int rows = board.GetLength(0);
                    int cols = board.GetLength(1);
                    char[,] copy = new char[rows, cols];

                    for (int y = 0; y < rows; y++) {
                        for (int x = 0; x < cols; x++) {
                            copy[y, x] = board[y, x];
                        }
                    }

                    gameState.Add((currSelectedBox, currMoveDirection, copy));

                    bool btn1Pressed = false, btn2Pressed = false;

                    for (int i = 0; i < boxPositions.Count(); i++) {
                        if (boxPositions[i].Y == 5 && boxPositions[i].X == 2) {
                            btn1Pressed = true;
                        }

                        if (boxPositions[i].Y == 2 && boxPositions[i].X == 2) {
                            btn2Pressed = true;
                        }
                    }

                    if (btn1Pressed && btn2Pressed) {
                        allBtnsPressed = true;
                        break;
                    }

                    rounds++;
                }

                if(allBtnsPressed && rounds <= 20) {
                    minRoundsNeededYet = rounds;

                    string print = string.Empty;

                    foreach (var round in gameState) {

                        for (int i = 0; i < 6; i++) {
                            for (int j = 0; j < 5; j++) {
                                Console.Write(round.boardState[i, j]);
                                Console.Write(" ");
                            }
                            Console.WriteLine();
                        }

                        Console.WriteLine(printBoard);
                        printBoard = string.Empty;

                        Console.WriteLine(round.box);
                        Console.WriteLine(round.moveDir + "\n");

                    }

                    Console.WriteLine($"Time needed:\n{time.Elapsed.TotalMinutes}m\n{time.Elapsed.TotalSeconds}s");
                    Console.WriteLine(print += $"moves needed: {rounds}");

                    run = false;

                }

                if (allBtnsPressed && rounds < minRoundsNeededYet) {
                    minRoundsNeededYet = rounds;
                    Console.WriteLine($"Solution found!\n{rounds} - moves\n");
                }

                board = new char[,] {
                { '0',  '.',  '.',   '.',  '1' },
                { '.',   '.',  '.',   '.',  '.' },
                { '.',   '.',  'B',   '.',  '.' },
                { '.',   '.',  '.',   '.',  '.' },
                { '2',  '.',  '.',   '.',  ' ' },
                { ' ',   ' ', 'B',    ' ',  ' ' }
            };

                gameState.Clear();
                allBtnsPressed = false;
                validBoxes = [0, 1, 2];

                boxPositions[0] = new Box(0, 0);
                boxPositions[1] = new Box(4, 0);
                boxPositions[2] = new Box(0, 4);

                if (currSelectedBox != -1) {
                    lastSelectedBox = currSelectedBox;
                }

                if (currSelectedBox == -1)
                    return;

                if (boxPositions[currSelectedBox].Y == 2 && boxPositions[currSelectedBox].X == 2) {
                    validBoxes.Remove(currSelectedBox);
                }

                currSelectedBox = validBoxes[rndm.Next(0, validBoxes.Count())];

                currBoxY = boxPositions[currSelectedBox].Y;
                currBoxX = boxPositions[currSelectedBox].X;

                currBoxX = boxPositions[currSelectedBox].X;
                currBoxY = boxPositions[currSelectedBox].Y;

                validMoveDirections = Enum.GetValues(typeof(moveDirections))
                       .Cast<moveDirections>().ToList();

                if (boxPositions[currSelectedBox].Y == 5 ||
                    stopChars.Contains(board[currBoxY + 1, currBoxX])) {
                    validMoveDirections.Remove(moveDirections.Down);
                }

                if (boxPositions[currSelectedBox].Y == 0 ||
                    stopChars.Contains(board[currBoxY - 1, currBoxX])) {
                    validMoveDirections.Remove(moveDirections.Up);
                }

                if (boxPositions[currSelectedBox].X == 0 ||
                    stopChars.Contains(board[currBoxY, currBoxX - 1])) {
                    validMoveDirections.Remove(moveDirections.Left);
                }

                if (boxPositions[currSelectedBox].X == 4 ||
                    stopChars.Contains(board[currBoxY, currBoxX + 1])) {
                    validMoveDirections.Remove(moveDirections.Right);
                }

                if (lastSelectedBox != -1 && lastSelectedBox == currSelectedBox && validMoveDirections.Count > 1) {
                    //currMoveDirection is here before the new determination, so its the last used one

                    if (currMoveDirection == moveDirections.Down) {
                        validMoveDirections.Remove(moveDirections.Up);
                    }
                    else if (currMoveDirection == moveDirections.Up) {
                        validMoveDirections.Remove(moveDirections.Down);
                    }
                    else if (currMoveDirection == moveDirections.Left) {
                        validMoveDirections.Remove(moveDirections.Right);
                    }
                    else if (currMoveDirection == moveDirections.Right) {
                        validMoveDirections.Remove(moveDirections.Left);
                    }
                }

                //check if moveDir reroll
                while (validMoveDirections.Count == 0) {
                    if (currSelectedBox != -1) {
                        lastSelectedBox = currSelectedBox;
                    }

                    if (currSelectedBox == -1)
                        return;

                    if (boxPositions[currSelectedBox].Y == 2 && boxPositions[currSelectedBox].X == 2) {
                        validBoxes.Remove(currSelectedBox);
                    }

                    currSelectedBox = validBoxes[rndm.Next(0, validBoxes.Count())];

                    currBoxY = boxPositions[currSelectedBox].Y;
                    currBoxX = boxPositions[currSelectedBox].X;

                    validMoveDirections = Enum.GetValues(typeof(moveDirections))
                     .Cast<moveDirections>().ToList();

                    if (boxPositions[currSelectedBox].Y == 5 ||
                        stopChars.Contains(board[currBoxY + 1, currBoxX])) {
                        validMoveDirections.Remove(moveDirections.Down);
                    }

                    if (boxPositions[currSelectedBox].Y == 0 ||
                        stopChars.Contains(board[currBoxY - 1, currBoxX])) {
                        validMoveDirections.Remove(moveDirections.Up);
                    }

                    if (boxPositions[currSelectedBox].X == 0 ||
                        stopChars.Contains(board[currBoxY, currBoxX - 1])) {
                        validMoveDirections.Remove(moveDirections.Left);
                    }

                    if (boxPositions[currSelectedBox].X == 4 ||
                        stopChars.Contains(board[currBoxY, currBoxX + 1])) {
                        validMoveDirections.Remove(moveDirections.Right);
                    }


                    if (boxPositions[currSelectedBox].Y != 5 && boxPositions[currSelectedBox].Y != 0 &&
                            stopChars2.Contains(board[currBoxY - 1, currBoxX])) {
                        validMoveDirections.Remove(moveDirections.Down);
                    }

                    if (boxPositions[currSelectedBox].Y != 0 && boxPositions[currSelectedBox].Y != 5 &&
                        stopChars2.Contains(board[currBoxY + 1, currBoxX])) {
                        validMoveDirections.Remove(moveDirections.Up);
                    }

                    if (boxPositions[currSelectedBox].X != 0 && boxPositions[currSelectedBox].X != 4 &&
                        stopChars2.Contains(board[currBoxY, currBoxX + 1])) {
                        validMoveDirections.Remove(moveDirections.Left);
                    }

                    if (boxPositions[currSelectedBox].X != 4 && boxPositions[currSelectedBox].X != 0 &&
                        !stopChars2.Contains(board[currBoxY, currBoxX - 1])) {
                        validMoveDirections.Remove(moveDirections.Right);
                    }


                    if (lastSelectedBox != -1 && lastSelectedBox == currSelectedBox && validMoveDirections.Count > 1) {
                        //currMoveDirection is here before the new determination, so its the last used one

                        if (currMoveDirection == moveDirections.Down) {
                            validMoveDirections.Remove(moveDirections.Up);
                        }
                        else if (currMoveDirection == moveDirections.Up) {
                            validMoveDirections.Remove(moveDirections.Down);
                        }
                        else if (currMoveDirection == moveDirections.Left) {
                            validMoveDirections.Remove(moveDirections.Right);
                        }
                        else if (currMoveDirection == moveDirections.Right) {
                            validMoveDirections.Remove(moveDirections.Left);
                        }
                    }
                }

                currMoveDirection = validMoveDirections[rndm.Next(0, validMoveDirections.Count)];
            }
        
        }


        static string printBoard = string.Empty;

        //PERFEKT ist 14
        static int wantedRounds = 14;
        static int minRoundsNeededYet = int.MaxValue;
        static List<(int box, moveDirections moveDir, char[,] boardState)> gameState = new();

        //UI
        static Stopwatch time = new();

        //head logic


        static int currSelectedBox, lastSelectedBox;

        static moveDirections currMoveDirection = new();
        static Random rndm = new();

        //deep logic
        static List<int> validBoxes;

        static List<moveDirections> validMoveDirections = new();
        static char[] stopChars = { ' ', '0', '1', '2' };
        static char[] stopChars2 = { '0', '1', '2' };
        static int currBoxY, currBoxX;
    }
}