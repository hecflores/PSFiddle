using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class StateManagment : IStateManagment
    {
        private Func<String, Object> getAction;
        private Action<String, Object> setAction;
        public StateManagment(
                    Func<String, Object> getAction, 
                    Action<String, Object> setAction)
        {
            this.getAction = getAction;
            this.setAction = setAction;
        }
        public Object Get(string Name)
        {
            var obj = getAction(Name);
            if (obj == null)
            {
                throw new Exception($"{Name} was not found");
            }

            return obj;
        }
        public E Get<E>(string Name)
        {
            var obj = Get(Name);          

            if(!(obj is E))
            {
                throw new Exception($"{Name} is not of type {typeof(E).Name}");
            }

            return (E)obj;
        }

        public bool Has(string Name)
        {
            var obj = getAction(Name);
            return obj != null;
        }
        public bool Has<E>(string Name)
        {
            if (!Has(Name))
                return false;

            var obj = getAction(Name);
            if (!(obj is E))
                return false;
            return true;
        }
        public void Set<E>(string name, E obj)
        {
            setAction(name, obj);
        }
    }
}
