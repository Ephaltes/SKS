using System;
using System.Collections.Generic;
using System.IO;

using AutoMapper;

using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

using Newtonsoft.Json;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.Services.Converter
{
    public class GeometryConverter : IValueConverter<string, Geometry>,
        IValueConverter<Geometry, string>
    {
        public string Convert(Geometry sourceMember, ResolutionContext context)
        {
            string geometryJson;
            JsonSerializer? serializer = GeoJsonSerializer.Create();
            using (StringWriter stringWriter = new StringWriter())
            using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter))
            {
                serializer.Serialize(jsonWriter, sourceMember);
                geometryJson = stringWriter.ToString();
            }

            return geometryJson;
        }
        public Geometry? Convert(string sourceMember, ResolutionContext context)
        {
            Geometry geometry;
            JsonSerializer serializer = GeoJsonSerializer.Create();
            using (StringReader stringReader = new StringReader(sourceMember))
            using (JsonTextReader jsonReader = new JsonTextReader(stringReader))
            {
                Feature? feature = serializer.Deserialize<Feature>(jsonReader);
                geometry = feature.Geometry;

                if (geometry is null)
                    return null;

                return ReverseGeometryIfNotCCW(geometry);

            }

        }

        public Geometry ReverseGeometryIfNotCCW(Geometry geometry)
        {
            switch (geometry.GeometryType)
            {
                case Geometry.TypeNameMultiPolygon:
                {
                    MultiPolygon multiPolygon = (MultiPolygon)geometry;
                    for (int i = 0; i < multiPolygon.Geometries.Length; i++)
                    {
                        Polygon polygon = (Polygon)multiPolygon.Geometries[i];
                        if (!polygon.Shell.IsCCW)
                        {
                            multiPolygon.Geometries[i] = multiPolygon.Geometries[i].Reverse();
                        }
                    }
                    return geometry;
                }
                case Geometry.TypeNamePolygon:
                {
                    Polygon polygon = (Polygon)geometry;
                    if (!polygon.Shell.IsCCW)
                    {
                        geometry = geometry.Reverse();
                    }

                    return geometry;
                }
            }

            throw new NotImplementedException();
        }
    }
}