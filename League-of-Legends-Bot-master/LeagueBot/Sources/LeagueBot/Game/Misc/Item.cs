﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBot.Game.Misc
{
    public class Item
    {
        public string Name
        {
            get;
            private set;
        }
        public int Cost
        {
            get;
            private set;
        }
        public bool Buyed
        {
            get;
            set;
        }

        public bool BuyComponents
        {
            get;
            set;
        }

        public Item(string name, int cost, bool BuyComponents)
        {
            this.Name = name;
            this.Cost = cost;
            this.Buyed = false;
            this.BuyComponents = BuyComponents;
        }
    }
}
