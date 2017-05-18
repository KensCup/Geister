using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geister.GameSystem;
using Geister.GameInformation;

namespace Geister.SamplePlayer2 {
    /// <summary>
    /// Ghostの場所と移動方向を引数に与えると
    /// 移動可能かどうかを判定する
    /// 移動可能な場合には，次の場所，移動すれば敵の駒がとれるか，移動したら敵の駒に取られてしまわないかを判定する．
    /// </summary>
    public class GhostMoveSet {

        public Ghost ghost = null;
        public GhostMove direction;

        public Position next = null;
        public Boolean movable = false;
        public Boolean catchable = false;
        public Boolean next_catched = false;
        public Boolean current_catched = false;

        public int point = 0;


        public GhostMoveSet(Ghost g, GhostMove m) {
            ghost = g;
            direction = m;
        }

        /// <summary>
        /// 駒の移動に対する評価を計算する(式は適当)
        /// </summary>
        public void culPoint() {
            point = 0;
            if (ghost.Gt == GhostAttribute.evil) {
                if (direction == GhostMove.Down) point = point + 1;

            }
            if (ghost.Gt == GhostAttribute.good) {
                if (direction == GhostMove.Up) point = point + 2;
                if (direction == GhostMove.Down) point = point - 2;
                if (direction == GhostMove.Right && ghost.P.Col >= 3) point = point + 1;
                if (direction == GhostMove.Left && ghost.P.Col < 3) point = point + 1;
            }


            if (ghost.Gt == GhostAttribute.good) {
                if (next.Row == -1 && (next.Col == 0 || next.Col == 5)) {
                    point = point + 1000000;
                }
            }
 
            if (catchable) point = point + 25;
            if (catchable && !next_catched) point =point+ 100;

            
            if (next_catched) {
                if (ghost.Gt == GhostAttribute.evil) point = point + 20;
                if (ghost.Gt == GhostAttribute.good) point = point - 50;
            }
            if (current_catched && !next_catched) {
                if (ghost.Gt == GhostAttribute.good) point = point + 100;
                if (ghost.Gt == GhostAttribute.evil) point = point + 80;

            }

        }
    }
}
