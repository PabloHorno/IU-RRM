#pragma once

#include "dedo.h"
//PULGAR
#define PULGAR_ABIERTO 60
#define PULGAR_CERRADO 107
//INDICE
#define INDICE_ABIERTO 56
#define INDICE_CERRADO 101
//CORAZON
#define CORAZON_ABIERTO 57
#define CORAZON_CERRADO 96
//ANULAR
#define ANULAR_ABIERTO 58
#define ANULAR_CERRADO 98
//MENIQUE
#define MENIQUE_ABIERTO 57
#define MENIQUE_CERRADO 105

class Mano {
public:
	enum dedos {PULGAR, INDICE, CORAZON, ANULAR, MENIQUE};
	void set_pines(unsigned pin[5]);
	void set_pines(unsigned pin_pulgar, unsigned pin_indice,unsigned pin_corazon, unsigned pin_anular, unsigned pin_menique);
	void abrir_dedos();
	void cerrar_dedos();
	void contar(unsigned iteraciones = 1);
	void indice_pulgar();
	void asignar_posicion_dedos(unsigned posiciones[5]);
	void calibrar_dedos();
	bool is_ready();

	//private:
	Dedo dedos[5];
};