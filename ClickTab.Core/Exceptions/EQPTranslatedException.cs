using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.Exceptions
{
    public class EQPTranslatedException : Exception
    {
        public string TranslateKey { get; set; }

        public EQPTranslatedException(string TranslateKey) : base(TranslateKey)
        {
            this.TranslateKey = TranslateKey;
        }
    }
}
