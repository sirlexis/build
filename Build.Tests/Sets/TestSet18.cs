﻿using System;

namespace Build.Tests.TestSet18
{
    public interface IFactory<T>
    {
        T GetInstance();
    }

    public struct ValueStruct<T> where T : struct
    {
        public T Value;

        public ValueStruct(T value) => Value = value;

        public static implicit operator T(ValueStruct<T> valueStruct) => valueStruct.Value;

        public static implicit operator ValueStruct<T>(T value) => new ValueStruct<T>(value);

        public static bool operator !=(ValueStruct<T> left, ValueStruct<T> right) => !(left == right);

        public static bool operator ==(ValueStruct<T> left, ValueStruct<T> right)
        {
            return left.Equals(right);
        }

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();
    }

    public class EmptyClass
    {
    }

    public class Factory2<T> : IFactory<T>
    {
        public Factory2()
        {
        }

        public Factory2(Func<T> func) => Func = func;

        public Func<T> Func { get; }

        public T GetInstance() => Func();
    }

    public class Factory3<T>
    {
        public Factory3(Func<T> func) => Func = func;

        public object Func { get; }
    }

    public class Factory4<T> : IFactory<T>
    {
        public Factory4()
        {
        }

        [Dependency(typeof(Func<>))]
        public Factory4(Func<T> func) => Func = func;

        public Func<T> Func { get; }

        public T GetInstance() => Func();
    }

    public class Factory5<T>
    {
        public Factory5(Factory2<T> factory) => Factory = factory;

        public Factory5(IFactory<T> factory) => Factory = factory;

        public IFactory<T> Factory { get; }
    }

    public class IntPtrFactory
    {
        public IntPtrFactory(IntPtr factory) => Factory = factory;

        public IntPtr Factory { get; }
    }

    public class LazyFactory<T> : IFactory<T>
    {
        public LazyFactory(Func<T> func) => Func = func;

        public Func<T> Func { get; }

        public T GetInstance() => Func();
    }

    public class ValueClass<T> where T : struct
    {
        public T Value;

        public ValueClass(T value) => Value = value;

        public static implicit operator T(ValueClass<T> valueStruct) => valueStruct.Value;

        public static implicit operator ValueClass<T>(T value) => new ValueClass<T>(value);
    }
}