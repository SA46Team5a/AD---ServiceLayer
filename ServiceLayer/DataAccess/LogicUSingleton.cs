using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataAccess
{
    public partial class StationeryStoreEntities
    {
        static StationeryStoreEntities instance;
        public static StationeryStoreEntities Instance
        {
            get
            {
                if (instance is null)
                    instance = new StationeryStoreEntities();
                return instance;
            }
        }
    }
}
