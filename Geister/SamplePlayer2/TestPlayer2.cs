using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Geister.GameSystem;
using Geister.GameInformation;
using Geister.SamplePlayer2;



//競合を防ぐため，名前空間をユニークなものに変更してプログラムを作成してください．
namespace Geister.SamplePlayer2 {
    /// <summary>
    /// 1日クオリティ
    /// 
    /// アルゴリズム
    /// 現状態と移動後状態で適当にスコアリングして移動法を決めるサンプルプレイヤー
    /// 初期配置ランダム
    /// goodを取られないように移動
    /// 敵に取られず敵の駒を取れる場合にはすぐ取る
    /// 敵の取れる駒は取る
    /// 
    /// goodは少しずつはゴールに近づいていく
    /// ゴールにたどり着いたらゴールに入る
    /// 
    /// </summary>
    /// 
    class TestPlayer2 : AbstractPlayer {

        FieldObject myID;
        FieldObject enemyID;

        FieldObject[,] boardState;
        /// <summary>
        /// 以下の2つのコンストラクタ内でゴーストの初期配置を設定してください．
        public TestPlayer2() : base() {
            SetInitialPlacement(RandPlace());
        }

        public TestPlayer2(string name) : base() {
            this.name = name;
            SetInitialPlacement(RandPlace());

        }
        /// <summary>
        /// 初期位置をランダム配置する
        /// </summary>
        /// <returns></returns>
        private GhostAttribute[,] RandPlace() {
            GhostAttribute[] randplace = new GhostAttribute[8] { GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil ,
                                                      GhostAttribute.good, GhostAttribute.good, GhostAttribute.good, GhostAttribute.good };

            System.Random rng = new System.Random();
            int n = randplace.Length;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                GhostAttribute tmp = randplace[k];
                randplace[k] = randplace[n];
                randplace[n] = tmp;
            }
            return new GhostAttribute[2, 4] {
                { randplace[0],randplace[1],randplace[2],randplace[3] },
                { randplace[4],randplace[5],randplace[6],randplace[7] }
            };

        }

        /// <summary>
        /// ゴーストがランダムに動くサンプルプログラム
        /// </summary>
        /// <returns></returns>
        public override Move GetMove() {

            boardState = GetBoardState();
            myID = GetMyPlayerID();

            if (myID.Equals(FieldObject.P1))
            {
                enemyID = FieldObject.P2;
            }
            else if(myID.Equals(enemyID))
            {
                enemyID = FieldObject.P1;
            }


            List<Ghost> glist = GetMyGhostList().OrderBy(j => Guid.NewGuid()).ToList();
            List<GhostMove> gmlist = new List<GhostMove>();
            gmlist.Add(GhostMove.Down);
            gmlist.Add(GhostMove.Left);
            gmlist.Add(GhostMove.Right);
            gmlist.Add(GhostMove.Up);


            Move m = null;
            GhostMoveSet gms = null;

            int point = -500000;

            for (int i = 0; i < GetMyGhostList().Count; i++) {
                Ghost g = glist[i];

                foreach (GhostMove gm in gmlist) {
                    gms = new GhostMoveSet(g, gm);
                    if (IsMovable(g.P, gm)) {

                        gms.next = NextPosition(g.P, gm);
                        gms.catchable = isCatchable(gms.next);
                        gms.next_catched = isCatched(gms.next);
                        gms.current_catched = isCatched(gms.ghost.P);
                        gms.culPoint();

                        System.Console.WriteLine("serected  {0},{1},{2}", gms.ghost.P.Row, gms.ghost.P.Col, gms.direction);
                        System.Console.WriteLine("serected  {0},{1},{2}", gms.catchable, gms.next_catched, gms.point);

                        if (gms.point > point) {
                            point = gms.point;
                            m = new Move(g.P, gm);
                        }
                    }
                }

            }



            return m;
        }




        /// 移動した場合に相手の駒をとれるか判定
        /// </summary>
        /// <returns></returns>
        private Boolean isCatchable(Position p) {
            List<Position> enemyGhostList = GetGhostPositionList(enemyID);
            foreach (Position enemyGhost in enemyGhostList) {
                if (p == enemyGhost) return true;
            }
            return false;
        }

        /// <summary>
        /// 移動した場合次のターンで取られてしまうか判定
        /// </summary>
        /// <returns></returns>
        private Boolean isCatched(Position next) {
            List<Position> enemyGhostList = GetGhostPositionList(enemyID);
            foreach (Position enemyGhost in enemyGhostList) {
                if (next.Row == enemyGhost.Row) {
                    if (next.Col == enemyGhost.Col + 1 || next.Col == enemyGhost.Col - 1) {
                        return true;
                    }
                }
                if (next.Col == enemyGhost.Col) {
                    if (next.Row == enemyGhost.Row + 1 || next.Row == enemyGhost.Row - 1) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 次のポジションを計算する　判定はしない
        /// </summary>
        /// <param name="current"></param>
        /// <param name="gm"></param>
        /// <returns></returns>
        public Position NextPosition(Position current, GhostMove direction) {
            Position next_p = current.Clone();
            if (direction == GhostMove.Down) next_p.Row++;
            if (direction == GhostMove.Up) next_p.Row--;
            if (direction == GhostMove.Right) next_p.Col++;
            if (direction == GhostMove.Left) next_p.Col--;
            return next_p;
        }


        /// <summary>
        /// 指定したゴーストが移動可能であるか
        /// </summary>
        /// <param name="p"></param>
        /// <param name="gm"></param>
        /// <returns></returns>
        private Boolean IsMovable(Position p, GhostMove gm) {
            if (Exists(p)) {
                if (gm == GhostMove.Down) {
                    if (p.Row == 5) {
                        return false;
                    }

                    if (boardState[p.Row + 1, p.Col] == GetMyPlayerID()) {
                        return false;
                    }
                } else if (gm == GhostMove.Left) {
                    if (p.Col == 0) {
                        return false;
                    }

                    if (boardState[p.Row, p.Col - 1] == GetMyPlayerID()) {
                        return false;
                    }
                } else if (gm == GhostMove.Right) {
                    if (p.Col == 5) {
                        return false;
                    }

                    if (boardState[p.Row, p.Col + 1] == GetMyPlayerID()) {
                        return false;
                    }
                } else if (gm == GhostMove.Up) {
                    if (p.Row == 0) {
                        if (p.Col == 0 || p.Col == 5) {
                            if (GetMyGhostAttribute(p) == GhostAttribute.good) {
                                return true;
                            }
                        }
                        return false;
                    }

                    if (boardState[p.Row - 1, p.Col] == GetMyPlayerID()) {
                        return false;
                    }
                }
                return true;
            }





            return false;
        }

        /// <summary>
        /// 指定したマスにゴーストが存在するか
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Boolean Exists(Position p) {
            foreach (Ghost g in GetMyGhostList()) {
                if (g.P == p) {
                    return true;
                }
            }
            return false;

        }






    }
}
