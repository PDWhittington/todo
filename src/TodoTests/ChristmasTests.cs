using System;
using NUnit.Framework;
using Todo.DateNaming;

namespace TodoTests;

public class ChristmasTests
{
    static class DateConstants
    {
        public const string ChristmasEve2017 = "2017-12-24";
        public const string ChristmasDay2017 = "2017-12-25";
        public const string BoxingDay2017 = "2017-12-26";
        public const string NewYearsEve2017 = "2017-12-31";
        public const string NewYearsDay2018 = "2018-01-01";
        public const string Epiphany2018 = "2018-01-06";
        
        public const string ChristmasEve2018 = "2018-12-24";
        public const string ChristmasDay2018 = "2018-12-25";
        public const string BoxingDay2018 = "2018-12-26";
        public const string NewYearsEve2018 = "2018-12-31";
        public const string NewYearsDay2019 = "2019-01-01";
        public const string Epiphany2019 = "2019-01-06";
        
        public const string ChristmasEve2019 = "2019-12-24";
        public const string ChristmasDay2019 = "2019-12-25";
        public const string BoxingDay2019 = "2019-12-26";
        public const string NewYearsEve2019 = "2019-12-31";
        public const string NewYearsDay2020 = "2020-01-01";
        public const string Epiphany2020 = "2020-01-06";

    }

    static class DayNames
    {
        public const string ChristmasEve = "Christmas Eve";
        public const string ChristmasDay = "Christmas Day";
        public const string BoxingDay = "Boxing Day";
        public const string NewYearsEve = "New Year's Eve";
        public const string NewYearsDay = "New Year's Day";
        public const string Epiphany = "Epiphany";
    }
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase(DateConstants.ChristmasEve2017, DayNames.ChristmasEve)]
    [TestCase(DateConstants.ChristmasDay2017, DayNames.ChristmasDay)]
    [TestCase(DateConstants.BoxingDay2017, DayNames.BoxingDay)]
    [TestCase(DateConstants.NewYearsEve2017, DayNames.NewYearsEve)]
    [TestCase(DateConstants.NewYearsDay2018, DayNames.NewYearsDay)]
    [TestCase(DateConstants.Epiphany2018, DayNames.Epiphany)]
    [TestCase(DateConstants.ChristmasEve2018, DayNames.ChristmasEve)]
    [TestCase(DateConstants.ChristmasDay2018, DayNames.ChristmasDay)]
    [TestCase(DateConstants.BoxingDay2018, DayNames.BoxingDay)]
    [TestCase(DateConstants.NewYearsEve2018, DayNames.NewYearsEve)]
    [TestCase(DateConstants.NewYearsDay2019, DayNames.NewYearsDay)]
    [TestCase(DateConstants.Epiphany2019, DayNames.Epiphany)]
    [TestCase(DateConstants.ChristmasEve2019, DayNames.ChristmasEve)]
    [TestCase(DateConstants.ChristmasDay2019, DayNames.ChristmasDay)]
    [TestCase(DateConstants.BoxingDay2019, DayNames.BoxingDay)]
    [TestCase(DateConstants.NewYearsEve2019, DayNames.NewYearsEve)]
    [TestCase(DateConstants.NewYearsDay2020, DayNames.NewYearsDay)]
    [TestCase(DateConstants.Epiphany2020, DayNames.Epiphany)]
    public void TestChristmasAndNewYear(string date, string nameExpected)
    {
        var dateParsed = DateOnly.Parse(date);
        
        var dateNamer = new ChristmasNewYearDateNamer();
        
        var result = dateNamer.TryGetName(dateParsed, out var name);
        
        Assert.AreEqual(name != null, result);
        Assert.AreEqual(nameExpected, name);
    }
}