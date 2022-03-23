using System;

namespace Todo.Contracts.Data.FileSystem;

[Flags]
public enum ListFileTypeEnum
{
    DayList = 1,
    TopicList = 2
}
