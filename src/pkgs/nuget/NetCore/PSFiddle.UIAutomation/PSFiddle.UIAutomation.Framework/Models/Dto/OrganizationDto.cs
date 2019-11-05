using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class OrganizationDto
    {
        /// <summary>
        /// Gets or sets the alpha identifier.
        /// </summary>
        /// <value>
        /// The alpha identifier.
        /// </value>
        public string AlphaID { get; set; }

        /// <summary>
        /// Gets or sets the name of the registered business.
        /// </summary>
        /// <value>
        /// The name of the registered business.
        /// </value>
        public string RegisteredBusinessName { get; set; }

        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>
        /// The address1.
        /// </value>
        public string Address1 { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>
        /// The zip.
        /// </value>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the is o3.
        /// </summary>
        /// <value>
        /// The is o3.
        /// </value>
        public string ISO3 { get; set; }
    }
}
