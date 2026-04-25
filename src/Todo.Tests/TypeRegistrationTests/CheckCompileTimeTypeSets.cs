// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using NUnit.Framework;
// using Todo;
// using Todo.Contracts.Services.Execution;
//
// namespace Todo.Tests.TypeRegistrationTests;
//
// [TestFixture]
// public class CheckCompileTimeTypeSets
// {
//     // [Test]
//     // public static void CheckFactoryRegistration()
//     // {
//     //     CheckGenericRegistration(typeof(ICommandFactory<>), "GetCommandFactories");
//     // }
//
//     [Test]
//     public static void CheckExecutorRegistration()
//     {
//         CheckInterfaceRegistration(typeof(ICommandExecutor), "GetCommandExecutors");
//     }
//
//     [Test]
//     public static void CheckExecutorsAndInterfaces()
//     {
//         var compileTimeSetMethod = typeof(Initialise).GetMethod("GetCommandExecutorsAndInterfaces", 
//             BindingFlags.Static | BindingFlags.NonPublic)!;
//         
//         var compileTimeTypes = ((IEnumerable<Todo.Initialise.InterfaceAndImplementation>)compileTimeSetMethod.Invoke(null, null)!)
//             .ToArray();
//         
//         var compileTimeDictionary = compileTimeTypes
//             .GroupBy(x => x.Interface)
//             .ToDictionary(x => x.Key, 
//                 x => x.Select(y => y.Implementation).ToArray());
//
//         var compileTimeInterfaces = compileTimeTypes
//             .Select(ct => ct.Interface)
//             .ToHashSet();
//
//         var interfacesToMap = GetTypes()
//             .Where(x => x is { IsInterface: true, IsGenericType: false } && x != typeof(ICommandExecutor))
//             .Where(x => x.IsAssignableTo(typeof(ICommandExecutor)))
//             .ToArray();
//         
//         Assert.IsTrue(compileTimeInterfaces.SetEquals(interfacesToMap));
//         
//         foreach (var interfaceToRegister in interfacesToMap)
//         {
//             var compileTimeImplementations = compileTimeDictionary[interfaceToRegister];
//
//             var typesAssignableToInterface = GetTypes()
//                 .Where(x => x is { IsClass: true, IsAbstract: false })
//                 .Where(x => x.IsAssignableTo(interfaceToRegister))
//                 .ToHashSet();
//
//             Assert.IsTrue(typesAssignableToInterface.SetEquals(compileTimeImplementations));
//         }
//     }
//
//     private static void CheckInterfaceRegistration(Type type, string compileTimeTypeSetMethod)
//     {
//         var compileTimeSetMethod = typeof(Initialise).GetMethod(compileTimeTypeSetMethod, 
//             BindingFlags.Static | BindingFlags.NonPublic)!;
//         
//         var compileTimeTypes = ((IEnumerable<Type>)compileTimeSetMethod.Invoke(null, null)!)
//             .ToArray();
//
//         var reflectedTypes = typeof(Initialise).Assembly.GetTypes()
//             .Where(x => x is { IsClass: true, IsAbstract: false })
//             .Where(t => !t.IsGenericTypeDefinition)                         // skip open generic classes (usually not useful here)
//             .Where(t => t.IsAssignableTo(type))
//             .ToHashSet();
//         
//         Assert.IsTrue(reflectedTypes.SetEquals(compileTimeTypes));
//     }
//     
//     private static void CheckGenericRegistration(Type type, string compileTimeTypeSetMethod)
//     {
//         var compileTimeSetMethod = typeof(Initialise).GetMethod(compileTimeTypeSetMethod, 
//             BindingFlags.Static | BindingFlags.NonPublic)!;
//         
//         var compileTimeTypes = ((IEnumerable<Type>)compileTimeSetMethod.Invoke(null, null)!)
//             .ToArray();
//
//         var reflectedTypes = typeof(Initialise).Assembly.GetTypes()
//             .Where(x => x is { IsClass: true, IsAbstract: false })
//             .Where(t => !t.IsGenericTypeDefinition)                         // skip open generic classes (usually not useful here)
//             .Where(t => ImplementsOpenGenericInterface(t, type))
//             .ToHashSet();
//         
//         Assert.IsTrue(reflectedTypes.SetEquals(compileTimeTypes));
//     }
//     
//     private static bool ImplementsOpenGenericInterface(Type type, Type openGenericInterface)
//     {
//         // Check the type itself (in case it's a generic class implementing it directly)
//         if (type.IsGenericType && 
//             type.GetGenericTypeDefinition() == openGenericInterface)
//             return true;
//
//         // Check all implemented interfaces
//         return type.GetInterfaces()
//             .Any(i => i.IsGenericType && 
//                       i.GetGenericTypeDefinition() == openGenericInterface);
//     }
//
//     private static IEnumerable<Type> GetTypes()
//     {
//         var mainAssembly = Assembly.Load("Todo");
//         var contractsAssembly = Assembly.Load("Todo.Contracts");
//
//         return new[] { mainAssembly, contractsAssembly }
//             .SelectMany(x => x.GetTypes());
//     }
//     
// }