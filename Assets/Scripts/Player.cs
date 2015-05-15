using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Player
    {
        public enum Side
        {
            Blue,
            Red,
            None
        }

        public static Side Opposite(Side side)
        {
            if(side == Side.Blue)
                return Side.Red;
            
            return Side.Blue;
        }

    }
}
