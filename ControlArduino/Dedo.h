#pragma once
#include <Arduino.h>
#include <Servo.h>
#define DELEY_APERTURA_DEDO 7000
#define DELEY_CERRAR_DEDO 7000

class Dedo :public Servo {
public:
	void iniciar(unsigned, unsigned, unsigned);
	void abrir(bool delay = true);
	void cerrar(bool delay = true);
	unsigned get_posicion_abierto() {
		return
			posicion_abierto;
	};
	unsigned get_posicion_cerrado() {
		return
			posicion_cerrado;
	};
	enum _estado { abierto, cerrado, desconocido };
	_estado get_estado() { return estado; };
private:
	unsigned posicion_abierto;
	unsigned posicion_cerrado;
	_estado estado;
};
