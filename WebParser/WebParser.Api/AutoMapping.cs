using AutoMapper;
using System.Collections.Generic;
using WebParser.Api.Scan;
using WebParser.Api.Storage;

namespace WebParser.Api
{
    /// <summary>
    /// Auto mapper configuration
    /// </summary>
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<TopWord, TopWordEntity>();
            CreateMap<TopWordEntity, TopWord>();
            CreateMap<TopPair, TopPairEntity>();
            CreateMap<TopPairEntity, TopPair>();

            CreateMap<TopWordViewModel, TopWord>();
            CreateMap<TopWord, TopWordViewModel>();
            CreateMap<TopPairViewModel, TopPair>();
            CreateMap<TopPair, TopPairViewModel>();

            CreateMap<ScanJobEntity, ScanJob>();
            CreateMap<ScanJob, ScanJobEntity>();
            CreateMap<List<ScanJob>, List<ScanJobEntity>>();

            CreateMap<ScanJobViewModel, ScanJob>();
            CreateMap<ScanJob, ScanJobViewModel>();
        }
    }
}
