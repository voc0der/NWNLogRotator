using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNLogRotator.classes
{
    class Settings
    {
        public string OutputDirectory;
        public string PathToLog;
        public int MinimumRowsCount;

        public Settings(string OutputDirectory, string PathToLog, int MinimumRowsCount)
        {
            this.OutputDirectory = OutputDirectory;
            this.PathToLog = PathToLog;
            this.MinimumRowsCount = MinimumRowsCount;
        }
    }
}
