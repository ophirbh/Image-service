using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// Represents a new file command.
    /// </summary>
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        /// <summary>
        /// Constructor. creates a new file command instance.
        /// </summary>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        /// <summary>
        /// Executes the assignment of the command by activating the AddFile function in the modal.
        /// </summary>
        /// <param name="args">The assignment's arguments.</param> 
        /// <param name="result">The result of the assignment.</param> 
        /// <returns>A string with information about the success/failure of the assignment.</returns> 
        public string Execute(string[] args, out bool result)
        {
            return m_modal.AddFile(args[0], out result); 
        }
    }
}
