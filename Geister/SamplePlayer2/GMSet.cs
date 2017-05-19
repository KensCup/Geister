using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geister.GameSystem;
using Geister.GameInformation;

namespace Geister.SamplePlayer2
{
    /// <summary>
    /// Ghostの場所と移動方向を引数に与えると
    /// 移動可能かどうかを判定する
    /// 移動可能な場合には，次の場所，移動すれば敵の駒がとれるか，移動したら敵の駒に取られてしまわないかを判定する．
    /// </summary>
    public class GhostMoveSet
    {

        public Ghost ghost = null;
        public GhostMove direction;

        public Position next = null;
        public Boolean movable = false;
        public Boolean catchable = false;
        public Boolean next_catched = false;
        public Boolean current_catched = false;

        public int point = 0;


        public GhostMoveSet(Ghost g, GhostMove m)
        {
            ghost = g;
            direction = m;
        }

        /// <summary>
        /// 駒の移動に対する評価を計算する(式は適当)
        /// </summary>
        public void culPoint()
        {
            point = 0;
            if (current_catched) point = point + 20;

            if (ghost.Gt == GhostAttribute.evil)
            {
                Random cRandom = new System.Random();
                point = point + cRandom.Next(15);
            }

            if (ghost.Gt == GhostAttribute.good)
            {
                if (direction == GhostMove.Up) point = point + 3;
                if (direction == GhostMove.Down)
                    if (ghost.P.Col != 0) point = point - 2;
                    else point = point + 1;
                if (direction == GhostMove.Right && ghost.P.Col >= 3) point = point + 1;
                if (direction == GhostMove.Left && ghost.P.Col < 3) point = point + 1;
            }

            if (ghost.Gt == GhostAttribute.good)
            {
                if (next.Row == -1 && (next.Col == 0 || next.Col == 5))
                {
                    point = point + 1000000;
                }
            }
            if (ghost.Gt == GhostAttribute.evil)
            {

                if (current_catched && next_catched && catchable) point = point + 250;
                else if (current_catched && next_catched && !catchable) point = point + 0;
                else if (current_catched && !next_catched && catchable) point = point + 150;
                else if (current_catched && !next_catched && !catchable) point = point + 0;
                else if (!current_catched && next_catched && catchable) point = point + 200;
                else if (!current_catched && next_catched && !catchable) point = point + 50;
                else if (!current_catched && !next_catched && catchable) point = point + 100;
                else if (!current_catched && !next_catched && !catchable) point = point + 0;

            }
            if (ghost.Gt == GhostAttribute.good)
            {
                point = point + 2;
                if (current_catched && next_catched && catchable) point = point + 50;
                else if (current_catched && next_catched && !catchable) point = point - 50;

                else if (current_catched && !next_catched && catchable) point = point + 250;
                else if (current_catched && !next_catched && !catchable) point = point + 200;

                else if (!current_catched && next_catched && catchable) point = point - 50;
                else if (!current_catched && next_catched && !catchable) point = point - 100;
                else if (!current_catched && !next_catched && catchable) point = point + 100;
                else if (!current_catched && !next_catched && !catchable) point = point + 0;

            }

        }
    }
}
