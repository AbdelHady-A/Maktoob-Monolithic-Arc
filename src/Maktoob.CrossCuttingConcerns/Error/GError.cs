using System;
using System.Diagnostics.CodeAnalysis;

namespace Maktoob.CrossCuttingConcerns.Error
{
    /// <summary>
    /// Encapsulates an error from the Maktoob subsystem.
    /// </summary>
    public class GError : IEquatable<GError>
    {
        /// <summary>
        /// Gets or sets the code for this error.
        /// </summary>
        /// <value>
        /// The code for this error.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description for this error.
        /// </summary>
        /// <value>
        /// The description for this error.
        /// </value>
        public string Description { get; set; }

        public bool Equals([AllowNull] GError other)
        {
            if (ReferenceEquals(this, null) && ReferenceEquals(other, null))
            {
                return true;
            }

            if (ReferenceEquals(this, null) || ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.Code == other.Code)
            {
                return true;
            }
            return false;
        }
    }
}