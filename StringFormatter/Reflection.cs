using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StringFormatter
{
    internal class Reflection
    {
        private class Key
        {
            internal Type Type { get; set; }
            internal string Name { get; set; }

        }
        private readonly ConcurrentDictionary<Key, Func<object, object>> propertyGetters;
        public Reflection()
        {
            propertyGetters = new ConcurrentDictionary<Key, Func<object, object>>();
        }
        public object GetValue(object entity, string name)
        {
            Func<object, object> getter;

            var key = new Key { Type = entity.GetType(), Name = name };

            if (propertyGetters.ContainsKey(key))
                getter = propertyGetters[key];
            else
            {
                var param = Expression.Parameter(typeof(object), "instance");
                Expression<Func<object, object>> getterExpression;
                try
                {
                    Expression body = Expression.PropertyOrField(Expression.TypeAs(param, entity.GetType()), name);
                    Expression conversion = Expression.Convert(body, typeof(object));
                    getterExpression = Expression.Lambda<Func<object, object>>(conversion, param);
                    getter = getterExpression.Compile();
                }
                catch
                {
                    getter = null;
                }
                if (getter != null)
                    propertyGetters.TryAdd(key, getter);
                else
                    return null;
            }
            return getter(entity);
        }
    }
}
