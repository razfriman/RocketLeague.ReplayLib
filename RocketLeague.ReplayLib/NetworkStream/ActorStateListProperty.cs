using System;
using System.Collections.Generic;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ActorStateListProperty : ActorStateProperty
    {
        public ActorStateListProperty(ActorStateProperty convertFromProperty) : base(convertFromProperty)
        {
            Data = new List<object> { convertFromProperty.Data };
        }

        public void Add(ActorStateProperty property)
        {
            if (PropertyId != property.PropertyId)
            {
                throw new ArgumentException("Property id mismatch, can not add to list");
            }

            ((List<object>)Data).Add(property.Data);
        }
    }
}