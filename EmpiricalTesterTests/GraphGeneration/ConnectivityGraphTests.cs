using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmpiricalTester.GraphGeneration.Tests
{
    [TestClass()]
    public class ConnectivityGraphTests
    {
        [TestMethod()]
        public void GenerateConnectivityMatrixTest()
        {
            ConnectivityGraph cGraph = new ConnectivityGraph();
            for (int i = 0; i < 5; i++)
            {
                cGraph.AddVertex();
            }

            cGraph.AddEdge(0, 2);
            cGraph.AddEdge(0, 1);
            cGraph.AddEdge(2, 1);
            cGraph.AddEdge(1, 3);
            cGraph.AddEdge(3, 4);

            var connectivity = cGraph.GenerateConnectivityMatrix();
            bool row1 = connectivity[0][0] == false && connectivity[0][1] == true 
                     && connectivity[0][2] == true && connectivity[0][3] == true
                     && connectivity[0][4] == true;

            bool row2 = connectivity[1][0] == false && connectivity[1][1] == false
                     && connectivity[1][2] == false && connectivity[1][3] == true
                     && connectivity[1][4] == true;

            bool row3 = connectivity[2][0] == false && connectivity[2][1] == true
                     && connectivity[2][2] == false && connectivity[2][3] == true
                     && connectivity[2][4] == true;

            bool row4 = connectivity[3][0] == false && connectivity[3][1] == false
                     && connectivity[3][2] == false && connectivity[3][3] == false
                     && connectivity[3][4] == true;

            bool row5 = connectivity[4][0] == false && connectivity[4][1] == false
                     && connectivity[4][2] == false && connectivity[4][3] == false
                     && connectivity[4][4] == false;

            Assert.IsTrue(row1 && row2 && row3 && row4 && row5);
        }

      
    }
}