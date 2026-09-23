# A note on ahead-of-time compilation in the Todo app

Phil Whittington -- [@PDWhittington](https://x.com/PDWhittington), DMs open.

## Table of contents

- [Introduction](#introduction)
- [Replacement of JSON serializer](#replacement-of-json-serializer)
- [Dependency registration](#dependency-registration)
- [LibGit2Sharp](#libgit2sharp)


## Introduction

The Todo app is a CLI app that is invoked from the shell to execute a single, discrete command and then exit. Even before the app was enabled for ahead-of-time compilation and was a conventionally JIT-compiled C# app, each session would last, typically, a couple of hundred milliseconds, unless the command involved interfacing with a remote Git repository, in which case that duration might be a couple of seconds.


In any case, a large portion of the running time was consumed by the JIT-compiler creating native code from the IL that has been loaded from disk. One of the best arguments for JIT-compilation, aside from the cross-platform compatibility of the binary, is [tiered compiling](https://github.com/dotnet/runtime/blob/main/docs/design/features/tiered-compilation.md). This is far less of a consideration for an app of this kind because no session is ever long enough for the runtime compiler to find portions of the code that are 'hot' and to recompile them at a higher tier.

Given that binary cross-compatibility does not carry the same salience for open-source projects (one can always compile the code afresh for the target platform), it seemed that there was no reason not to pursue full ahead-of-time (AOT) compilation in a bid to make each todo command execute as quickly as possible.

This document outlines some of the changes I have made to the app so that it can be AOT-compiled.

## Replacement of JSON serializer

Prior to to my decision to pursue AOT compilation, I had selected [Utf8Json](https://github.com/neuecc/Utf8Json) on the strength of the fact that the library is zero-allocation and the benchmark statistics I had seen were favourable. While this library emits IL for fast serialisation and deserialisation, it does so by using reflection, which is not available in AOT-compiled binaries.

Since the library is no longer maintained, there was no possibility for me to submit any changes that would enable AOT-compatibility.

I decided, therefore, to switch to [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/api/system.text.json?view=net-11.0-pp), which is the built in JSON serialization library in .Net. From .Net 6 onwards, System.Text.Json has used a source generator so that the serisation and deserialisation IL code can be generated at compile time. This means, of course, that no IL is emitted at runtime and therefore no runtime reflection is required. Consequently the AOT step is performed after the de/serialisation code is already emitted.

As you can see, the code is almost identical, but the key difference with System.Text.Json is that a source generator runs at build-time.

<table>
<thead>
<tr>
<th></th>
<th>Approach</th>
<th>Short commit hash</th>
<th>File</th>
<th>Code</th>
</tr>
</thead>
<tbody>
<tr>
<td>
Before AOT
</td>
<td>
Using Utf8Json (not compatible with AOT)
</td>
<td>
<code>7e8151a</code>
</td>
<td>
<code>src/todo/StateAndConfig/ConfigurationProvider.cs</code>
</td>
<td>

```csharp
using Utf8Json;
using Utf8Json.Resolvers;
```
...
```csharp

private ConfigurationInfo GetConfiguration()
{   
    CompositeResolver.RegisterAndSetAsDefault(
        [new ColorFormatter()],
        [StandardResolver.Default]
    );
    
    var path =  _settingsPathProvider.GetSettingsPathInHierarchy().Path ??
                throw new FileNotFoundException($"{_constantsProvider.SettingsFileName} not found.",
                    _constantsProvider.SettingsFileName);

    using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
    
    var configuration = JsonSerializer.Deserialize<Configuration>(fileStream)
                        ?? throw new Exception($"Configuration could not be loaded from {path}");

    return ConfigurationInfo.Of(path, configuration);
}

```
</td>
</tr>
<tr height="0"> <!-- Circumvent Github's alternate row colouring -->
</tr>
<tr>
<td>
With AOT
</td>
<td>
Using AOT compilation (compatible with AOT)
</td>
<td>
<code>ebd2701</code>
</td>
<td>
<code>src/todo/StateAndConfig/ConfigurationProvider.cs</code>
</td>
<td>

```csharp
using System.Text.Json;
```
...
```csharp
private ConfigurationInfo GetConfiguration()
{   
    var path =  _settingsPathProvider.GetSettingsPathInHierarchy().Path ??
                throw new FileNotFoundException($"{_constantsProvider.SettingsFileName} not found.",
                    _constantsProvider.SettingsFileName);

    using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
    
    var configuration = JsonSerializer.Deserialize<Configuration>(fileStream, 
            AppJsonContext.Default.Configuration)
            ?? throw new Exception($"Configuration could not be loaded from {path}");

    return ConfigurationInfo.Of(path, configuration);
}
```
</td>
</tr>
</table>

## Dependency registration

Another key difference is with dependency registration. Prior to the introduction of AOT compilation, the where two cases in which I used reflection to determine, at runtime, the dependencies that should be registered.

The app uses a pattern of "CommandFactory" classes to read the command line supplied to the app and to determine if that string relates to a particular command. If it does, then the factory generates a command class, which is then resolved to a particular "CommandExecutor" class. In the cases of the CommandFactory and CommandExecutor classes, they were registered at start-up using reflection, because each of them either `ICommandFactory<CommandBase>` or `ICommandExecutor`.

<table>
<thead>
<tr>
<th></th>
<th>Approach</th>
<th>Short commit hash</th>
<th>File</th>
<th>Code</th>
</tr>
</thead>
<tbody>
<tr>
<td>
Before AOT
</td>
<td>
Using conventional reflection to register all classes that implement ICommandFactory<CommandBase> or ICommandExecutor.
</td>
<td>
<code>7e8151a</code>
</td>
<td>
<code>src/Todo/Initialise.cs</code>
</td>
<td>

```csharp
    public static IServiceCollection GetServiceCollection()
        => new ServiceCollection()
            .AddLogging()

            ...

            /* Command interpretation and execution */
            .AutoRegisterTypes<ICommandFactory<CommandBase>>()
            .AutoRegisterTypes<ICommandExecutor>()

            ...
            ;
```
...
```csharp
        extension(IServiceCollection serviceCollection)
        {
            private IServiceCollection AutoRegisterTypes<T>()
            {
                var typesToRegister = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x is { IsClass: true, IsAbstract: false })
                    .Where(x => x.IsAssignableTo(typeof(T)));

                foreach (var typeToRegister in typesToRegister)
                {
                    serviceCollection.AddSingleton(typeToRegister);
                    serviceCollection.AddSingleton(typeof(T), typeToRegister);
                }

                var interfacesToMap = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x is { IsInterface: true, IsGenericType: false } && x != typeof(T))
                    .Where(x => x.IsAssignableTo(typeof(T)));

                foreach (var interfaceToRegister in interfacesToMap)
                {
                    var typesAssignableToInterface = Assembly
                        .GetExecutingAssembly()
                        .GetTypes()
                        .Where(x => x is { IsClass: true, IsAbstract: false })
                        .Where(x => x.IsAssignableTo(interfaceToRegister));

                    foreach (var typeAssignableToInterface in typesAssignableToInterface)
                    {
                        serviceCollection.AddSingleton(interfaceToRegister,
                            x => x.GetRequiredService(typeAssignableToInterface));
                    }
                }

                return serviceCollection;
            }
```
</td>
</tr>
<tr height="0"> <!-- Circumvent Github's alternate row colouring -->
</tr>
<tr>
<td rowspan="7">
With AOT
</td>
<td rowspan="7">
Using a Rosylyn Analyzer to generate the dependency registration for each class explicitly at build-time
</td>
<td rowspan="7">
<code>ebd2701</code>
</td>
<td>
<code>src/Todo/Initialise.cs</code>
</td>
<td>

```csharp
    public static IServiceCollection GetServiceCollection()
        => new ServiceCollection()

            ...

            /* Command interpretation and execution */
            /* These two methods are auto-generated in the SourceGenerators project */
            .RegisterCommandFactories()
            .RegisterCommandExecutors()

            ...
            ;
```
</td>
</tr>
<tr height="0"> <!-- Circumvent Github's alternate row colouring -->
</tr>
<tr>
<td>
<code>src/Todo.SourceGenerators/BaseRegistrationGenerator.cs</code>
</td>
<td>
In this base class we implement IIncrementalGenerator and generate the dependency registration code that we need.
</td>
</tr>
<tr height="0"> <!-- Circumvent Github's alternate row colouring -->
</tr>
<tr>
<td>
<code>src/Todo.SourceGenerators/CommandExecutorRegistrationGenerator.cs</code>
</td>
<td>
This class derived from BaseRegistrationGenerator and generates the explicit C# code to register all classes in Todo which implement ICommandExecutor.
</td>
</tr>
<tr height="0"> <!-- Circumvent Github's alternate row colouring -->
</tr>
<tr>
<td>
<code>src/Todo.SourceGenerators/CommandFactoryRegistrationGenerator.cs</code>
</td>
<td>
This class derived from BaseRegistrationGenerator and generates the explicit C# code to register all classes in Todo which implement ICommandFactory.
</td>
</tr>
</table>

## LibGit2Sharp

As we have seen already with Utf8Json, perhaps the biggest challenge with AOT compilation is that many third-party libraries use features of the C# Framework that render them incompatible with any consuming application that is being compiled ahead-of-time. This is also true with [libgit2sharp](https://github.com/libgit2/libgit2sharp).

So far I have engaged in a number of workarounds such as compiling the library within the same solution as the Todo app, but obviously the best long-term fix is to ensure that the nuget library itself is AOT-compliant.

I have requested to be assigned this [issue](https://github.com/libgit2/libgit2sharp/issues/2160) and have already forked libgit2sharp [here](https://github.com/PDWhittington/libgit2sharp). 

As soon as I can I will create a pull request to fix this so that AOT works straight out of the box when the nuget package is consumed.

The issues all involve ensuring that the native calls into the underlying C git library (libgit2) are marshalled correctly. Different method decorators are used to ensure that the AOT compiler has the information it needs at build time.
