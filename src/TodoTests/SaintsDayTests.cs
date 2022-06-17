using System;
using NUnit.Framework;
using Todo.Dates.Naming;

namespace TodoTests;

public class SaintsDayTests
{
    private static class DateConstants
    {
        public const string StDavidsDay2017 = "2017-03-01";
        public const string StPatricksDay2017 = "2017-03-17";
        public const string StGeorgesDay2017 = "2017-04-23";
        public const string StAndrewsDay2017 = "2017-11-30";

        public const string StDavidsDay2018 = "2018-03-01";
        public const string StPatricksDay2018 = "2018-03-17";
        public const string StGeorgesDay2018 = "2018-04-23";
        public const string StAndrewsDay2018 = "2018-11-30";

        public const string StDavidsDay2019 = "2019-03-01";
        public const string StPatricksDay2019 = "2019-03-17";
        public const string StGeorgesDay2019 = "2019-04-23";
        public const string StAndrewsDay2019 = "2019-11-30";

        public const string StDavidsDay2020 = "2020-03-01";
        public const string StPatricksDay2020 = "2020-03-17";
        public const string StGeorgesDay2020 = "2020-04-23";
        public const string StAndrewsDay2020 = "2020-11-30";

        public const string StDavidsDay2021 = "2021-03-01";
        public const string StPatricksDay2021 = "2021-03-17";
        public const string StGeorgesDay2021 = "2021-04-23";
        public const string StAndrewsDay2021 = "2021-11-30";
    }

    private static class DayNames
    {
        public const string StDavidsDay = "St. David's Day";
        public const string StPatricksDay = "St. Patrick's Day";
        public const string StGeorgesDay = "St. George's Day";
        public const string StAndrewsDay = "St. Andrew's Day";
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase(DateConstants.StDavidsDay2017, DayNames.StDavidsDay)]
    [TestCase(DateConstants.StPatricksDay2017, DayNames.StPatricksDay)]
    [TestCase(DateConstants.StGeorgesDay2017, DayNames.StGeorgesDay)]
    [TestCase(DateConstants.StAndrewsDay2017, DayNames.StAndrewsDay)]
    [TestCase(DateConstants.StDavidsDay2018, DayNames.StDavidsDay)]
    [TestCase(DateConstants.StPatricksDay2018, DayNames.StPatricksDay)]
    [TestCase(DateConstants.StGeorgesDay2018, DayNames.StGeorgesDay)]
    [TestCase(DateConstants.StAndrewsDay2018, DayNames.StAndrewsDay)]
    [TestCase(DateConstants.StDavidsDay2019, DayNames.StDavidsDay)]
    [TestCase(DateConstants.StPatricksDay2019, DayNames.StPatricksDay)]
    [TestCase(DateConstants.StGeorgesDay2019, DayNames.StGeorgesDay)]
    [TestCase(DateConstants.StAndrewsDay2019, DayNames.StAndrewsDay)]
    [TestCase(DateConstants.StDavidsDay2020, DayNames.StDavidsDay)]
    [TestCase(DateConstants.StPatricksDay2020, DayNames.StPatricksDay)]
    [TestCase(DateConstants.StGeorgesDay2020, DayNames.StGeorgesDay)]
    [TestCase(DateConstants.StAndrewsDay2020, DayNames.StAndrewsDay)]
    [TestCase(DateConstants.StDavidsDay2021, DayNames.StDavidsDay)]
    [TestCase(DateConstants.StPatricksDay2021, DayNames.StPatricksDay)]
    [TestCase(DateConstants.StGeorgesDay2021, DayNames.StGeorgesDay)]
    [TestCase(DateConstants.StAndrewsDay2021, DayNames.StAndrewsDay)]
    public void TestSaintsDays(string date, string nameExpected)
    {
        var dateParsed = DateOnly.Parse(date);

        var dateNamer = new SaintsDayDateNamer();

        var result = dateNamer.TryGetSpecialName(dateParsed, out var name);

        Assert.AreEqual(name is not null, result);
        Assert.AreEqual(nameExpected, name);
    }
}
