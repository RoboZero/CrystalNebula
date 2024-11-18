using System;
using System.Collections.Generic;

namespace Source.Logic.State.ResearchGraphs
{
    [Serializable]
    public class ResearchGraph
    {
        public string StartingDefinition;
        public Dictionary<string, List<ResearchEdge>> Edges;
    }
}