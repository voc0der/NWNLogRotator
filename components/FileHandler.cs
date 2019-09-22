using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNLogRotator.components
{
    public partial class FileHandler : Component
    {
        public FileHandler()
        {
            InitializeComponent();
        }

        public FileHandler(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
