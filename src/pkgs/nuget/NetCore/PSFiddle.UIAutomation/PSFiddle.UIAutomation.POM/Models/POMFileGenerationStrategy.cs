using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models
{
    public abstract class POMFileGenerationStrategy
    {
        private readonly string name;
        private readonly string directory;
        private readonly POMParsingContext _context;
        public POMParsingContext Context => _context;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name => name;

        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        public string Directory => directory;

        public POMFileGenerationStrategy(String Name,String Directory, POMParsingContext pOMParsingContext)
        {
            name = Name;
            directory = Directory;
            this._context = pOMParsingContext;
        }

        /// <summary>
        /// Priorities this instance.
        /// </summary>
        /// <returns></returns>
        public abstract int Priority();

        /// <summary>
        /// Discovers the parsing files.
        /// </summary>
        /// <returns></returns>
        public abstract List<String> DiscoverParsingFiles();

        /// <summary>
        /// Parses the file.
        /// </summary>
        /// <param name="File">The file.</param>
        public abstract void ParseFile(String File);
    }
}
