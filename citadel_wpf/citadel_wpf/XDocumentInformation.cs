using System.Collections.Generic;
using System.Xml.Linq;

namespace citadel_wpf
{
    //The foundation required to edit, delete, read, and save entity information to the XML files
    public partial class XDocumentInformation
    {
        public XDocument Handle;
        public string Name;
        public string Path;
        private List<EntityWindow> Observers = new List<EntityWindow>();

        public XDocumentInformation(XDocument x)
        {
            Handle = x;
        }
        public XDocumentInformation()
        {
        }

        public void Save()
        {
            Handle.Save(Path);
            Handle = XDocument.Load(Path);
            Update();
        }

        public void Attach(EntityWindow i)
        {
            Observers.Add(i);
        }

        public void Detach(EntityWindow i)
        {
            Observers.Remove(i);
        }

        public void Update()
        {
            foreach (var i in Observers)
            {
                i.Update(this);
            }
        }
    }
}

