using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceAlgorithm {

    public class Data {
        
        public enum moveDirections { Down = 0, Up, Left, Right };

        public static char[,]? board;

        //y5 / x4

        public static Box[] boxPositions = new Box[3];

    }

    public struct Box {
        public int Y;
        public int X;

        public Box(int x, int y) {
            Y = y;
            X = x;
        }
    }
}
