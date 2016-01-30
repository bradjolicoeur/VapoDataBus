## VapoDataBus
VapoDataBus was created to purge NServiceBus DataBus files after n days.  With some small modifications it can be used to purge other files as well. 

I originally used Sam Martindale's Powershell script for purging, but found that I needed something that handled a larger volume of files.

This project improves on Sam's PS script in the following ways:
- It can be run once an hour to delete an hour of files at a time
- Uses .NET and is slightly faster at deleting files than PS
- Is installed as a windows service with Quartz.Net scheduling

Sam, Thanks for publishing your PowerShell script.  It was a lifesaver when I found it and it inspired this project.

### Configuration
The configuration is all in the app settings.  For the most part you will only need to change the path to your DataBus share.

- Cron := This is a standard Cron Expression set to run every hour
- FolderPath := Set to the path to your DataBus share.
- FolderDatePatter := This is a Regex that parses the DataBus folder names into a datetime
- NumberOfDays =: The max number of days you want to keep files.

Important note on NumberOfDays: 
This is based on 24 hours so that it deletes files every hour.  If you set the number of days to 1 as an example, it will delete any folders over 24 hours old.

```sh
  <appSettings>
    <add key="Cron" value="0 0 0/1 * * ?" /> <!--Set to run every hour -->
    <add key="FolderPath" value="C:\data\temp\DataBus" /> 
    <add key="FolderDatePattern"
    value="^(?&lt;year>\d{4})\-(?&lt;month>\d{2})\-(?&lt;day>\d{1,2})_(?&lt;hour>\d{1,2})" />
    <add key="NumberOfDays" value="3" />
  </appSettings>
```

### Installation

This project uses TopShelf and can be installed as a windows service by executing the following commandline after you pull the project down and compile it:

```sh
 vapodatabus.exe install
```

If you want to customize the service name or etc, please lookup the Topshelf command line parameters