using System.Collections.Generic;
using NUnit.Framework;
using Todo.Contracts.StringOperations;

namespace Todo.Tests;

[TestFixture]
public class TextExtensionsTests
{
    public class TestCase
    {
        public byte A { get; init; }
        public byte B { get; init; }
        public bool Expected { get; init; }
    }

    [Test]
    [TestCaseSource(nameof(GetEqualsIgnoreCaseTests))]
    public void EqualsIgnoreCase(TestCase testCase)
    {
        Assert.AreEqual(testCase.Expected, testCase.A.EqualsIgnoreCase(testCase.B));
    }

    public static IEnumerable<TestCase> GetEqualsIgnoreCaseTests()
    {
        yield return new TestCase
        {
            A = (byte)'a',
            B = (byte)'A',
            Expected = true
        };

        yield return new TestCase
        {
            A = (byte)'A',
            B = (byte)'a',
            Expected = true
        };

        yield return new TestCase
        {
            A = (byte)'z',
            B = (byte)'Z',
            Expected = true
        };

        yield return new TestCase
        {
            A = (byte)'Z',
            B = (byte)'z',
            Expected = true
        };
    }
}