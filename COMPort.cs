using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace COMPortLogger
{
    class ComPort : IComparable
    {
        public int Port { get; set; }
        public string Description { get; set; }
        public DateTime Added { get; set; }

        private static int GetPort(string s)
        {
            s = s.Remove(0, s.IndexOf("(COM") + 4);
            s = s.Substring(0, s.Length - 1);
            return int.Parse(s);
        }

        internal static List<ComPort> GetAllPorts()
        {
            DateTime added = DateTime.Now;
            List<ComPort> ret = new List<ComPort>();
            using (ManagementObjectSearcher searcher =
                       new ManagementObjectSearcher("root\\CIMV2",
                       "SELECT * FROM Win32_PnPEntity"))
            {

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"] == null) continue;
                    string cap = queryObj["Caption"].ToString();
                    if (cap.Contains("(COM") == false) continue;

                    if (cap.StartsWith("Communications Port")) continue;

                    ComPort c = new ComPort();
                    c.Added = added;
                    c.Port = GetPort(cap);
                    c.Description = cap.Substring(0, cap.IndexOf(" (COM"));
                    ret.Add(c);
                }
            }
            return ret;
        }

        public int CompareTo(object obj)
        {
            ComPort c2 = obj as ComPort;
            return this.Added.AddMilliseconds(this.Port).CompareTo(c2.Added.AddMilliseconds(c2.Port));
        }
    }
}
