using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace SNNSimulation
{
    public class SNNSimulationInfo : GH_AssemblyInfo
    {
        public override string Name => "SNNSimulation";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        //public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("361e6c4f-7acd-4d85-a08d-25e65114fefc");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}