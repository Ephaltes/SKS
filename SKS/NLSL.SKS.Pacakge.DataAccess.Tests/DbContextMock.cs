using System.Collections.Generic;
using System.Linq;

using FakeItEasy;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

namespace NLSL.SKS.Package.DataAccess.Tests
{
    public static class DbContextMock
    {
        //evtl direkt dbSet mocken anstatt diese Mocker klasse? https://www.codeproject.com/Articles/1220095/Using-FakeItEasy-with-Entity-Framework
        
        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            IQueryable<T> queryable = sourceList.AsQueryable();
            DbSet<T> dbSet = A.Fake<DbSet<T>>();

            A.CallTo(() => dbSet.As<IQueryable<T>>().Provider).Returns(queryable.Provider);
            A.CallTo(() => dbSet.As<IQueryable<T>>().Expression).Returns(queryable.Expression);
            A.CallTo(() => dbSet.As<IQueryable<T>>().ElementType).Returns(queryable.ElementType);
            A.CallTo(() => dbSet.As<IQueryable<T>>().GetEnumerator()).Returns(queryable.GetEnumerator());
            A.CallTo(() => dbSet.Add(A<T>.Ignored)).Invokes((T s) => sourceList.Add(s));

            return dbSet;
        }
    }
}