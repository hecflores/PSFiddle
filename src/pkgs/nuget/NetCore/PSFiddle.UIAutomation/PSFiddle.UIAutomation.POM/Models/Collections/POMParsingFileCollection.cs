using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace PSFiddle.UIAutomation.POM.Models.Collections
{
    public class POMParsingFileCollection : BaseCollection<POMParsingFile>
    {
        public POMParsingFileCollection(POMParsingContext context) : base(context)
        {
        }

        /// <summary>
        /// Finds the parsed file.
        /// </summary>
        /// <param name="FilePath">The file path.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Expected 1 match but found {parsedFiles.Count()} matches with file path {FilePath}</exception>
        public POMParsingFile FindParsedFile(String FilePath)
        {
            var parsedFiles = this.Where(b => b.FilePath.Contains(FilePath));
            if (parsedFiles.Count() != 1)
                throw new Exception($"Expected 1 match but found {parsedFiles.Count()} matches with file path {FilePath}");

            return parsedFiles.FirstOrDefault();
        }
    }
}
