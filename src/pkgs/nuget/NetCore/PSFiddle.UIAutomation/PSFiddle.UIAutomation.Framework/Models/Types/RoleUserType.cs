using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Types
{
    public class RoleUsersType
    {
        /// <summary>
        /// Gets or sets the alpha user role key.
        /// </summary>
        /// <value>
        /// The alpha user role key.
        /// </value>
        public long? AlphaUserRoleKey { get; set; }
        /// <summary>
        /// Gets or sets the role key.
        /// </summary>
        /// <value>
        /// The role key.
        /// </value>
        public long RoleKey { get; set; }
        /// <summary>
        /// Gets or sets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        public long OrganizationKey { get; set; }
        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>
        /// The user key.
        /// </value>
        public long UserKey { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RoleUsersType"/> is status.
        /// </summary>
        /// <value>
        ///   <c>true</c> if status; otherwise, <c>false</c>.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        public string ModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets the modified by key.
        /// </summary>
        /// <value>
        /// The modified by key.
        /// </value>
        public long? ModifiedByKey { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the created by key.
        /// </summary>
        /// <value>
        /// The created by key.
        /// </value>
        public long? CreatedByKey { get; set; }


    }
}
