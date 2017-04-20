# WinMQTTHub
###Control your Windows box over MQTT!

Enterprisecoding WinMQTTHub is an application/service/library which allows you to remotely control your Windows system over MQTT.

WinMQTT consist of tree main components;
<table>
<tr><th>Component</th><th>Description</th></tr>
<tr><td>WinMQTTHub.Core</td><td>Main library for WinMQTTHub.  Entry class **Hub** located within this assembly.</td></tr>
<tr><td>WinMQTTHub.App</td><td>WinMQTTHub desktop application. Acts as a simple desktop application wrapper for **Hub** class.</td></tr>
<tr><td>WinMQTTHub.Service</td><td>WinMQTTHub service application. Acts as a simple windows service wrapper for **Hub** class.</td></tr>
</table>


##Configuration
Hub application uses following parameters;
<table>
<tr><th>Property</th><th>Description</th><th>Default</th></tr>
<tr><td> BrokerUrl</td><td> URL of the MQTT broker to use. </td><td> tcp://localhost:1883 </td></tr>
<tr><td> BrokerUsername </td><td> Username used when connecting to MQTT broker. </td><td> </td></tr>
<tr><td> BrokerPassword </td><td> Password used when connecting to MQTT broker. </td><td> </td></tr>
<tr><td> ClientId</td><td> Client ID to present to the broker. </td><td> WinMQTTHub </td></tr>
<tr><td> TopicPrefix</td><td> Client ID to present to the broker. </td><td> winmqtthub </td></tr>
<tr><td> ReconnectInterval </td><td> Time interval between connection attempts in seconds. </td><td> 5 </td></tr>
<tr><td> ModuleAssemblyPattern </td><td> Assembly search pattern for addon things. </td><td> *.*Things.dll </td></tr>
</table>

####Note
Application icons made [byPixel Buddha](http://www.flaticon.com/authors/pixel-buddha) from [www.flaticon.com](http://www.flaticon.com) is licensed by [CC 3.0 BY](http://creativecommons.org/licenses/by/3.0/)