using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Bot_Manager
{
    public class TabContent
    {
        private readonly string _header;
        private readonly object _content;
        private readonly int _id;

        public TabContent(int id, string header, object content)
        {
            _id = id;
            _header = header;
            _content = content;
        }

        public string Header
        {
            get { return _header; }
        }

        public object Content
        {
            get { return _content; }
        }

        public int Id
        {
            get { return _id; }
        }
    }
}
