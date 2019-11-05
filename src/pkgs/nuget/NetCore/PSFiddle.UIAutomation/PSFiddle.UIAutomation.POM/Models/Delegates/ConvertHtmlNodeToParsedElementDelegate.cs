using System;
using System.Collections.Generic;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.Delegates
{
    /// <summary>
    /// Delegate for the converting html node to parsed element
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="CurrentNode">The current node.</param>
    /// <returns></returns>
    public delegate POMRawElement ConvertHtmlNodeToParsedElementDelegate(POMParsingContext context, POMRawElement CurrentNode);

}
