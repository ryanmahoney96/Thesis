namespace citadel_wpf
{
    public struct NodeInformation
    {
        public string EntityOne;
        public string Relationship;
        public string EntityTwo;

        //TODO get rid of this
        public NodeInformation(string eo, string r, string et)
        {
            EntityOne = eo;
            Relationship = r;
            EntityTwo = et;
        }

        override public string ToString()
        {
            return ("'" + EntityOne + "' " + Relationship + " '" + EntityTwo + "'");
        }
    }

}
