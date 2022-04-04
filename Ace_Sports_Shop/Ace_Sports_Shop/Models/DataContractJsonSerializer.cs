using System;
using System.IO;

namespace Ace_Sports_Shop.Models
{
    internal class DataContractJsonSerializer
    {
        private Type type;

        public DataContractJsonSerializer(Type type)
        {
            this.type = type;
        }

        internal Recaptcha ReadObject(MemoryStream ms)
        {
            throw new NotImplementedException();
        }
    }
}