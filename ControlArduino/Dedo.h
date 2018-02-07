#pragma once
#include <Arduino.h>
#include <VarSpeedServo.h>
#define DELEY_APERTURA_DEDO 7000
#define DELEY_CERRAR_DEDO 7000

#define POS_MIN      1050  //Vastago completamente retraido
#define POS_MAX      2000 //Vastago completamente Extendido

class Dedo :public VarSpeedServo {
public:
	void iniciar(unsigned, unsigned, unsigned);
	void abrir(bool delay = true);
	void cerrar(bool delay = true);
	void set_parametros(float A, float B, float C)
	{
		this->A = A; this->B = B, this->C = C;
	}
	unsigned get_posicion_abierto() {
		return
			posicion_abierto;
	};
	unsigned get_posicion_cerrado() {
		return
			posicion_cerrado;
	};
	unsigned get_posicion_via_angulo(float);
	enum _estado { abierto, cerrado, desconocido };
	_estado get_estado() { return estado; };
private:
	unsigned posicion_abierto;
	unsigned posicion_cerrado;
	_estado estado;
	float A = 0.0, B = 0.0, C = 0.0;
};
