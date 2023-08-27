﻿using System;
using System.Collections.Generic;
using System.Drawing;
using LeagueBot;
using LeagueBot.Patterns;
using LeagueBot.Game.Enums;
using LeagueBot.Game.Misc;

namespace LeagueBot
{
    public class Coop : PatternScript
    {

        private Point CastTargetPoint
        {
            get;
            set;
        }

        private Item[] Items = new Item[]
        {
            new Item("Doran Blade",400, false),
            new Item("Berserk",1100, true),
            new Item("Kraken Slayer",3000, true),
            new Item("Blade of the ruined king",3300, true), // <--- Cost when Lost Chapter were bought
            new Item("Guinsoo",3200, true),
            new Item("Phantom Dancer",2800, true),
            new Item("Immortal Shieldbow",3000, true),
        };

        public override bool ThrowException
        {
            get
            {
                return false;
            }
        }

        //Go to your lane 
        //enum lanes
        //Every lanes has its own pixel
        //

        public override void Execute()
        {
            bot.log("Waiting for league of legends process...");

            bot.waitProcessOpen(Constants.GameProcessName);

            bot.waitUntilProcessBounds(Constants.GameProcessName, 1040, 800);

            bot.wait(200);

            bot.log("Waiting for game to load.");

            bot.bringProcessToFront(Constants.GameProcessName);
            bot.centerProcess(Constants.GameProcessName);

            game.waitUntilGameStart();

            bot.log("Game Started");

            bot.bringProcessToFront(Constants.GameProcessName);
            bot.centerProcess(Constants.GameProcessName);

            bot.wait(3000);

            if (game.getSide() == SideEnum.Blue)
            {
                CastTargetPoint = new Point(1084, 398);
                bot.log("We are blue side !");
            }
            else
            {
                CastTargetPoint = new Point(644, 761);
                bot.log("We are red side !");
            }

            game.player.upgradeSpellOnLevelUp();

            OnSpawnJoin();

            bot.log("Playing...");

            GameLoop();

            this.End();
        }
        private void BuyItems()
        {
            int golds = game.player.getGolds();

            game.shop.toogle();
            bot.wait(5000);
            foreach (Item item in Items)
            {
                if (item.Cost > golds)
                {
                    break;
                }
                if (!item.Buyed)
                {
                    game.shop.searchItem(item.Name);

                    game.shop.buySearchedItem();

                    item.Buyed = true;

                    golds -= item.Cost;
                    bot.wait(100);
                }
            }

            game.shop.toogle();

        }
        private void CheckBuyItems()
        {
            int golds = game.player.getGolds();

            foreach (Item item in Items)
            {
                //If item too expensive check components
                if (item.Cost > golds)
                {
                    break;
                }
                if (!item.Buyed)
                {
                    game.player.recall();
                    bot.wait(10000);
                    if (game.player.getManaPercent() == 1)
                    {
                        OnSpawnJoin();

                    }
                    

                }
            }


        }

        private void GameLoop()
        {
            //Checks level
            int level = game.player.getLevel();

            //Dead state
            bool dead = false;

            //Checks for recall
            bool isRecalling = false;

            while (bot.isProcessOpen(Constants.GameProcessName))
            {
                bot.bringProcessToFront(Constants.GameProcessName);

                bot.centerProcess(Constants.GameProcessName);

                int newLevel = game.player.getLevel();

                if (newLevel != level)
                {
                    level = newLevel;
                    game.player.upgradeSpellOnLevelUp();
                }


                if (game.player.dead())
                {
                    if (!dead)
                    {
                        dead = true;
                        isRecalling = false;
                        OnDie();
                    }

                    bot.wait(4000);
                    continue;
                }

                if (dead)
                {
                    dead = false;
                    OnRevive();
                    continue;
                }

                if (isRecalling)
                {
                    game.player.recall();
                    bot.wait(8500);

                    if (game.player.getManaPercent() == 1)
                    {
                        OnSpawnJoin();
                        isRecalling = false;
                    }
                    continue;
                }



                if (game.player.getManaPercent() <= 0.10d)
                {
                    isRecalling = true;
                    continue;
                }

                if (game.player.getHealthPercent() <= 0.07d)
                {
                    isRecalling = true;
                    continue;
                }

                CastAndMove();


            }
        }
        private void OnDie()
        {
            BuyItems();
        }
        private void OnSpawnJoin()
        {
            BuyItems();
            //Go to your lane
            
        }
        private void OnRevive()
        {
            
        }

        private void CastAndMove() // Replace this by Champion pattern script.
        {
            
            Random rnd = new Random();
            int Numero = rnd.Next(0, 6);
            game.moveCenterScreen();

            if (Numero == 0)
                {
                game.player.tryCastSpellOnTarget(3); // veigar cage
                game.moveCenterScreen();
                bot.wait(2000);
                Numero = rnd.Next(0, 6);
            }
            else if (Numero == 1)
            {
                game.player.tryCastSpellOnTarget(2); // Z
                game.moveCenterScreen();
                bot.wait(2000);
                Numero = rnd.Next(0, 6);
            }

            else if (Numero == 2)
            {
                game.player.tryCastSpellOnTarget(1); // Q
                game.moveCenterScreen();
                bot.wait(2000);
                Numero = rnd.Next(0, 6);
            }

            else if (Numero == 3)
            {
                game.player.tryCastSpellOnTarget(4); // ult 
                game.moveCenterScreen();
                bot.wait(2000);
                Numero = rnd.Next(0, 6);
            }
            else if (Numero == 4)
            {
                //game.player.tryCastSpellOnTarget(D); // Flash 
                game.moveCenterScreen();
                bot.wait(2000);
                Numero = rnd.Next(0, 6);
            }
            else if (Numero == 5)
            {
                //game.player.tryCastSpellOnTarget(F); // Ghost
                game.moveCenterScreen();
                bot.wait(2000);
                Numero = rnd.Next(0, 6);
            }
            else if (Numero == 6)
            {
                CheckBuyItems();
                game.moveCenterScreen();
                bot.wait(2000);
                Numero = rnd.Next(0, 6);

            }
          
            int Ripeti = 0;
            while (Ripeti < 3)
            {

                Ripeti = Ripeti + 1;

                game.moveCenterScreen();

                game.player.tryCastSpellOnTarget(3); // veigar cage

                game.moveCenterScreen();

                game.player.tryCastSpellOnTarget(2); // Z

                game.moveCenterScreen();

                game.player.tryCastSpellOnTarget(1); // Q

                game.moveCenterScreen();

                game.player.tryCastSpellOnTarget(4); // ult 

                game.moveCenterScreen();

                game.player.tryCastSpellOnTarget(5); // Flash

                game.moveCenterScreen();

                game.player.tryCastSpellOnTarget(6); // Ghost
            }
            Ripeti = 0;
            CheckBuyItems();
            
        }



        public override void End()
        {
            bot.executePattern("EndCoop");
            base.End();
        }
    }
}
