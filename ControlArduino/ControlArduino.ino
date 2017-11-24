/*
 Name:		ControlArduino.ino
 Created:	10/6/2017 11:58:17 AM
 Author:	Pablo Horno
*/

// the setup function runs once when you press reset or power the board
#include <Servo.h>
#include <ArduinoJson.h>
#include "Mano.h"


//Defines
#define LINEARACTUATORPIN 8         //Linear Actuator Digital Pin


//generic deadband limits - not all joystics will center at 512, so these limits remove 'drift' from joysticks that are off-center.
#define DEADBANDLOW 482   //decrease this value if drift occurs, increase it to increase sensitivity around the center position
#define DEADBANDHIGH 542  //increase this value if drift occurs, decrease it to increase sensitivity around the center position

//max/min puse values in microseconds to send to the servo
#define POS_MIN      1050  //fully retracted
#define POS_MAX      2000 //fully extended

Servo linearActuator;    // create servo object to control the linear actuator
int JSValue;             // variables to hold the last reading from the analog pins for the joystick. The value will be between 0 and 1023
int ValueMapped;        // the joystick values will be changed (or 'mapped') to new values to be sent to the linear actuator.

int LinearValue = 1500;   //current positional value being sent to the linear actuator. Start at the 'centered' position

int speed = 1;        //alter this value to change the speed of the system. Higher values mean higher speeds

Mano mano;
void setup() {
	Serial.begin(9600);
	//handShake();

	//initialize servos
	linearActuator.attach(LINEARACTUATORPIN, POS_MIN, POS_MAX);  // attaches/activates the linear actuator on pin LINEARACTUATORPIN 

																 //use the writeMicroseconds to set the linear actuator to a default centered position
	linearActuator.writeMicroseconds(LinearValue);
}

// the loop function runs over and over again until power down or reset
void loop() {
	if (Serial.available())
	{
		JSValue = Serial.parseInt();
		Serial.println(JSValue);
	}


	//only update if the joystick is outside the deadzone (i.e. moved oustide the center position)
	if (JSValue > DEADBANDHIGH || JSValue < DEADBANDLOW)
	{
		ValueMapped = map(JSValue, 0, 1023, speed, -speed); //Map analog value from native joystick value (0 to 1023) to incremental change (speed to -speed).
		LinearValue = LinearValue + ValueMapped; //add mapped joystick value to present Value
	}

	//use the writeMicroseconds to set the servos to their new positions
	linearActuator.writeMicroseconds(LinearValue);

	delay(10); // waits for the servo to get to they're position before continuing 
}

bool handShake()
{
	while (!Serial.availableForWrite()) {}
	Serial.println("READY");
	while (true)
	{
		String str;
		if (Serial.available() && Serial.readString() == "SYNC")
		{
			Serial.println("ACK");
			break;
		}
		delay(20);
	}
	while (true)
	{
		String str;
		if (Serial.available() && Serial.readString() == "INI")
		{
			Serial.println("OK");
			break;
		}
		delay(20);
	}

}
