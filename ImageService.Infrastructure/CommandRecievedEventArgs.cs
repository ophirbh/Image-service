using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure
{
    /// <summary>
    /// Represents arguments for an ICommand interface type class.
    /// </summary>
    public class CommandRecievedEventArgs : EventArgs
    {
        public int CommandID { get; set; }      // The Command ID
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory

        /// <summary>
        /// Constructor. creates a new command recieved arguments instance.
        /// </summary>
        /// <param name="id">The command's identification.</param> 
        /// <param name="args">The command's arguments.</param> 
        /// <param name="path">The path of the file that the command refers to.</param> 
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
