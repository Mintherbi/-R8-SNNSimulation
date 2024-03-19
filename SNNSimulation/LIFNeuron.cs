﻿using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNNSimulation
{
    public class LIFNeuron
    {
        //properties - dynamic
        public Point3d location;           //뉴런 위치
        public double v;           //현재 멤브렌인 포텐셜
        public double w;           //스파이크 가중치
        public List<bool> y;         //스파이크 기록 (bool 배열)
        public int dt;         //dt
        public int post=-1;           //포스트 뉴런의 일련번호 (hash)
        public double distance;         //거리 값 (임시)

        //construction
        public LIFNeuron(Point3d location)
        {
            this.location = location;
        }

        //method
        public void ode_step(int t, double input=0)
        {
            double tau = 20e-3;         // membrane time constant
            double el = -60e-3;         // leak potential
            double r = -100e6;          // membrane resistence

            this.v += this.dt / tau * (el - this.v + r * input);
        }

        public int spike()
        {
            double vr = -70e-3;         //resting potential
            double vth = -50e-3;         //Threshold

            if (this.v >= vth) 
            { 
                y.Add(true); 
                this.v = vr;
                return 1;
            }
            else 
            { 
                y.Add(false);
                return 0;
            }
        }
    }
}