using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GinasGameofWar.Code
{
    class Player
    {
        public string FirstName { get; set; }
        public Queue<Card> Hand { get; set; }

        public Player(string name) {

            FirstName = name;

        }
    }
}
