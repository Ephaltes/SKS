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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using NetTopologySuite.Geometries;

namespace NLSL.SKS.Package.BusinessLogic.Entities
{
    /// <summary>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TransferWarehouse : Hop
    {
        /// <summary>
        /// GeoJSON of the are covered by the logistics partner.
        /// </summary>
        /// <value>GeoJSON of the are covered by the logistics partner.</value>
        public Geometry RegionGeometry
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the logistics partner.
        /// </summary>
        /// <value>Name of the logistics partner.</value>
        public string LogisticsPartner
        {
            get;
            set;
        }

        /// <summary>
        /// BaseURL of the logistics partner&#x27;s REST service.
        /// </summary>
        /// <value>BaseURL of the logistics partner&#x27;s REST service.</value>
        public string LogisticsPartnerUrl
        {
            get;
            set;
        }
    }
}