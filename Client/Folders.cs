using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommonClass;
namespace Client
{
    public class Folders:ObservableCollection<MatterFolder>
    {
        public Folders() : base() {  }
    }
}
