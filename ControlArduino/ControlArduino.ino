/*
 Name:		ControlArduino.ino
 Created:	10/6/2017 11:58:17 AM
 Author:	Pablo Horno
*/
#include "Mano.h"
//max/min puse values in microseconds to send to the servo
#define POS_MIN      1050  //fully retracted
#define POS_MAX      2000 //fully extended

Mano RRH;

void setup() {
	RRH.set_pines(4, 5, 6, 7, 8);
	Serial.begin(9600);
}

// the loop function runs over and over again until power down or reset
void loop() {
	if (Serial.available())
	{
		RRH.procesar();
	}
}
