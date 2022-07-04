using System;
using System.Collections.Generic;

namespace Calssinheriatane
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Creature creature = new Creature();
            //creature.Breath();

            Human human = new Human();
            human.name = "실험체1";
            human.Breath();
            human.Grow();
            human.Grow();

            Yellowman yellowman = new Yellowman();
            yellowman.name = "황인종1";
            yellowman.Grow();
            BlackMan blackMan = new BlackMan();
            blackMan.name = "흑인종1";
            blackMan.Grow();
            Whiteman whiteman = new Whiteman();
            whiteman.name = "백인종1";
            whiteman.Grow();

            List<Human> men = new List<Human>();
            men.Add(new Yellowman());
            men.Add(new BlackMan());
            men.Add(new Whiteman());

            for (int i = 0; i < men.Count; i++)
            {
                men[i].name = $"사람{i + 1}";
                men[i].Grow();
            }

            List<ITwoLeggedWalker> twoLeggedWalkers = new List<ITwoLeggedWalker>();
            twoLeggedWalkers.Add(new Yellowman());
            twoLeggedWalkers.Add(new BlackMan());
            twoLeggedWalkers.Add(new Whiteman());

            for (int i = 0; i < men.Count; i++)
            {
                twoLeggedWalkers[i].TwoLeggedWalk();
            }

        }
    }
}
