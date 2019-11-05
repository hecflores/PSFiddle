using MC.Track.TestSuite.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Dependencies
{
    public enum MatchState
    {
        HighConfidance,
        NoMatch,
        PossibleMatch
    }

    public interface IMatchingDependencies : IDependency
    {
        MatchingContext SetupMatching(UserType user, bool PerformPurchase = true, bool PerformLinking = true, bool HasHigh = true, bool HasNoMa = true, bool HasPossible = true);

        /// <summary>
        /// Creates the organization compliance.
        /// </summary>
        /// <param name="User">The user.</param>
        /// <param name="Purchased">if set to <c>true</c> [purchased].</param>
        /// <param name="Linked">if set to <c>true</c> [linked].</param>
        /// <param name="matchingState">State of the matching.</param>
        /// <returns>Company Name</returns>
        String CreateOrganizationCompliance(UserType User, bool Purchased, bool Linked, MatchState matchingState);
    }
}
