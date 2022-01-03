using System;
using NUnit.Framework;
using Todo.DateNaming;

namespace TodoTests;

public class EasterTests
{
    static class DateConstants
    {
        public const string MaundyThursday2017 = "2017-04-13";
        public const string GoodFriday2017 = "2017-04-14";
        public const string HolySaturday2017 = "2017-04-15";
        public const string EasterDay2017 = "2017-04-16";
        public const string EasterMonday2017 = "2017-04-17";
        
        public const string MaundyThursday2018 = "2018-03-29";
        public const string GoodFriday2018 = "2018-03-30";
        public const string HolySaturday2018 = "2018-03-31";
        public const string EasterDay2018 = "2018-04-01";
        public const string EasterMonday2018 = "2018-04-02";
        
        public const string MaundyThursday2019 = "2019-04-18";
        public const string GoodFriday2019 = "2019-04-19";
        public const string HolySaturday2019 = "2019-04-20";
        public const string EasterDay2019 = "2019-04-21";
        public const string EasterMonday2019 = "2019-04-22";
        
        public const string MaundyThursday2020 = "2020-04-09";
        public const string GoodFriday2020 = "2020-04-10";
        public const string HolySaturday2020 = "2020-04-11";
        public const string EasterDay2020 = "2020-04-12";
        public const string EasterMonday2020 = "2020-04-13";
        
        public const string MaundyThursday2021 = "2021-04-01";
        public const string GoodFriday2021 = "2021-04-02";
        public const string HolySaturday2021 = "2021-04-03";
        public const string EasterDay2021 = "2021-04-04";
        public const string EasterMonday2021 = "2021-04-05";
        
        public const string MaundyThursday2022 = "2022-04-14";
        public const string GoodFriday2022 = "2022-04-15";
        public const string HolySaturday2022 = "2022-04-16";
        public const string EasterDay2022 = "2022-04-17";
        public const string EasterMonday2022 = "2022-04-18";
        
        public const string MaundyThursday2023 = "2023-04-06";
        public const string GoodFriday2023 = "2023-04-07";
        public const string HolySaturday2023 = "2023-04-08";
        public const string EasterDay2023 = "2023-04-09";
        public const string EasterMonday2023 = "2023-04-10";

    }

    static class DayNames
    {
        public const string MaundyThursday = "Maundy Thursday";
        public const string GoodFriday = "Good Friday";
        public const string HolySaturday = "Holy Saturday";
        public const string EasterDay = "Easter Day";
        public const string EasterMonday = "Easter Monday";
    }
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase(DateConstants.MaundyThursday2017, DayNames.MaundyThursday)]
    [TestCase(DateConstants.GoodFriday2017, DayNames.GoodFriday)]
    [TestCase(DateConstants.HolySaturday2017, DayNames.HolySaturday)]
    [TestCase(DateConstants.EasterDay2017, DayNames.EasterDay)]
    [TestCase(DateConstants.EasterMonday2017, DayNames.EasterMonday)]
    [TestCase(DateConstants.MaundyThursday2018, DayNames.MaundyThursday)]
    [TestCase(DateConstants.GoodFriday2018, DayNames.GoodFriday)]
    [TestCase(DateConstants.HolySaturday2018, DayNames.HolySaturday)]
    [TestCase(DateConstants.EasterDay2018, DayNames.EasterDay)]
    [TestCase(DateConstants.EasterMonday2018, DayNames.EasterMonday)]
    [TestCase(DateConstants.MaundyThursday2019, DayNames.MaundyThursday)]
    [TestCase(DateConstants.GoodFriday2019, DayNames.GoodFriday)]
    [TestCase(DateConstants.HolySaturday2019, DayNames.HolySaturday)]
    [TestCase(DateConstants.EasterDay2019, DayNames.EasterDay)]
    [TestCase(DateConstants.EasterMonday2019, DayNames.EasterMonday)]
    [TestCase(DateConstants.MaundyThursday2020, DayNames.MaundyThursday)]
    [TestCase(DateConstants.GoodFriday2020, DayNames.GoodFriday)]
    [TestCase(DateConstants.HolySaturday2020, DayNames.HolySaturday)]
    [TestCase(DateConstants.EasterDay2020, DayNames.EasterDay)]
    [TestCase(DateConstants.EasterMonday2020, DayNames.EasterMonday)]
    [TestCase(DateConstants.MaundyThursday2021, DayNames.MaundyThursday)]
    [TestCase(DateConstants.GoodFriday2021, DayNames.GoodFriday)]
    [TestCase(DateConstants.HolySaturday2021, DayNames.HolySaturday)]
    [TestCase(DateConstants.EasterDay2021, DayNames.EasterDay)]
    [TestCase(DateConstants.EasterMonday2021, DayNames.EasterMonday)]
    [TestCase(DateConstants.MaundyThursday2022, DayNames.MaundyThursday)]
    [TestCase(DateConstants.GoodFriday2022, DayNames.GoodFriday)]
    [TestCase(DateConstants.HolySaturday2022, DayNames.HolySaturday)]
    [TestCase(DateConstants.EasterDay2022, DayNames.EasterDay)]
    [TestCase(DateConstants.EasterMonday2022, DayNames.EasterMonday)]
    [TestCase(DateConstants.MaundyThursday2023, DayNames.MaundyThursday)]
    [TestCase(DateConstants.GoodFriday2023, DayNames.GoodFriday)]
    [TestCase(DateConstants.HolySaturday2023, DayNames.HolySaturday)]
    [TestCase(DateConstants.EasterDay2023, DayNames.EasterDay)]
    [TestCase(DateConstants.EasterMonday2023, DayNames.EasterMonday)]
    public void TestEaster(string date, string nameExpected)
    {
        var dateParsed = DateOnly.Parse(date);
        
        var dateNamer = new EasterDateNamer();
        
        var result = dateNamer.TryGetSpecialName(dateParsed, out var name);
        
        Assert.AreEqual(name != null, result);
        Assert.AreEqual(nameExpected, name);
    }
}