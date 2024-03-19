﻿using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SNNSimulation
{
    public class Wiring
    {
        //Properites
        public List<LIFNeuron> Nf = new List<LIFNeuron>();
        public Vector3d direction; //0:x, 1:y, 2:z
        public double wth = 5e-2;           //가소성이 일어나는 weigh 한계

        //construction
        public Wiring(ref List<LIFNeuron> Neurons, Vector3d direction)
        {
            this.direction = direction;

            for(int i = 0; i < Neurons.Count; i++)
            {
                Nf.Add(Neurons[i]);
            }

            SortForword(ref this.Nf);           //direction 방향으로 정렬을 함
        }

        //method
        public void initialize(int seed)
        {
            Random rand = new Random(seed);

            for (int i = 0; i < this.Nf.Count; i++)
            {
                List<LIFNeuron> SortbyDis = new List<LIFNeuron>();

                SortbyDis = this.Nf.Skip(i).ToList();
                SortDistance(this.Nf[i].location, ref SortbyDis);
                
                int radial_rand = (int) (Math.Pow(rand.NextDouble(), 4) * SortbyDis.Count);

                if (radial_rand == 0 && SortbyDis.Count>1) { radial_rand =  1; }
                this.Nf[i].post = this.Nf.IndexOf(SortbyDis[radial_rand]);                
            }
        }

        public void plasticity(int i)
        {
            if (Nf[i].w < this.wth || Nf[i].w > (-1) * this.wth)
            {
                int new_connect;
                do
                {
                    Random rand = new Random();
                    new_connect = (int) Math.Round((Nf.Count - i) * rand.NextDouble());
                } while (new_connect != i);
                Nf[i].post = new_connect;
            }
        }

        //misc
        private void SortForword(ref List<LIFNeuron> list) 
        {
            list = list.OrderBy(x => x.location.Y).ToList();
        }

        private void SortDistance(Point3d from, ref List<LIFNeuron> list)
        {           
            for(int i = 0; i < list.Count; i++)
            {
                list[i].distance = from.DistanceTo(list[i].location);
            }

            list = list.OrderBy(x=>x.distance).ToList();
        }
    }
}