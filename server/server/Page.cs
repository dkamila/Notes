using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    class Page
    {
        public string title;
        public string data;

        public Page(string title, string data)
        {
            this.title = title;
            this.data = data;
        }
    }
}
