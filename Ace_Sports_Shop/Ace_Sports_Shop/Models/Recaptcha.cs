using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Net;
using System.Text;
using System.IO;

namespace Ace_Sports_Shop.Models
{
    [DataContract]
    public class Recaptcha
    {
        private object Request;

        [DataMember(Name ="Success")]
        public bool Success { get; set; }

        [DataMember(Name = "error-codes")]
        public string[] Errorcodes { get; set; }
       

        //public bool IsValCaptcha(string recaptcha)
        //{
        //    if (string.IsNullOrEmpty(recaptcha))
        //    {
        //        return false;
        //    }
        //    var secreKey = "6LeiCvYUAAAAADP1GvevNmYcgInm3p_LZtIGJkXL";
        //    string remoteIP = Request.ServerVariables["REMOTE_ADDR"];
        //    string myPrameter = string.Format("secret={0},& responsive={1},remoteip={2}", secreKey, recaptcha, remoteIP);
        //    Recaptcha re;
        //    using (var wc = new WebClient())
        //    {
        //        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        //        var json = wc.UploadString("https://www.google.com/recaptcha/api/siteverify", myPrameter);
        //        var js = new DataContractJsonSerializer(typeof(Recaptcha));
        //        var ms = new MemoryStream(Encoding.ASCII.GetBytes(json));
        //        re = js.ReadObject(ms) as Recaptcha;
        //        if (re != null && re.Success)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //}

    }
}