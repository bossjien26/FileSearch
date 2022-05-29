using System.Collections.Generic;

namespace Helpers
{
    public class HeaderSetting
    {
        public List<Access> Response { get; set; }
    }

    public class Access
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}