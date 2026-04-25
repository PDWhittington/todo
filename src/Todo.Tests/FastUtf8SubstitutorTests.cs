using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;
using Todo.Contracts.Data.Memory;
using Todo.StringOperations;

namespace Todo.Tests;

internal static class StringOperationsExtensions
{
    
    public static UnmanagedByteArray ToUnmanagedArray (this string str)
    {
        var data  = GC.AllocateArray<byte>(str.Length, pinned: true);

        for (var i = 0; i < str.Length; i++)
        {
            data[i] = Convert.ToByte(str[i]);
        }

        // 4. Permanently pin and get a stable pointer
        var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        var pointer = handle.AddrOfPinnedObject();
        
        return UnmanagedByteArray.Of(handle, pointer, str.Length);
    }
        
}

internal static class Create
{
    public static MemoryStream MemoryStream () => new();
}


[TestFixture]
public class FastUtf8SubstitutorTests
{
    private readonly FastUtf8Substitutor _substitutor =  new();
    
    [Test]
    public void NoReplacements()
    {
        var template = "The quick brown fox jumped over the lazy dog.".ToUnmanagedArray();

        var substitutions = new Dictionary<string, string>();

        var stream = Create.MemoryStream();
        
        _substitutor.CopyToStream(template, substitutions, stream);
        
        var resultArr = stream.ToArray();
        var resultStr = Encoding.UTF8.GetString(resultArr);

        const string expected = "The quick brown fox jumped over the lazy dog.";
        
        Assert.AreEqual(expected, resultStr);
    }
    
    [Test]
    public void OneKey()
    {
        var template = "The quick {colour} fox jumped over the lazy dog.".ToUnmanagedArray();

        var substitutions = new Dictionary<string, string>
        {
            { "colour", "brown" }
        };

        var stream = Create.MemoryStream();
        _substitutor.CopyToStream(template, substitutions, stream);
        
        var resultArr = stream.ToArray();
        var resultStr = Encoding.UTF8.GetString(resultArr);

        const string expected = "The quick brown fox jumped over the lazy dog.";
        
        Assert.AreEqual(expected, resultStr);
    }
    
    [Test]
    public void SeveralKeys()
    {
        var template = "The quick {colour} {animal} {verb} over the lazy dog.".ToUnmanagedArray();

        var substitutions = new Dictionary<string, string>
        {
            { "colour", "brown" },
            { "animal", "fox" },
            { "verb", "jumped" }
        };

        var stream = Create.MemoryStream();
        _substitutor.CopyToStream(template, substitutions, stream);
        
        var resultArr = stream.ToArray();
        var resultStr = Encoding.UTF8.GetString(resultArr);

        const string expected = "The quick brown fox jumped over the lazy dog.";
        
        Assert.AreEqual(expected, resultStr);
    }
    
    [Test]
    public void FalseOpenBracket()
    {
        var template = "The {quick brown fox over the lazy dog.".ToUnmanagedArray();

        var substitutions = new Dictionary<string, string>
        {
            { "quick", "!!!!!" }
        };

        var stream = Create.MemoryStream();
        _substitutor.CopyToStream(template, substitutions, stream);
        
        var resultArr = stream.ToArray();
        var resultStr = Encoding.UTF8.GetString(resultArr);

        const string expected = "The {quick brown fox over the lazy dog.";
        
        Assert.AreEqual(expected, resultStr);
    }
    
    [Test]
    public void FalseKey()
    {
        //Keys containing whitespace are not allowed
        var template = "The {quick brown} fox over the lazy dog.".ToUnmanagedArray();

        var substitutions = new Dictionary<string, string>
        {
            { "quick brown", "!!!!!" }
        };

        var stream = Create.MemoryStream();
        _substitutor.CopyToStream(template, substitutions, stream);
        
        var resultArr = stream.ToArray();
        var resultStr = Encoding.UTF8.GetString(resultArr);

        const string expected = "The {quick brown} fox over the lazy dog.";
        
        Assert.AreEqual(expected, resultStr);
    }
    
        
    [Test]
    public void NestedKey()
    {
        //Keys containing whitespace are not allowed
        var template = "The {quick {colour} fox} over the lazy dog.".ToUnmanagedArray();

        var substitutions = new Dictionary<string, string>
        {
            { "colour", "brown" }
        };

        var stream = Create.MemoryStream();
        _substitutor.CopyToStream(template, substitutions, stream);
        
        var resultArr = stream.ToArray();
        var resultStr = Encoding.UTF8.GetString(resultArr);

        const string expected = "The {quick brown fox} over the lazy dog.";
        
        Assert.AreEqual(expected, resultStr);
    }
    
    [Test]
    public void KeyAtStart()
    {
        var template = "{article} quick brown fox over the lazy dog.".ToUnmanagedArray();

        var substitutions = new Dictionary<string, string>
        {
            { "article", "The" }
        };

        var stream = Create.MemoryStream();
        _substitutor.CopyToStream(template, substitutions, stream);
        
        var resultArr = stream.ToArray();
        var resultStr = Encoding.UTF8.GetString(resultArr);

        const string expected = "The quick brown fox over the lazy dog.";
        
        Assert.AreEqual(expected, resultStr);
    }
        
    [Test]
    public void KeyAtEnd()
    {
        var template = "The quick brown fox over the lazy dog{punctuation}".ToUnmanagedArray();

        var substitutions = new Dictionary<string, string>
        {
            { "punctuation", "." }
        };

        var stream = Create.MemoryStream();
        _substitutor.CopyToStream(template, substitutions, stream);
        
        var resultArr = stream.ToArray();
        var resultStr = Encoding.UTF8.GetString(resultArr);

        const string expected = "The quick brown fox over the lazy dog.";
        
        Assert.AreEqual(expected, resultStr);
    }
    
    [Test]
    public void KeyAtBothEnds()
    {
        var template = "{article} quick brown fox over the lazy dog{punctuation}".ToUnmanagedArray();

        var substitutions = new Dictionary<string, string>
        {
            { "article", "The" },
            { "punctuation", "." }
        };

        var stream = Create.MemoryStream();
        _substitutor.CopyToStream(template, substitutions, stream);
        
        var resultArr = stream.ToArray();
        var resultStr = Encoding.UTF8.GetString(resultArr);

        const string expected = "The quick brown fox over the lazy dog.";
        
        Assert.AreEqual(expected, resultStr);
    }
}