using System;
using Pi_Books.Data.Models;

namespace Pi_Books.Data.ViewModels
{
    public class CustomActionResultVM
    {
        public Exception Exception { get; set; }
        //public Object Data { get; set; }
        public Publisher Publisher { get; set; }
    }
}