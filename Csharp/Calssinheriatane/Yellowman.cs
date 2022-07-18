using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calssinheriatane
{
    internal class Yellowman:Human
    {
        public override void Grow()
        {
            mass += 13f;
            height += 8f;

            Console.WriteLine($"{name} 이 자랐다 ! . |{mass},{height}|");

        }

        public void GoAcadamy()
        {

        }


    }
}
