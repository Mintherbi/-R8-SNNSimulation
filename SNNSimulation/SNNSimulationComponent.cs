using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace SNNSimulation
{
    public class SNNSimulationComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SNNSimulationComponent()
          : base("SNNSimulation", "SNN",
            "Biologically Inspired Neural Network",
            "BinaryNature", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Neurons", "Neuron", "List of Neurons",GH_ParamAccess.list);      //0
            pManager.AddIntegerParameter("Seed", "Seed", "Random Seed for Wiring Network", GH_ParamAccess.item, 0);
            pManager.AddBooleanParameter("reset", "reset", "Reset to Initialize Network", GH_ParamAccess.item, false);
            //pManager.AddNumberParameter("Sensory", "Sensory", "Input of Spiking Neural Network", GH_ParamAccess.item);
        }

        List<LIFNeuron> Neurons;
        Wiring connectome;
        int t;
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Synapse", "Synapes", "Connections of Neurons", GH_ParamAccess.list);
            //pManager.AddNumberParameter("Weight", "Weight", "Weight of Each Synapse", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region ///Set Input Param
            List<Point3d> loc = new List<Point3d>();
            int seed = new int();
            List<Double> sensory = new List<Double>();
            bool reset = new bool();

            if (!DA.GetDataList(0, loc)) { return; }
            if (!DA.GetData(1, ref seed)) { return; }
            //if (!DA.GetDataList(2,sensory)) { return; }
            if (!DA.GetData(2, ref reset)) { return; }
            #endregion

            if (reset == false)
            {
                Neurons = new List<LIFNeuron>();

                for (int i = 0; i < loc.Count; i++)
                {
                    LIFNeuron temp = new LIFNeuron(loc[i]);
                    Neurons.Add(temp);                    
                }
                connectome = new Wiring(ref Neurons,new Vector3d(1,0,0));
                connectome.initialize(seed);

                t = 0;
            }

            for (int i = 0;i< Neurons.Count; i++)
            {
                int ap;
                ap = connectome.Nf[i].spike();
                if(ap > 0)
                {
                    connectome.Nf[connectome.Nf[i].post].ode_step(t, Neurons[i].w);     // 연결되어있는 다음 뉴런에 w 만큼 영향을 줌
                }
                connectome.plasticity(i);
            }





            t++;

            #region ///Set Output Param
            List<Line> Syn = new List<Line>();
            List<double> w = new List<double>();

            for (int i = 0;i < connectome.Nf.Count; i++)
            {
                if (connectome.Nf[i].post != -1)
                {
                    Syn.Add(new Line(connectome.Nf[i].location, connectome.Nf[connectome.Nf[i].post].location));
                    //w.Add(Neurons[i].w);
                }
            }
            
            DA.SetDataList(0, Syn);
            //DA.SetDataList(1, w);
            #endregion
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        //protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("16fba8f8-064f-4a32-8160-2ec3f9c5c9d4");
    }
}