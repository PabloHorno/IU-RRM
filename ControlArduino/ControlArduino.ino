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
Servo lineal;
void setup() {
	Serial.begin(9600);
	while (!Serial.availableForWrite()) {}
	lineal.attach(8);
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

// the loop function runs over and over again until power down or reset
void loop() {
	if (Serial.available())
	{
		int pos = Serial.parseInt();
		Serial.println(pos);
		lineal.write(pos);
	}
}
