using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Maktoob.CrossCuttingConcerns.Error;
using Microsoft.AspNetCore.Identity;

namespace Maktoob.CrossCuttingConcerns.Result
{
    /// <summary>
    /// Represents the result of an Maktoob operation that can have an outcome.
    /// </summary>
    /// <typeparam name="TOutcome">The type of outcome object.</typeparam>
    
    public class GResult<TOutcome> : GResult where TOutcome : class
    {
        /// <summary>
        /// containing an outcome that produced during the Maktoob operation.
        /// </summary>
        public TOutcome Outcome { get; protected set; }
        /// <summary>
        /// Returns an <see cref="GResult"/> indicating a successful Maktoob operation.
        /// </summary>
        /// <returns>An <see cref="GResult"/> indicating a successful operation.</returns>
        public new static GResult<TOutcome> Success(TOutcome result)
        {
            return new GResult<TOutcome> { Succeeded = true, Outcome = result };
        }

        /// <summary>
        /// Creates an <see cref="GResult"/> indicating a failed Maktoob operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="GError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="GResult"/> indicating a failed Maktoob operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public new static GResult<TOutcome> Failed(params Error.GError[] errors)
        {
            var result = new GResult<TOutcome> { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }
    }
    /// <summary>
    /// Represents the result of an Maktoob operation.
    /// </summary>
    public class GResult : IEquatable<GResult>
    {
        protected List<Error.GError> _errors = new List<Error.GError>();

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="GError"/>s containing an errors
        /// that occurred during the Maktoob operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="GError"/>s.</value>
        public IEnumerable<Error.GError> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="GResult"/> indicating a successful Maktoob operation.
        /// </summary>
        /// <returns>An <see cref="GResult"/> indicating a successful operation.</returns>
        public static GResult Success { get; } = new GResult { Succeeded = true };

        /// <summary>
        /// Creates an <see cref="GResult"/> indicating a failed Maktoob operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="GError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="GResult"/> indicating a failed Maktoob operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static GResult Failed(params Error.GError[] errors)
        {
            var result = new GResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="GResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="GResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned 
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }

        public bool Equals([AllowNull] GResult other)
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

            if (this.Succeeded && other.Succeeded)
            {
                return true;
            }

            if (this.Errors.SequenceEqual(other.Errors))
            {
                return true;
            }
            return false;
        }
    }
}