/*
 Name:		ControlArduino.ino
 Created:	10/6/2017 11:58:17 AM
 Author:	Pablo Horno
*/
#include <ArduinoJson.h>
//#include <VarSpeedServo.h>
#include<Servo.h>
//max/min puse values in microseconds to send to the servo
#define POS_MIN      1050  //fully retracted
#define POS_MAX      2000 //fully extended

int speed;
int pos;
int lastPulse = 0;    // the time in milliseconds of the last pulse
int refreshTime = 20; // the time needed in between pulses
//VarSpeedServo servo;
Servo servo;
void setup() {
	Serial.begin(9600);
	//handShake();
	servo.attach(8);
}

// the loop function runs over and over again until power down or reset
void loop() {
	if (Serial.available())
		servo.writeMicroseconds(map(Serial.parseInt(),0,50,1050,2000));
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

