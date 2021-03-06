﻿using System;
using Geister.GameSystem;
using Geister.Player;
using Geister.SamplePlayer2;


namespace Geister
{
    /// <summary>
    /// ゲームシステムのクラス
    /// </summary>
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        public static void Main()
        {
            App app = new App();

            //引数：(AbstractPlayer ,　AbstractPlayer , int ターン数, int ゲーム進行速度)
            //思考時間は1000msです
            //ゲーム進行速度：1プレイヤーごとの時間を調整
            //例：100 → 進行を100ms遅らせる
            //0 ~ 1000 までの値で調整

            //引数：(AbstractPlayer , AbstractPlayer, AbstractPlayer, AbstractPlayer, int ターン数, int ゲーム進行速度)
            GameManager gameManager = new GameManager(new TestPlayer("a"), new TestPlayer2("b"), 500, 20);

            app.InitializeComponent();
            app.Run(new GeisterUI(gameManager));

        }
    }
}
