/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.BusinessLogic.Entities
{
    /// <summary>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Recipient
    {
        /// <summary>
        /// Name of person or company.
        /// </summary>
        /// <value>Name of person or company.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Street
        /// </summary>
        /// <value>Street</value>
        public string Street
        {
            get;
            set;
        }

        /// <summary>
        /// Postalcode
        /// </summary>
        /// <value>Postalcode</value>
        public string PostalCode
        {
            get;
            set;
        }

        /// <summary>
        /// City
        /// </summary>
        /// <value>City</value>
        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// Country
        /// </summary>
        /// <value>Country</value>
        public string Country
        {
            get;
            set;
        }
        
        public bool IsInAustria =>
            (Country.ToLowerInvariant() == "austria" 
             || Country.ToLowerInvariant() == "österreich");
    }
}