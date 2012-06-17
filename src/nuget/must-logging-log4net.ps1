param($installPath, $toolsPath, $package, $project)

# $project = Get-Project

$log4net = $project.ProjectItems.Item("nohros.logging.log4net.dll")

# set Build Action to None
$log4net.Properties.Item("BuildAction").Value = 0

# set Copy to Output Directy to Copy if newer
$log4net.Properties.Item("CopyToOutputDirectory").Value = 2