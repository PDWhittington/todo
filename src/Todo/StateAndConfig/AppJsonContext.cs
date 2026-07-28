using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Data.Html;

namespace Todo.StateAndConfig;


[JsonSourceGenerationOptions(
  UseStringEnumConverter = true,
  WriteIndented = true,           // or true if you want pretty-printed output
  ReadCommentHandling = JsonCommentHandling.Skip)]   // or your preferred defaults
[JsonSerializable(typeof(Configuration))]             // the class that contains the Color property
[JsonSerializable(typeof(PerOsLaunchInfos))]
[JsonSerializable(typeof(ScoreCategory))]
[JsonSerializable(typeof(TodoListInfo))]
[JsonSerializable(typeof(TimeSpan))]
[JsonSerializable(typeof(HtmlThemeEnum))]
[JsonSerializable(typeof(IterationMethodEnum))]
[JsonSerializable(typeof(Color))]                       // important for the converter
public partial class AppJsonContext : JsonSerializerContext;