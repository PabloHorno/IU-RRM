/*
 Name:		ControlArduino.ino
 Created:	10/6/2017 11:58:17 AM
 Author:	Pablo Horno
*/

// the setup function runs once when you press reset or power the board
#include <Servo.h>
#include <ArduinoJson.h>
#include "Mano.h"
Mano mano;
void setup() {
	Serial.begin(9600);
}

// the loop function runs over and over again until power down or reset
void loop() {
	if (Serial.available())
	{
		String str = Serial.readString();
		Serial.println(str);
	}
}
