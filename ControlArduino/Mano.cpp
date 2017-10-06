#include "Arduino.h"
#include "Mano.h"

void Mano::set_pines(unsigned pin[5])
{
	dedos[PULGAR].iniciar(pin[0], PULGAR_CERRADO, PULGAR_ABIERTO);
	dedos[INDICE].iniciar(pin[1], INDICE_CERRADO, INDICE_ABIERTO);
	dedos[CORAZON].iniciar(pin[2], CORAZON_CERRADO, CORAZON_ABIERTO);
	dedos[ANULAR].iniciar(pin[3], ANULAR_CERRADO, ANULAR_ABIERTO);
	dedos[MENIQUE].iniciar(pin[4], MENIQUE_CERRADO,MENIQUE_ABIERTO);
}

void Mano::set_pines(unsigned pin_pulgar, unsigned pin_indice, unsigned pin_corazon, unsigned pin_anular,unsigned pin_menique)
{
	unsigned pin[] = { pin_pulgar, pin_indice, pin_corazon, pin_anular, pin_menique };
	set_pines(pin);
}

void Mano::abrir_dedos()
{
	for(unsigned i = 0; i < sizeof(dedos); i++)
		dedos[i].abrir(false);
	delay(8000);
}

void Mano::cerrar_dedos()
{
	for (unsigned i = 0; i < sizeof(dedos); i++)
		dedos[i].cerrar(false);
	delay(8000);
}

void Mano::contar(unsigned interaciones)
{
	for (unsigned iter = 0; iter < interaciones; iter++)
	{
		cerrar_dedos();
		for (unsigned i = 0; i < sizeof(dedos); i++)
			dedos[i].abrir();
	}
}

void Mano::indice_pulgar()
{
}

void Mano::asignar_posicion_dedos(unsigned posiciones[5])
{
	for (unsigned i = 0; i < sizeof(dedos); i++)
		dedos[i].write(posiciones[i]);
}
void Mano::calibrar_dedos()
{
	Serial.println("Calibrando dedos...");
	unsigned dedo = 0;
	bool siguiente_dedo = false;
	unsigned posicion = 80;
	unsigned input;
	while (dedo < 5)
	{
		while (!siguiente_dedo)
		{
			if (Serial.available()) {
				input = Serial.read();
				if (input == 43)
				{
					posicion++;
				}
				else if (input == 45)
				{
					posicion--;
				}
				else if (input == 42)
				{
					siguiente_dedo = true;
				}

				dedos[dedo].write(posicion);

				Serial.print("Dedo: ");
				Serial.print(dedo);
				Serial.print(" --- Posicion: ");
				Serial.println(posicion);
			}
		}
		siguiente_dedo = false;
		dedo++;
	}
}

bool Mano::is_ready()
{
	for (unsigned i = 0; i < sizeof(dedos); i++)
		if(dedos[i].attached())
			return false;
	return true;
}
