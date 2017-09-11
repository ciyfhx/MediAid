using System;
using System.Collections.Generic;
using System.Text;

namespace MediAid.Models
{
    public class BaseModel
    {

        public string Id { get; set; }
        

        public BaseModel()
        {
            //Generate a Gobal Unique Id
            Id = Guid.NewGuid().ToString();
        }


    }
}
