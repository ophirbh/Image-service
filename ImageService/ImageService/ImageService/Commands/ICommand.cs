using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// Represents a command interface.
    /// </summary>
    public interface ICommand
    {
        string Execute(string[] args, out bool result);  // The Function That will Execute 
    }
}
