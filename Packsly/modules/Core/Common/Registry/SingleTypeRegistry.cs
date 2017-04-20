using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly.Core.Common.Registry {

    public class SingleTypeRegistry<T> : IRegistry<T> {

        protected static List<T> modules = new List<T>();

        #region IRegistry

        public void Register(params T[] elements) {
            foreach(T element in elements)
                if(!modules.Contains(element) || !modules.Any(m => m.GetType().Equals(element.GetType())))
                    modules.Add(element);
        }

        #endregion

    }

}
