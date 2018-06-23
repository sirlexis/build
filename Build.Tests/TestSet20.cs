﻿using System;

namespace Build.Tests.TestSet20
{
    public interface IFactory<T>
    {
        T GetInstance();
    }

    public class Class1
    {
    }

    public class Class2
    {
        [Dependency(RuntimeInstance.Singleton)]
        public Class2(Func<Class1> func)
        {
            Func = func;
        }

        public Func<Class1> Func { get; }
    }

    public class Class3
    {
        [Dependency(RuntimeInstance.CreateInstance)]
        public Class3(Func<Class1> func) => Func = func;

        public Func<Class1> Func { get; }
    }

    public class Class4
    {
        public Class4(Lazy<Class1> factory) => Lazy = factory;

        public Lazy<Class1> Lazy { get; }
    }

    public class Class5
    {
        public Class5(Func<Class1> func) => Func = func;

        public Class5([Injection(typeof(Func<Class1>))]object func) => Func = (Func<Class1>)func;

        public Func<Class1> Func { get; }
    }

    public class Lazy<T> : IFactory<T>
    {
        public Lazy(Func<T> func) => Func = func;

        public Func<T> Func { get; }

        public T GetInstance() => Func();
    }
}