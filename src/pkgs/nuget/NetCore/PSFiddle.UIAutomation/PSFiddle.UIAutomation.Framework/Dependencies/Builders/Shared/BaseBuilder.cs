using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders.Shared;
using MC.Track.TestSuite.Model.Helpers;

namespace MC.Track.TestSuite.Toolkit.Dependencies.Builders.Shared
{
    public class BaseBuilder<T, E> : IBaseBuilder<T, E> where E: IBaseBuilder<T, E>
    {
        private T                initialModel = default(T);
        private List<String>     actionNames = new List<String>();
        private List<Func<T, T>> actions = new List<Func<T, T>>();

        public BaseBuilder() : this(default(T)) { }
        public BaseBuilder(T initial)
        {
            this.initialModel = initial;
        }
        public E Out(Action<T> callback)
        {
            this.AddBuildStep("Outing", (inItem) =>
            {
                callback(inItem);
                return inItem;
            });
            return (E)((IBaseBuilder<T, E>)this);
        }
        public E AddBuildStep(String name, Func<T, T> action)
        {
            this.actionNames.Add(name);
            this.actions.Add(action);
            return (E)((IBaseBuilder<T, E>)this);
        }
        public E InsertBuildStep(int pos, String name, Func<T, T> action)
        {
            this.actionNames.Insert(pos, name);
            this.actions.Insert(pos, action);
            return (E)((IBaseBuilder<T, E>)this);
        }
        public T Build()
        {
            XConsole.WriteLine($":: Building {typeof(T).Name} ::");
            if (this.actions.Count == 0 && this.initialModel == null) throw new Exception("Nothing to build");
            
            T item = this.initialModel;
            for(int i=0;i<this.actions.Count;i++)
            {
                XConsole.WriteLine($"   {this.actionNames[i]}");
                var action = this.actions[i];
                item = action(item);
            }

            XConsole.WriteLine($"");
            this.actions.Clear();
            this.actionNames.Clear();

            // Invoke all tear downs
            var methods = this.GetType().GetMethods(System.Reflection.BindingFlags.Public).ToList();
            var teardowns = methods.Where(b => Attribute.GetCustomAttribute(this.GetType(), typeof(BuilderTearDownAttribute)) != null).ToList();
            foreach(var tearDownMethod  in teardowns)
            {
                tearDownMethod.Invoke(this,new object[] { });
            }
           
            

            return item;

        }
    }
}
