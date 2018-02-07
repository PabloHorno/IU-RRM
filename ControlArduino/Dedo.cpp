#include "Dedo.h"
void Dedo::iniciar(unsigned pin, unsigned apertura_maxima, unsigned apertura_minima)
{
	attach(pin);
	posicion_abierto = apertura_minima;
	posicion_cerrado = apertura_maxima;
}

void Dedo::abrir(bool _delay)
{
	if (estado != abierto)
	{
		this->write(posicion_abierto);
		if (_delay)
			delay(DELEY_APERTURA_DEDO);
		estado = abierto;
	}
}

void Dedo::cerrar(bool _delay)
{
	if (estado != cerrado)
	{
		this->write(posicion_cerrado);
		if (_delay)
			delay(DELEY_APERTURA_DEDO);
		estado = cerrado;
	}
}

unsigned Dedo::get_posicion_via_angulo(float angulo)
{
	return map((-B - sqrtf(B*B - 4 * A*(C - angulo))) / (2 * A),0,50,POS_MIN,POS_MAX);
}
