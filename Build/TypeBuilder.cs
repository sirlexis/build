using System;
using System.Collections.Generic;
using System.Linq;

namespace Build
{
    /// <summary>
    /// Type builder
    /// </summary>
    public sealed class TypeBuilder : ITypeBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuilder"/> class.
        /// </summary>
        /// <param name="typeConstructor">Type constructor</param>
        /// <param name="typeFilter">Type filter</param>
        /// <param name="typeParser">Type parser</param>
        /// <param name="typeResolver">Type resolver</param>
        /// <param name="defaultTypeResolution">
        /// Parameter defaults to true for automatic type resolution enabled. If value is false and
        /// not all type dependencies are resolved, exception will be thrown
        /// </param>
        /// <param name="defaultTypeInstantiation">
        /// Parameter defaults to true for automatic type instantiation enabled. If value is false
        /// and type is resolved to default value for reference type, exception will be thrown
        /// </param>
        /// <param name="defaultTypeAttributeOverwrite">
        /// Parameter defaults to true for automatic type attribute overwrite. If value is false
        /// exception will be thrown for type attribute overwrites
        /// </param>
        public TypeBuilder(ITypeConstructor typeConstructor, ITypeFilter typeFilter, ITypeParser typeParser, ITypeResolver typeResolver, bool defaultTypeResolution, bool defaultTypeInstantiation, bool defaultTypeAttributeOverwrite)
        {
            UseDefaultTypeResolution = defaultTypeResolution;
            UseDefaultTypeInstantiation = defaultTypeInstantiation;
            UseDefaultTypeAttributeOverwrite = defaultTypeAttributeOverwrite;
            Constructor = typeConstructor ?? throw new ArgumentNullException(nameof(typeConstructor));
            Filter = typeFilter ?? throw new ArgumentNullException(nameof(typeFilter));
            Resolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
            Parser = typeParser ?? throw new ArgumentNullException(nameof(typeParser));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuilder"/> class.
        /// </summary>
        /// <param name="defaultTypeResolution">
        /// Parameter defaults to true for automatic type resolution enabled. If value is false and
        /// not all type dependencies are resolved, exception will be thrown
        /// </param>
        /// <param name="defaultTypeInstantiation">
        /// Parameter defaults to true for automatic type instantiation enabled. If value is false
        /// and type is resolved to default value for reference type, exception will be thrown
        /// </param>
        /// <param name="defaultTypeAttributeOverwrite">
        /// Parameter defaults to true for automatic type attribute overwrite. If value is false
        /// exception will be thrown for type attribute overwrites
        /// </param>
        public TypeBuilder(bool defaultTypeResolution, bool defaultTypeInstantiation, bool defaultTypeAttributeOverwrite)
        {
            UseDefaultTypeResolution = defaultTypeResolution;
            UseDefaultTypeInstantiation = defaultTypeInstantiation;
            UseDefaultTypeAttributeOverwrite = defaultTypeAttributeOverwrite;
            Constructor = new TypeConstructor();
            Filter = new TypeFilter();
            Resolver = new TypeResolver();
            Parser = new TypeParser();
        }

        /// <summary>
        /// Constructs type dependency
        /// </summary>
        public ITypeConstructor Constructor { get; }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public ITypeFilter Filter { get; }

        /// <summary>
        /// Gets the parser.
        /// </summary>
        /// <value>The parser.</value>
        public ITypeParser Parser { get; }

        /// <summary>
        /// Gets the resolver.
        /// </summary>
        /// <value>The resolver.</value>
        public ITypeResolver Resolver { get; }

        /// <summary>
        /// Gets the runtime aliased types.
        /// </summary>
        /// <value>The type aliases.</value>
        public IEnumerable<string> RuntimeAliasedTypes => Types.Where(p => p.Key != p.Value.Id).Select(p => p.Value.Id);

        /// <summary>
        /// Gets the runtime non aliased types.
        /// </summary>
        /// <value>The runtime non aliased types.</value>
        public IEnumerable<string> RuntimeNonAliasedTypes => Types.Where(p => p.Key == p.Value.Id).Select(p => p.Value.Id);

        /// <summary>
        /// Gets the runtime aliases.
        /// </summary>
        /// <value>The runtime aliases.</value>
        public IEnumerable<string> RuntimeTypeAliases => Types.Where(p => p.Key != p.Value.Id).Select(p => p.Key);

        /// <summary>
        /// Gets the runtime types.
        /// </summary>
        /// <value>The runtime types.</value>
        public IEnumerable<string> RuntimeTypes => Types.Select(p => p.Value.Id);

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <value>The types.</value>
        public IDictionary<string, IRuntimeType> Types { get; } = new Dictionary<string, IRuntimeType>();

        /// <summary>
        /// True if automatic type instantiation for reference types option enabled (does not throws
        /// exceptions for reference types defaults to null)
        /// </summary>
        /// <remarks>
        /// If automatic type instantiation for reference types is enabled, type will defaults to
        /// null if not resolved and no exception will be thrown
        /// </remarks>
        bool UseDefaultTypeAttributeOverwrite { get; }

        /// <summary>
        /// True if automatic type instantiation for reference types option enabled (does not throws
        /// exceptions for reference types defaults to null)
        /// </summary>
        /// <remarks>
        /// If automatic type instantiation for reference types is enabled, type will defaults to
        /// null if not resolved and no exception will be thrown
        /// </remarks>
        bool UseDefaultTypeInstantiation { get; }

        /// <summary>
        /// True if automatic type resolution for reference types option enabled (does not throws
        /// exceptions for reference types contains type dependencies to non-registered types)
        /// </summary>
        /// <remarks>
        /// If automatic type resolution for reference types is enabled, type will defaults to null
        /// if not resolved and no exception will be thrown
        /// </remarks>
        bool UseDefaultTypeResolution { get; }

        /// <summary>
        /// List the visited types.
        /// </summary>
        /// <value>The visited.</value>
        List<Type> Visited { get; } = new List<Type>();

        /// <summary>
        /// Gets the <see cref="RuntimeType"/> with the specified identifier.
        /// </summary>
        /// <value>The <see cref="RuntimeType"/>.</value>
        /// <param name="typeFullName">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IRuntimeType this[string typeFullName, IRuntimeType type]
        {
            get
            {
                if (!Types.ContainsKey(typeFullName))
                    Types.Add(typeFullName, type);
                Types[typeFullName].RegisterTypeDefinition(type.TypeFullName);
                return Types[typeFullName];
            }
        }

        /// <summary>
        /// Determines whether this instance can register the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if this instance can register the specified type; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRegister(Type type) => Filter.CanRegister(type);

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="typeFullName">The identifier.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="TypeInstantiationException"></exception>
        public object CreateInstance(string typeFullName, params object[] args)
        {
            if (Types.ContainsKey(typeFullName))
                return Types[typeFullName].CreateInstance(args);
            var runtimeTypes = GetRuntimeTypes(typeFullName, args).ToArray();
            if (runtimeTypes.Length == 1)
                return runtimeTypes[0].CreateInstance(args);
            if (runtimeTypes.Length > 1)
            {
                if (args != null && args.Length == 0)
                {
                    var runtimeType = runtimeTypes.FirstOrDefault((p) => p.Count == 0);
                    if (runtimeType != null)
                        return runtimeType.CreateInstance(args);
                }
                throw new TypeInstantiationException(string.Format("{0} is not instantiated (more than one constructor available)", typeFullName));
            }
            throw new TypeInstantiationException(string.Format("{0} is not instantiated (no constructors available)", typeFullName));
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="typeFullName">The identifier.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="TypeInstantiationException"></exception>
        public object GetInstance(string typeFullName, params object[] args)
        {
            if (Types.ContainsKey(typeFullName))
                return Types[typeFullName].CreateInstance(args);
            var runtimeTypes = GetRuntimeTypes(typeFullName, args).ToArray();
            if (runtimeTypes.Length == 1)
            {
                var runtimeType = runtimeTypes[0];
                if (runtimeType.GetInstance)
                {
                    return runtimeType.CreateInstance();
                }
                var parameters = new List<object>();
                foreach (var parameter in runtimeType.RuntimeTypes)
                {
                    var parameterRuntimeTypes = GetRuntimeTypes(parameter.TypeFullName, args).Where((p) => p.Type == parameter.Type).ToArray();
                    if (parameterRuntimeTypes.Length == 1)
                    {
                        var parameterRuntimeType = parameterRuntimeTypes[0];
                        if (parameterRuntimeType.GetInstance)
                            parameters.Add(parameterRuntimeType.CreateInstance());
                        else
                            parameters.Add(parameterRuntimeType.Value);
                    }
                    else
                    {
                        parameters.Add(null);
                    }
                }
                return runtimeType.CreateInstance(parameters.ToArray());
            }
            if (runtimeTypes.Length > 1)
            {
                throw new TypeInstantiationException(string.Format("{0} is not instantiated (more than one constructor available)", typeFullName));
            }
            throw new TypeInstantiationException(string.Format("{0} is not instantiated (no constructors available)", typeFullName));
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="type">The type.</param>
        internal void RegisterType(Type type, params object[] args)
        {
            Visited.Add(type);
            try
            {
                RegisterConstructor(type);
                if (args == null || args.Length == 0)
                    return;
                RegisterConstructorParameters(type.ToString(), args);
            }
            catch (TypeRegistrationException ex)
            {
                throw new TypeRegistrationException(string.Format("{0} is not registered", type), ex);
            }
            finally
            {
                Visited.Remove(type);
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        internal void Reset() => Types.Clear();

        /// <summary>
        /// Gets the full name of the parameters.
        /// </summary>
        /// <param name="typeFullName">The type.</param>
        /// <param name="parameterTypeFullName">Type of the parameter.</param>
        /// <returns></returns>
        /// <exception cref="TypeRegistrationException"></exception>
        static void CheckParametersFullName(string typeFullName, string parameterTypeFullName)
        {
            if (typeFullName == parameterTypeFullName)
                throw new TypeRegistrationException(string.Format("{0} is not registered (circular references found)", typeFullName));
        }

        /// <summary>
        /// Gets the full name of the parameters.
        /// </summary>
        /// <param name="parameterType">Type of the parameter.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        /// <exception cref="TypeRegistrationException"></exception>
        void CheckTypeFullName(Type parameterType, Type attributeType)
        {
            if (attributeType != null && !Filter.CheckTypeFullName(parameterType, attributeType))
                throw new TypeRegistrationException(string.Format("{0} is not registered (not assignable from {1})", parameterType.Name, attributeType.Name));
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="dependencyObject">The type definition.</param>
        /// <returns></returns>
        /// <exception cref="TypeRegistrationException"></exception>
        void CheckTypeFullName(ITypeDependencyObject dependencyObject)
        {
            var attributeType = Resolver.GetType(dependencyObject.RuntimeType.Type.Assembly, dependencyObject.TypeFullName);
            if (attributeType != null && !Filter.CheckTypeFullName(attributeType, dependencyObject.RuntimeType.Type))
                throw new TypeRegistrationException(string.Format("{0} is not registered (not assignable from {1})", attributeType.Name, dependencyObject.RuntimeType.TypeFullName));
        }

        /// <summary>
        /// Finds all dependency runtime types (instantiable types) which matches the criteria
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        IEnumerable<IRuntimeType> GetRuntimeTypes(string typeFullName, params object[] args) => Parser.FindAll(typeFullName, Format.GetParametersFullName(args), Types.Values.Where((p) => p.Attribute is DependencyAttribute));

        /// <summary>
        /// Registers the constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="TypeRegistrationException"></exception>
        void RegisterConstructor(Type type)
        {
            if (!type.IsValueType)
            {
                var constructorEnumerator = Constructor.GetDependencyObjects(type, UseDefaultTypeInstantiation).GetEnumerator();
                if (!constructorEnumerator.MoveNext())
                    throw new TypeRegistrationException(string.Format("{0} is not registered (no constructors available)", type));
                do
                {
                    var dependencyObject = constructorEnumerator.Current;
                    foreach (var injectionObject in dependencyObject.InjectionObjects)
                    {
                        RegisterConstructorParameter(dependencyObject, injectionObject);
                    }
                    RegisterConstructorDependencyObject(dependencyObject);
                } while (constructorEnumerator.MoveNext());
            }
        }

        /// <summary>
        /// Registers the type of the constructor.
        /// </summary>
        /// <param name="dependencyObject">Type dependency object.</param>
        void RegisterConstructorDependencyObject(ITypeDependencyObject dependencyObject)
        {
            var constructor = dependencyObject.RuntimeType;
            var constructorAttribute = dependencyObject.DependencyAttribute;
            CheckTypeFullName(dependencyObject);
            var typeFullName = dependencyObject.TypeFullNameWithParameters;
            if (!Types.ContainsKey(typeFullName))
            {
                var result = this[typeFullName, constructor];
                if (result != null)
                {
                    result.SetRuntimeInstance(constructorAttribute.RuntimeInstance);
                }
            }
        }

        /// <summary>
        /// Registers the constructor parameter.
        /// </summary>
        /// <param name="dependencyObject">The constructor.</param>
        /// <param name="injectionObject">The constructor parameter.</param>
        void RegisterConstructorParameter(ITypeDependencyObject dependencyObject, ITypeInjectionObject injectionObject)
        {
            var constructor = dependencyObject.RuntimeType;
            var constructorType = constructor.Type;
            var parameter = injectionObject.RuntimeType;
            var parameterType = parameter.Type;
            string typeFullName = injectionObject.TypeFullName;
            var attributeType = Resolver.GetType(constructorType.Assembly, typeFullName);
            CheckTypeFullName(parameterType, attributeType);
            CheckParametersFullName(constructorType.Name, parameterType.Name);
            var parameters = injectionObject.TypeParameters;
            var runtimeType = Parser.Find(typeFullName, parameters, Types.Values);
            if (UseDefaultTypeResolution && runtimeType == null)
                RegisterConstructorParameter(attributeType);
            RegisterConstructorType(parameterType);
            RegisterRuntimeType(dependencyObject, injectionObject);
        }

        /// <summary>
        /// Registers the parameter type of the constructor.
        /// </summary>
        /// <param name="type">The parameter type.</param>
        /// <exception cref="TypeRegistrationException"></exception>
        void RegisterConstructorParameter(Type type)
        {
            if (Filter.CanRegisterParameter(type))
            {
                if (Visited.Contains(type))
                    throw new TypeRegistrationException(string.Format("{0} is not registered (circular references found)", type));
                RegisterType(type);
            }
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="typeFullName">The identifier.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="TypeInstantiationException"></exception>
        void RegisterConstructorParameters(string typeFullName, params object[] args)
        {
            var parameterArgs = Format.GetParametersFullName(args);
            var runtimeTypes = new List<IRuntimeType>(Parser.FindAll(typeFullName, parameterArgs, Types.Values));
            if (runtimeTypes.Count == 0)
                throw new TypeRegistrationException(string.Format("{0} is not registered (no constructors available)", typeFullName));
            runtimeTypes[0].SetRuntimeInstance(RuntimeInstance.GetInstance);
            runtimeTypes[0].RegisterConstructorParameters(args);
        }

        /// <summary>
        /// Registers the type of the constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="TypeRegistrationException"></exception>
        void RegisterConstructorType(Type type)
        {
            if (CanRegister(type))
            {
                if (Visited.Contains(type))
                    throw new TypeRegistrationException(string.Format("{0} is not registered (circular references found)", type));
                RegisterType(type);
            }
        }

        void RegisterRuntimeType(ITypeDependencyObject dependencyObject, ITypeInjectionObject injectionObject)
        {
            var constructor = dependencyObject.RuntimeType;
            var parameter = injectionObject.RuntimeType;
            var typeFullName = injectionObject.TypeFullNameWithParameters;
            var result = this[typeFullName, parameter];
            if (result != null)
            {
                var constructorFullName = dependencyObject.TypeFullNameWithParameters;
                result.Attribute.RegisterRuntimeType(constructorFullName, injectionObject.InjectionAttribute, UseDefaultTypeAttributeOverwrite);
                constructor.AddConstructorParameter(CanRegister(result.Type), result);
            }
        }
    }
}