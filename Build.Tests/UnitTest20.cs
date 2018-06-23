using System;
using System.Collections.Generic;
using Xunit;

namespace Build.Tests.TestSet20
{
    public static class UnitTest
    {
        [Fact]
        public static void TestSet2_Method1()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            container.RegisterType<Class2>();
            var srv1 = container.CreateInstance<Class2>();
            Assert.NotNull(srv1);
        }

        [Fact]
        public static void TestSet2_Method10()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            System.Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Class2>(class1FactoryMethod);
            var srv1 = container.CreateInstance<Class2>();
            var srv2 = container.CreateInstance<Class2>();
            Assert.Equal(srv1, srv2);
        }

        [Fact]
        public static void TestSet2_Method11()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Class2>(class1FactoryMethod);
            var srv1 = container.GetInstance<Class2>();
            var srv2 = container.GetInstance<Class2>();
            Assert.Equal(srv1, srv2);
        }

        [Fact]
        public static void TestSet2_Method12()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Class2>(class1FactoryMethod);
            var srv1 = container.GetInstance<Class2>();
            var srv2 = container.GetInstance<Class2>();
            Assert.Equal(srv1.Func, srv2.Func);
        }

        [Fact]
        public static void TestSet2_Method13()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            container.RegisterType<Class2>();
            var srv1 = container.GetInstance<Class2>();
            Assert.NotNull(srv1);
        }

        [Fact]
        public static void TestSet2_Method14()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            container.RegisterType<Class2>();
            var srv1 = container.GetInstance<Class2>();
            Assert.Null(srv1.Func);
        }

        [Fact]
        public static void TestSet2_Method15()
        {
            //TestSet2
            var container = new Container();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class2>();
            var srv1 = container.GetInstance<Class2>();
            Assert.Null(srv1.Func);
        }

        [Fact]
        public static void TestSet2_Method16()
        {
            //TestSet2
            var container = new Container();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            var srv1 = container.GetInstance<Lazy<Class1>>();
            Assert.NotNull(srv1.Func);
        }

        [Fact]
        public static void TestSet2_Method17()
        {
            //TestSet2
            var container = new Container();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class2>(class1FactoryMethod);
            var srv1 = container.GetInstance<Class2>();
            Assert.NotNull(srv1.Func);
        }

        [Fact]
        public static void TestSet2_Method18()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Class3>(class1FactoryMethod);
            var srv1 = container.GetInstance<Class3>();
            var srv2 = container.GetInstance<Class3>();
            Assert.NotEqual(srv1, srv2);
        }

        [Fact]
        public static void TestSet2_Method19()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Class3>(class1FactoryMethod);
            var srv1 = container.GetInstance<Class3>();
            var srv2 = container.GetInstance<Class3>();
            Assert.Equal(srv1.Func, srv2.Func);
        }

        [Fact]
        public static void TestSet2_Method2()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            container.RegisterType<Class2>();
            var srv2 = container.CreateInstance<Class2>();
            Assert.NotNull(srv2);
        }

        [Fact]
        public static void TestSet2_Method20()
        {
            //TestSet2
            var container = new Container();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class2>();
            var srv1 = container.GetInstance<Class2>(null);
            Assert.Null(srv1.Func);
        }

        [Fact]
        public static void TestSet2_Method21()
        {
            //TestSet2
            var container = new Container();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class2>();
            var srv1 = container.GetInstance<Class2>(Array.Empty<object>());
            Assert.Null(srv1.Func);
        }

        [Fact]
        public static void TestSet2_Method22()
        {
            //TestSet2
            var container = new Container();
            var builder = (TypeBuilder)container.Builder;
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class2>();
            var args = new List<string> { typeof(Func<Class1>).ToString() };
            var instance = builder.GetInstance(Format.GetConstructorWithParameters(typeof(Class2).ToString(), args));
            Assert.NotNull(instance);
        }

        [Fact]
        public static void TestSet2_Method23()
        {
            //TestSet2
            var container = new Container();
            var builder = (TypeBuilder)container.Builder;
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class2>();
            var args = new List<string> { typeof(Func<Class1>).ToString() };
            var instance = (Class2)builder.GetInstance(Format.GetConstructorWithParameters(typeof(Class2).ToString(), args));
            Assert.Null(instance.Func);
        }

        [Fact]
        public static void TestSet2_Method24()
        {
            //TestSet2
            var container = new Container();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class2>();
            var args = new List<string> { typeof(Func<Class1>).ToString() };
            var instance = (Class2)container.GetInstance(Format.GetConstructorWithParameters(typeof(Class2).ToString(), args), new object[] { class1FactoryMethod });
            Assert.NotNull(instance.Func);
        }

        [Fact]
        public static void TestSet2_Method25()
        {
            //TestSet2
            var container = new Container();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class2>();

            Func<Class1> appDbContextFactoryFunction = () => new Class1();

            container.RegisterType<Lazy<Class1>>(appDbContextFactoryFunction);
            container.RegisterType<Class4>();

            var instance = container.GetInstance(typeof(Class4).ToString());
            Assert.NotNull(instance);
        }

        [Fact]
        public static void TestSet2_Method26()
        {
            //TestSet2
            var container = new Container();
            var builder = (TypeBuilder)container.Builder;
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class2>();
            var instance = (Class2)builder.GetInstance(typeof(Class2).ToString());
            Assert.Null(instance.Func);
        }

        [Fact]
        public static void TestSet2_Method27()
        {
            //TestSet2
            var container = new Container();
            var builder = (TypeBuilder)container.Builder;
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class2>();
            Func<Class1> args = () => null;
            var instance = builder.GetInstance(typeof(Class2).ToString(), args);
            Assert.NotNull(instance);
        }

        [Fact]
        public static void TestSet2_Method28()
        {
            //TestSet2
            var container = new Container();
            var builder = (TypeBuilder)container.Builder;
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            var args = new List<string> { typeof(Func<Class1>).ToString() };
            Assert.Throws<TypeInstantiationException>(() => builder.GetInstance(typeof(Class2).ToString(), args));
        }

        [Fact]
        public static void TestSet2_Method29()
        {
            //TestSet2
            var container = new Container();
            var builder = (TypeBuilder)container.Builder;
            container.RegisterType<Class2>();
            var instance = (Class2)builder.GetInstance(typeof(Class2).ToString());
            Assert.Null(instance.Func);
        }

        [Fact]
        public static void TestSet2_Method3()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            container.RegisterType<Class2>();
            var srv1 = container.CreateInstance<Class2>();
            Assert.Null(srv1.Func);
        }

        [Fact]
        public static void TestSet2_Method30()
        {
            //TestSet2
            var container = new Container();
            var builder = (TypeBuilder)container.Builder;
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Lazy<Class1>>(class1FactoryMethod);
            container.RegisterType<Class5>();
            Func<Class1> args = () => new Class1();
            Assert.Throws<TypeInstantiationException>(() => builder.GetInstance(typeof(Class5).ToString(), args));
        }

        [Fact]
        public static void TestSet2_Method31()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            container.RegisterType<Class2>();
            Func<Class1> args = () => new Class1();
            var srv1 = container.GetInstance<Class2>(args);
            Assert.NotNull(srv1);
        }

        [Fact]
        public static void TestSet2_Method4()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Class2>(class1FactoryMethod);
            var srv2 = container.CreateInstance<Class2>();
            Assert.NotNull(srv2.Func);
        }

        [Fact]
        public static void TestSet2_Method5()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            container.RegisterType<Class2>();
            var srv1 = container.CreateInstance<Class2>();
            var srv2 = container.CreateInstance<Class2>();
            Assert.Equal(srv1, srv2);
        }

        [Fact]
        public static void TestSet2_Method6()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Class2>(class1FactoryMethod);
            var srv1 = container.CreateInstance<Class2>();
            var srv2 = container.CreateInstance<Class2>();
            Assert.Equal(srv1.Func, srv2.Func);
        }

        [Fact]
        public static void TestSet2_Method7()
        {
            //TestSet2
            var container = new Container();
            container.RegisterType<Class1>();
            container.RegisterType<Class2>();
            var srv1 = container.CreateInstance<Class2>();
            var srv2 = container.CreateInstance<Class2>();
            Assert.Equal(srv1.Func, srv2.Func);
        }

        [Fact]
        public static void TestSet2_Method8()
        {
            var container = new Container();
            container.RegisterType<Class1>();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Class2>(class1FactoryMethod);
            Assert.NotNull(container.CreateInstance<Class1>());
        }

        [Fact]
        public static void TestSet2_Method9()
        {
            var container = new Container();
            container.RegisterType<Class1>();
            Func<Class1> class1FactoryMethod = () => new Class1();
            container.RegisterType<Class2>(class1FactoryMethod);
            Assert.NotNull(container.CreateInstance<Class2>());
        }
    }
}