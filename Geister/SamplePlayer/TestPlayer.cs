﻿﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Geister.GameSystem;
using Geister.GameInformation;

//競合を防ぐため，名前空間をユニークなものに変更してプログラムを作成してください．
namespace Geister.Player
{
    class TestPlayer : AbstractPlayer
    {

        FieldObject[,]  boardState;

        /// <summary>
        /// 以下の2つのコンストラクタ内でゴーストの初期配置を設定してください．
        /// </summary>
        public TestPlayer() : base()
        {
            SetInitialPlacement(new GhostAttribute[2, 4] { { GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil },
                                                      { GhostAttribute.good, GhostAttribute.good, GhostAttribute.good, GhostAttribute.good }}
                                                   );

        }

        public TestPlayer(string name) : base()
        {
            this.name = name;
            SetInitialPlacement(new GhostAttribute[2, 4] { { GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil, GhostAttribute.evil },
                                                      { GhostAttribute.good, GhostAttribute.good, GhostAttribute.good, GhostAttribute.good }}
                                                              );
        }

        /// <summary>
        /// ゴーストがランダムに動くサンプルプログラム
        /// </summary>
        /// <returns></returns>
        public override Move GetMove()
        {
            //得られる盤面などの情報は1P，2Pであっても手前側が自分のオバケの初期配置となる盤面で与えられます．
            //また，GhostMoveも手前側が自分のオバケの初期配置となる盤面を基準としています．
            //そのため，自分が1Pである時を想定してプログラムを作成してください．
            boardState = GetBoardState();

            List<Ghost> glist = GetMyGhostList().OrderBy(j => Guid.NewGuid()).ToList();
            List<GhostMove> gmlist = new List<GhostMove>();
            gmlist.Add(GhostMove.Down);
            gmlist.Add(GhostMove.Left);
            gmlist.Add(GhostMove.Right);
            gmlist.Add(GhostMove.Up);
            gmlist = gmlist.OrderBy(j => Guid.NewGuid()).ToList();

            Move m = null;

            for (int i = 0; i < GetMyGhostList().Count; i++)
            {
                Ghost g = glist[i];
                foreach (GhostMove gm in gmlist)
                {
                    if (IsMovable(g.P, gm))
                    {
                        m = new Move(g.P, gm);
                        break;
                    }
                }

            }

             return m;
        }

        /// <summary>
        /// 指定したゴーストが移動可能であるか
        /// </summary>
        /// <param name="p"></param>
        /// <param name="gm"></param>
        /// <returns></returns>
        private Boolean IsMovable(Position p, GhostMove gm)
        {
            if(Exists(p))
            {
                if (gm == GhostMove.Down)
                {
                    if (p.X == 5)
                    {
                        return false;
                    }

                    if (boardState[p.X + 1, p.Y] == GetMyPlayerID())
                    {
                        return false;
                    }
                }
                else if (gm == GhostMove.Left)
                {
                    if (p.Y == 0)
                    {
                        return false;
                    }

                    if (boardState[p.X, p.Y-1] == GetMyPlayerID())
                    {
                        return false;
                    }
                }
				else if (gm == GhostMove.Right)
				{
					if (p.Y == 5)
					{
						return false;
					}

					if (boardState[p.X, p.Y + 1] == GetMyPlayerID())
					{
						return false;
					}
				}
				else if (gm == GhostMove.Up)
				{
					if (p.X == 0)
					{
                        if(p.Y == 0 || p.Y == 5)
                        {
                            return true;
                        }
                        return false;
					}

					if (boardState[p.X-1, p.Y ] == GetMyPlayerID())
					{
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
        private Boolean Exists(Position p)
        {
            foreach(Ghost g in GetMyGhostList())
            {
                if(g.P == p){
                    return true;
                }
            }
            return false;   
            
        }

        


    }
}
