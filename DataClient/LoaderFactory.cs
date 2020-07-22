using BrassLoon.DataClient.LoaderComponents;
using System.Collections.Generic;
namespace BrassLoon.DataClient
{
    public class LoaderFactory : ILoaderFactory
    {
        public ILoader CreateLoader()
        {
            return new Loader
            {
                Components = new List<ILoaderComponent>
                {
                    new StringComponent(),
                    new IntegerComponent(),
                    new ShortComponent(),
                    new LongComponent(),
                    new DecimalComponent(),
                    new DoubleComponent(),
                    new DateComponent(),
                    new TimespanComponent(),
                    new BytesComponent(),
                    new ByteComponent(),
                    new BooleanComponent(),
                    new GuidComponent()
                }
            };
        }
    }
}