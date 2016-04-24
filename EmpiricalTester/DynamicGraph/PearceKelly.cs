using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EmpiricalTester.DataStructures;

namespace EmpiricalTester.DynamicGraph
{
    public class PearceKelly : IDynamicGraph
    {

        private List<SGTNode<PKNode>> _nodes; 
        private SGTree<PKNode> _order;
        private int _nCount;

        private List<SGTNode<PKNode>> _forward;
        private List<SGTNode<PKNode>> _backward;

        public PearceKelly()
        {
            _order = new SGTree<PKNode>(0.65);
            _nodes = new List<SGTNode<PKNode>>();
            _nCount = 0;
        }

        public void AddVertex()
        {
            _nodes.Add(_order.Count == 0
                ? _order.insertFirst(new PKNode(_nCount))
                : _order.insertAfter(_order.Root, new PKNode(_nCount)));

            _nCount++;
        }

        public bool AddEdge(int x, int y)
        {
            var lb = _nodes[y];
            var ub = _nodes[x];
            bool noCycle = true;

            if (_order.LT(lb, ub))
            {
                _forward = new List<SGTNode<PKNode>>();
                _backward = new List<SGTNode<PKNode>>();

                noCycle = DfsF(lb, ub);
                DfsB(ub, lb);

                Reorder();
            }

            if (noCycle)
            {
                _nodes[x].Value.Outgoing.Add(_nodes[y]);
                _nodes[y].Value.Incoming.Add(_nodes[x]);
            }

            return noCycle;
        }

        private bool DfsF(SGTNode<PKNode> n, SGTNode<PKNode> ub)
        {
            n.Value.Visited = true;
            // add to affected nodes for reorder
            _forward.Add(n);
            foreach (var w in n.Value.Outgoing)
            {
                if (n == ub)
                    return false;//cycle
                if(!w.Value.Visited && _order.LT(w, ub))
                    return DfsF(w, ub);
            }
            return true;
        }

        private void DfsB(SGTNode<PKNode> n, SGTNode<PKNode> lb)
        {
            n.Value.Visited = true;
            // add to affected nodes for reorder
            _backward.Add(n);
            foreach (var w in n.Value.Incoming)
            {
                if (!w.Value.Visited && _order.LT(lb, w))
                    DfsB(w, lb);
            }
        }

        private void Reorder()
        {
            var first = _backward[0];
            _backward.RemoveAt(0);

            //check the sort
            _forward.Sort((a, b) => a.CompareTo(b));
            _backward.Sort((a, b) => a.CompareTo(b));

            foreach (var node in _forward)
            {
                node.Value.Visited = false;
                _order.Remove(node);
            }

            foreach (var node in _backward)
            {
                node.Value.Visited = false;
                _order.Remove(node);
            }
            
            //insert all after ub ( _backwards[0] )
            for (int i = 0; i < _backward.Count; i++)
            {
                first = _order.insertAfter(first, _backward[i]);
            }
            for (int i = 0; i < _forward.Count; i++)
            {
                first = _order.insertAfter(first, _forward[i]);
            }            
        }


        public void RemoveEdge(int iv, int iw)
        {
            
        }

        public List<int> Topology()
        {
            return null;
        }

        public void ResetAll(int n)
        {
            
        }



    }

    internal class PKNode
    {
        public List<SGTNode<PKNode>> Outgoing { get; set; }
        public List<SGTNode<PKNode>> Incoming { get; set; }
        public bool Visited { get; set; }
        public int Value { get; set; }

        public PKNode(int nVal)
        {
            Value = nVal;
            Outgoing = new List<SGTNode<PKNode>>();
            Incoming = new List<SGTNode<PKNode>>();
        }
    }
}
