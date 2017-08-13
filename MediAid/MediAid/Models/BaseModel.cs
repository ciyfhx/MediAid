using System;
using System.Collections.Generic;
using System.Text;

namespace MediAid.Models
{
    public class BaseModel
    {
        
        public readonly string Id;

        public BaseModel()
        {
            //Generate a Gobal Unique Id
            Id = Guid.NewGuid().ToString();
        }

    }
}
