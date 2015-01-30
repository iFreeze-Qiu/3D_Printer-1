3D_Printer
==========

Version 1.0 of Force Feedback System for 3D Printer

A thorough user documentation file can be found here: https://www.dropbox.com/s/3lfdkf1q8zei6io/UserDoc.html?dl=0. This file goes in depth on how to use the program with the hardware itself.

Overview of Project
==========

This project is intended for use with the College of Staten Island's 3D Printer in the 6S building. It is designed to work 
solely with the hardware in Dr. Alan Lyon's research lab. The purpose of this program is to facilitate the automation of the
3D printing process.

For information pertaining to the 3D printing research project as a whole, refer to Dr. Lyon's research page:
http://csivc.csi.cuny.edu/Alan.Lyons/files/page5.html

The project was written in C# with .NET 4.5 Framework under Visual Studio 2013. The program's sole goals are to automate the
printing process and provide the user with an easy-to-use user interface for controlling the print process. 

The hardware involved are:

Microplotter (http://vergentaindia.com/pdf/catalog1.pdf)

Multi-Axis Piezo Alignment Stage (http://search.newport.com/?q=*&x2=sku&q2=8095)

Piezo Z Stage (http://www.physikinstrumente.com/product-detail-page/p-611z-201731.html)

Tension and Compress Load Cell (http://www.futek.com/product.aspx?stock=FSH00259)

USB voltmeter (http://www.digital-measure.com/html/voltmeter.htm)

The printing process requires the Z-Stage placed on the multi-axis stage underneath the microplotter's "arm", which will move 
down to dispense the ink. The load cell (or the force gauge) is attached to this arm. Attached to the force gauge is a special 
cylinder where the printing tips are held. These tips are dipped in the ink, then brought over to a position above the Z-Stage. 
The microplotter detects when it is in position and ready to begin the print - it triggers its relay to turn on, sending a 
voltage which is detected by the voltmeter. This voltmeter tells this program to begin the printing process - when the printing 
process begins, the Z-Stage begins moving up (in microns). The user tells this program which value to watch for from the force
gauge (typically 2 milligrams). When the Z-Stage begins to make contact with the tips, the force is detected and monitored. When
it reaches 2 milligrams, the program tells the Z-Stage to stop moving and allow the ink to dwell for a preset amount of time.
When the dwell time is finished, the program returns the Z-Stage to its default position and the microplotter returns to 
retrieve more ink as per its printing pattern (defined by the user). The multi-axis stage is there to provide different angles 
should the user require them.

Thus, this prorgam is responsible for controlling the multi-axis stage, the Z-Stage, the force gauge, and the USB voltmeter,
and automating the printing process. Once the program is running and all hardware connected, all the user needs to do is 
proceed with calibration (to determine speed and position settings for the Z-Stage) and then turn the voltmeter on through
the user interface. Once calibration is finished and the voltmeter on, the program will scan for change in voltage. The 
user then sets the printing pattern in the microplotter, begins the pattern, and the user's involvement is done - the
microplotter will follow through with its pattern and trigger the relay when needed. This relay trigger will notify the
voltmeter, which in turns let the program know to begin printing automatically. 

An additional feature added at the end of development is the Z-Stage "tracking mode" - this mode, when engaged, will 
continuously monitor for voltage change. When detected, it will move the Z-Stage up until the proper force value is detected,
as per the typical printing process. However, it records the position it found the proper force value and saves it. This is
repeated for as many times the voltage detection occurs and the tracking mode is engaged. When the user turns off tracking
mode, they are allowed to save the recorded position data to a spreadsheet. This spreadsheet contains all the positions and
the force values found during the duration of the tracking. This data can be used by the operator to map out the surface of
a given substrate placed on the Z-Stage - the substrate on which the ink is dispensed. 

Project Structure
==========

3D_Printer/ForceSensor
Contains Controller.cs which contains all logic specific to operation of the force gauge - connection/disconnection, reading
in force values, and changing settings (tare/gross).

3D_Printer/GUI
Contains the various forms making up the presentation layer. 

3D_Printer/PrintingLogic
Contains logic.cs and the various event classes dealing specifically with the printing and tracking logic.

3D_Printer/Voltmeter
Contains Controller.cs which contains all logic specific to operation of the voltmeter - connection/disconnection and reading
in voltage values.

3D_Printer/XYZStage
Contains Controller.cs and Ldcnlib.cs. Controller.cs contains all logic specific to the multi-axis stage and its drivers. 
Connecting to and disconnecting from the stage and each specific driver and all functions to move and group the drivers. 
Ldcnlib.cs is the COM Interop file to reveal the functions in the Ldcnlib.dll file, a COM assembly provided by the stage's 
manufacturer. 

3D_Printer/ZStage
Contains Controller.cs and E816.cs. Controller.cs contains all logic specific to the Z-Stage: connecting to and disconnecting
from the stage and moving the stage. E816.cs is the COM Interop file to reveal the functions in the E816_DLL.dll, a COM assembly
provided by the stage's manufacturer.

Futek USB DLL.dll is the .NET assembly associated with the force gauge, and USBMeasure.dll is the .NET assembly associated
with the voltmeter. These, along with E816_DLL.dll and Ldcnlib.dll, are required to run the program. Each assembly is provided
by their respective manufacturers.
