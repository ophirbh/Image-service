using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    /// <summary>
    /// the commands availiable
    /// </summary>
    public enum CommandEnum : int
    {
        NewFileCommand,
        CloseCommand,           //can close one or all handlers (path or * accordingly)
        GetConfigCommand, 
        LogCommand,             //to get all the list so far
        ClientClosedCommand,
        NewLogCommand           //a new log was recieved
    }
}
