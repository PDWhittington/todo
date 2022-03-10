# todo &mdash; an idiosyncratic todo list manager

This project is a small C# solution which manages todo lists written in Markdown.

Over the next couple of weeks, I will flesh out this Readme file with a description of how it should be used and a full list of features.

In the meantime, build the app and run `todo help` from the command line to see a full list of commands.

Please feel free to DM me on Twitter, [@PDWhittington](https://twitter.com/PDWhittington), or to add issues for any features you would like to see.

Thanks,

Phil Whittington

<table>
<tr>
<th>
Command
</th>
<th>
Description
</th>
</tr>
<tr>
<td>
createorshow
</td>
<td>
Creates or shows a markdown file for the date supplied. This is the default command and can be executed by typing anything that can be parsed as a date. Supplying no date assumes the current day.<br/><br/>                                                             
Usage: todo [date]                                           
</td>
</tr>
<tr>
<td>
a<br/>
archive
</td>
<td>
Archives the markdown file for a given date. The file is moved into the archive subfolder of the main todo folder. The name of the archive folder is specified in settings.json. Also in settings.json can be specified whether the file is moved simply in the file system, or by using git mv.<br/><br/>
Usage: todo a [date]
</td>
</tr>
<tr>
<td>
c<br/>
commit
</td>
<td>
Gathers the current modifications into a commit. Commit       
message is optional.                                          
                                                              
Usage: todo c [commit message]                                
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<tr>
<td>
</td>
<td>
</td>
</tr>
<table>

| c                | Gathers the current modifications into a commit. Commit       |
| commit           | message is optional.                                          |
|                  |                                                               |
|                  | Usage: todo c [commit message]                                |
| ---------------- | ------------------------------------------------------------- |
| h                | Opens the browser specified in the settings file and loads    |
| html             | the Html file for the given date.                             |
| showhtml         |                                                               |
|                  | Usage: todo h [date]                                          |
| ---------------- | ------------------------------------------------------------- |
| help             | Displays this help screen.                                    |
| about            |                                                               |
|                  | Usage: todo help                                              |
| ---------------- | ------------------------------------------------------------- |
| i                | Initialises the current folder with a default                 |
| init             | todo-settings.json file                                       |
|                  |                                                               |
|                  | Usage: todo init                                              |
| ---------------- | ------------------------------------------------------------- |
| k                | Deletes all the html files in the todo folder and the archive |
| killhtml         | subfolder                                                     |
|                  |                                                               |
|                  | Usage: todo k                                                 |
| ---------------- | ------------------------------------------------------------- |
| l                | Provides a list of all todo lists. Switches are as follows:-  |
| list             | m -- main todo folder.                                        |
|                  | a -- archive folder.                                          |
|                  | d -- lists relating to days.                                  |
|                  | t -- lists relating to topics.                                |
|                  |                                                               |
|                  | Usage: todo l [m                                              | a][d | t] |
| ---------------- | ------------------------------------------------------------- |
| p                | Converts a Markdown file to HTML. Can be used with anything   |
| print            | that can be parsed as a date. Supplying no date performs this |
| printhtml        | operation on the Markdown file for the current day.           |
|                  |                                                               |
|                  | Usage: todo p [date]                                          |
| ---------------- | ------------------------------------------------------------- |
| ph               | This command is equivalent to printhtml followed by showhtml  |
| printandshowhtml | (p, h).                                                       |
|                  |                                                               |
|                  | Usage: todo ph [date]                                         |
| ---------------- | ------------------------------------------------------------- |
| push             | Executes a git push.                                          |
|                  |                                                               |
|                  | Usage: todo push                                              |
| ---------------- | ------------------------------------------------------------- |
| rm               | Deletes the file. If git is enabled, the command performs a   |
| remove           | remove in git.                                                |
| delete           |                                                               |
|                  | Usage: todo rm [date]                                         |
| ---------------- | ------------------------------------------------------------- |
| s                | Executes a commit and push operation sequentially.            |
| sync             |                                                               |
|                  | Usage: todo s [commit message]                                |
| ---------------- | ------------------------------------------------------------- |
| sc               | Opens in the text editor all of the files for which conflicts |
| showconflicts    | exist                                                         |
|                  |                                                               |
|                  | Usage: todo sc                                                |
| ---------------- | ------------------------------------------------------------- |
| settings         | Shows the settings file in the default editor.                |
| showsettings     |                                                               |
|                  | Usage: todo settings                                          |
| ---------------- | ------------------------------------------------------------- |
| t                | Creates or shows a todo list relating to a single topic.      |
| topic            |                                                               |
|                  | Usage: todo t (topic name)                                    |
| ---------------- | ------------------------------------------------------------- |
| u                | Un-archives the markdown file for a given date. The file is   |
| unarchive        | moved form the subfolder back to the main todo folder. The    |
|                  | name of the archive folder is specified in settings.json.     |
|                  | Also in settings.json can be specified whether the file is    |
|                  | moved simply in the file system, or by using git mv.          |
|                  |                                                               |
|                  | Usage: todo u [date]                                          |
| ---------------- | ------------------------------------------------------------- |


Notes:

createorshow is the default command. This means it can be accessed simply by
typing anything that can be parsed as a date after the word todo.

Valid date formats:-

   "y", "yesterday"  yesterday
   [empty string], ".", "today"  today
   "tm", "tomorrow"  tomorrow
   [day]  maps to the day/month/year which is nearest in time to today
   [day]/[month]  maps to the day/month which is nearest in time to today
   +[daycount]  positive offset a number of days from today
   -[daycount]  negative offset a number of days from today

[Commit Message]  In the Commit and Sync commands, the commit message is
optional. If none is supplied, then a standard message detailing date and time
of the commit will be used.

