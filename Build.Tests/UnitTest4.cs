using Xunit;

namespace Build.Tests.TestSet4
{
    public class UnitTest
    {
        IContainer container;

        public UnitTest()
        {
            container = new Container();
            container.RegisterType<SqlDataRepository>();
            container.RegisterType<ServiceDataRepository>();
        }
        [Fact]
        public void TestSet4_Method1()
        {
            //TestSet4
            var srv1 = container.CreateInstance<ServiceDataRepository>();
            Assert.NotNull(srv1);
        }
        [Fact]
        public void TestSet4_Method2()
        {
            //TestSet4
            var srv2 = container.CreateInstance<ServiceDataRepository>();
            Assert.NotNull(srv2);
        }
        [Fact]
        public void TestSet4_Method3()
        {
            //TestSet4
            var srv1 = container.CreateInstance<ServiceDataRepository>();
            Assert.NotNull(srv1.Repository);
        }
        [Fact]
        public void TestSet4_Method4()
        {
            //TestSet4
            var srv2 = container.CreateInstance<ServiceDataRepository>();
            Assert.NotNull(srv2.Repository);
        }
        [Fact]
        public void TestSet4_Method5()
        {
            //TestSet4
            var srv1 = container.CreateInstance<ServiceDataRepository>();
            var srv2 = container.CreateInstance<ServiceDataRepository>();
            Assert.NotEqual(srv1, srv2);
        }
        [Fact]
        public void TestSet4_Method6()
        {
            //TestSet4
            var srv1 = container.CreateInstance<ServiceDataRepository>();
            var srv2 = container.CreateInstance<ServiceDataRepository>();
            Assert.Equal(srv1.Repository, srv2.Repository);
        }
    }
}