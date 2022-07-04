using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calssinheriatane
{
    internal abstract class Creature
    {
        public string DNA;
        public int age;
        public float mass;

        public abstract void Breath();
        }
}
