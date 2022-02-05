# Rocket-Payload-Simulator
A fun project I worked on to simulate a minified version of NASA's ground station rocket launching simulator. I made this app with WPF as the frond-end tool & WCF at the back. In order to simulate data transfer from rocket to ground station, the WCF service sends telemetry and gathered data from rocket and payload upon request. Also used multithreading and locking concepts to ensure shared config data files are safely read and written to.

## Features of the app

The figure below shows the UI of the app WPF. The following are the sections of the UI:

1. The Rocket Data Section: It shows which rockets data the user is currently viewing. The data received from WCF service like Altitude, latitude and longitude is displayed. Every rocket has a payload assigned to it. The payload can only be deployed when the rocket's orbit time reaches 0 (rocket reaches the orbit). Hence, Payload status is set if user clicks deploy payload and if the rocket has reached its orbit.

2. Payload Data Section: This section is similar to Rocket Data Section with the additional conditions that every payload has a data type assigned to it. In the figure below, the payload sends image data collected from WCF service. Also, payloads only appear in the dropdown if the deploy payload button is clicked.

3. Rocket and Payload Command Section: This section is used to send request to WCF Service to start sending data. Once user clicks on Start Telemetry, the app starts receiving data from the endpoint. A separate thread is assigned for each rocket launched. The WCF service runs percall instance context mode and multiple concurrency mode.

4. Launch Rocket Section: This section displays the rockets yet to be launched. The data for each rocket is read from a config file. Once user clicks Launch Button, that particular rocket shifts to the top most Rocket Data section. Also, if the user clicks on Deorbit Button, the rocket currently active ends its mission and joins the list of rockets yet to launch dropdown. 

5. Logs: This section keeps track of any command given by user or any action performed by the rocket/payload during its orbit period. It also keeps track of any exceptions or errors encountered during the run

![Payload](https://user-images.githubusercontent.com/40236708/106436025-74718c00-6428-11eb-97ad-8aef7a244a88.JPG)
