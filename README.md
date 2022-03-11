# todo &mdash; an idiosyncratic todo list manager

## Introduction

This project is a lightweight C# solution which manages todo lists written in Markdown.

Please feel free to DM me on Twitter, [@PDWhittington](https://twitter.com/PDWhittington), or to add issues on this repo for any features you would like to see.

Thanks,

Phil Whittington

## How to build

Install git and ensure that it is in your PATH environment variable. To clone this repository, type the following at a command-line:

```
git clone https://github.com/PDWhittington/todo.git
```
You will also need a recent .Net SDK installed. I am on .Net 6.0. To build the app, run something like the following. This will build the app so that it can run on all target operating systems. One dependency, gitlib2sharp, has OS-specific dependencies (i.e. the .Net assembly wraps a native library which varies from platform to platform). Building for all target operating systems will ensure that all the native libraries are produced. This means that the app can run on Windows, OSX or Linux.


```
cd todo
dotnet build src
```
If you are confident that you need the app to run on only one operating system, you can publish the app for just the [runtime identifier](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog#windows-rids) that you need.


```
dotnet publish src --runtime win-x64 --configuration Release --no-self-contained
```

```
dotnet publish src --runtime osx-x64 --configuration Release --no-self-contained
```

```
dotnet publish src --runtime linux-x64 --configuration Release --no-self-contained
```

## Usage

Once you have the app, add its published location to your PATH environment variable.

The following commands can be invoked from the command line:-

<style>
    td {vertical-align: top}
</style>

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
            (No text)<br/>
            createorshow
        </td>
        <td>
            Creates or shows a markdown file for the date supplied. This is the default command and can be executed by
            typing anything that can be parsed as a date. Supplying no date assumes the current day.<br /><br />
            Usage: todo [date]
        </td>
    </tr>
    <tr>
        <td>
            a<br />
            archive
        </td>
        <td>
            Archives the markdown file for a given date. The file is moved into the archive subfolder of the main todo
            folder. The name of the archive folder is specified in settings.json. Also in settings.json can be specified
            whether the file is moved simply in the file system, or by using git mv.<br /><br />
            Usage: todo a [date]
        </td>
    </tr>
    <tr>
        <td>
            c<br />
            commit
        </td>
        <td>
            Gathers the current modifications into a commit. Commit message is optional.<br /><br />
            Usage: todo c [commit message]
        </td>
    </tr>
    <tr>
        <td>
            h<br />
            html<br />
            showhtml
        </td>
        <td>
            Opens the browser specified in the settings file and loads the Html file for the given date. <br /><br />
            Usage: todo h [date]
        </td>
    </tr>
    <tr>
        <td>
            help<br />
            about
        </td>
        <td>
            Displays this help screen.<br /><br />
            Usage: todo help
        </td>
    </tr>
    <tr>
        <td>
            i<br />
            init
        </td>
        <td>
            Initialises the current folder with a default todo-settings.json file.<br /><br />
            Usage: todo init
        </td>
    </tr>
    <tr>
        <td>
            k<br />
            killhtml
        </td>
        <td>
            Deletes all the html files in the todo folder and the archive subfolder<br /><br />
            Usage: todo k
        </td>
    </tr>
    <tr>
        <td>
            l<br />
            list
        </td>
        <td>
            Provides a list of all todo lists. Switches are as follows:-<br /><br />
            <ul>
                <li>m -- main todo folder.</li>
                <li>a -- archive folder.</li>
                <li>d -- lists relating to days.</li>
                <li>t -- lists relating to topics.</li>
            </ul><br />
            Usage: todo l [m | a] [d | t]
        </td>
    </tr>
    <tr>
        <td>
            p<br />
            print<br />
            printhtml<br />
        </td>
        <td>
            Converts a Markdown file to HTML. Can be used with anything that can be parsed as a date. Supplying no date
            performs this operation on the Markdown file for the current day.<br /><br />
            Usage: todo p [date]
        </td>
    </tr>
    <tr>
        <td>
            ph<br />
            printandshowhtml
        </td>
        <td>
            This command is equivalent to printhtml followed by showhtml (p, h).<br /><br />
            Usage: todo ph [date]
        </td>
    </tr>
    <tr>
        <td>
            push
        </td>
        <td>
            Executes a git push.<br /><br />
            Usage: todo push
        </td>
    </tr>
    <tr>
        <td>
            rm<br />
            remove<br />
            delete
        </td>
        <td>
            Deletes the file. If git is enabled, the command performs a
            remove in git.<br /><br />
            Usage: todo rm [date]
        </td>
    </tr>
    <tr>
        <td>
            s<br />
            sync
        </td>
        <td>
            Executes a commit and push operation sequentially.<br /><br />
            Usage: todo s [commit message]
        </td>
    </tr>
    <tr>
        <td>
            sc<br />
            showconflicts
        </td>
        <td>
            Opens in the text editor all of the files for which conflicts exist<br /><br />
            Usage: todo sc
        </td>
    </tr>
    <tr>
        <td>
            settings<br />
            showsettings
        </td>
        <td>
            Shows the settings file in the default editor.<br /><br />
            Usage: todo settings
        </td>
    </tr>
    <tr>
        <td>
            t<br />
            topic
        </td>
        <td>
            Creates or shows a todo list relating to a single topic.<br /><br />
            Usage: todo t (topic name)
        </td>
    </tr>
    <tr>
        <td>
            u<br />
            unarchive
        </td>
        <td>
            Un-archives the markdown file for a given date. The file is moved form the subfolder back to the main todo
            folder. The name of the archive folder is specified in settings.json. Also in settings.json can be specified
            whether the file is moved simply in the file system, or by using git mv.<br /><br />
            Usage: todo u [date]
        </td>
    </tr>
<table>

Notes:

createorshow is the default command. This means it can be accessed simply by
typing anything that can be parsed as a date after the word todo.

Valid date formats:-

* "y", "yesterday" &#8594; yesterday<br/>
* [empty string], ".", "today" &#8594; today<br/>
* "tm", "tomorrow" &#8594; tomorrow<br/>
* (day) &#8594; the day/month/year which is nearest in time to today<br/>
* (day)/(month) &#8594; the day/month which is nearest in time to today<br/>
* +(daycount) &#8594; positive offset a number of days from today<br/>
* -(daycount) &#8594; negative offset a number of days from today<br/><br/>

[Commit Message] &#8594; In the Commit and Sync commands, the commit message is optional. If none is supplied, then a standard message detailing date and time of the commit will be used.

