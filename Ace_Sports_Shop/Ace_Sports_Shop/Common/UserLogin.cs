using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ace_Sports_Shop.Common
{
    [Serializable]
    public  class UserLogin
    {
        public long ID { get; set; }
        public string Name { set; get; }
    }
}