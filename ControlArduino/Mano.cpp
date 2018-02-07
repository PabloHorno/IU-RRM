#include "Arduino.h"
#include "Mano.h"

Mano::Mano()
{
	dedos[PULGAR].set_parametros(0.0001, -2.62, 33.2144);
	dedos[INDICE].set_parametros(-0.0018, -2.3714, 24.2552);
	dedos[CORAZON].set_parametros(0.0473, -3.9503, 43.7119);
	dedos[ANULAR].set_parametros(-0.0104, -2.2612, 26.4541);
	dedos[MENIQUE].set_parametros(-0.0031, -2.739, 32.617);
}

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
void Mano::set_posicion_dedos(float angulo, unsigned velocidad, bool wait)
{
  dedos[PULGAR].write(dedos[PULGAR].get_posicion_via_angulo(angulo),velocidad,false);
  dedos[INDICE].write(dedos[INDICE].get_posicion_via_angulo(angulo),velocidad,false);
  dedos[CORAZON].write(dedos[CORAZON].get_posicion_via_angulo(angulo),velocidad,false);
  dedos[ANULAR].write(dedos[ANULAR].get_posicion_via_angulo(angulo),velocidad,false);
  dedos[MENIQUE].write(dedos[MENIQUE].get_posicion_via_angulo(angulo),velocidad,wait);
}
void Mano::procesar()
{
  if(Serial.available())
  {
    JsonObject& parametros = jsonBuffer.parseObject(Serial.readString());
    Serial.println((int)parametros["tipo"]);
    switch((int)parametros["tipo"])
    {
      case 0: //AbrirCerrarMano Completa
      {
       // {"tipo":0.0,"R":3.0,"VACompleta":2.0,"AACompleta":30.0,"TACompleta":5.0,"VCCompleta":2.0,"ACCompleta":-90.0,"TCCompleta":5.0}
       set_posicion_dedos(parametros["AACompleta"],parametros["VACompleta"],true);
       delay(1000*(int)parametros["TACompleta"]);
       set_posicion_dedos(parametros["ACCompleta"],parametros["VCCompleta"],true);
       delay(1000*(int)parametros["TCCompleta"]);
        break;
      }
      case 1: //AbrirCerrarMano Dedos
      {
       //{"tipo":1.0,"R":3.0,"AAA":30.0,"VCP":2.0,"ACP":30.0,"TCP":5.0,"VAP":2.0,"AAP":30.0,"TAP":5.0,"VCI":2.0,"ACI":30.0,"TCI":5.0,"VAI":2.0,"AAI":30.0,"TAI":5.0,"VCC":2.0,"ACC":30.0,"TCC":5.0,"VAC":2.0,"AAC":30.0,"TAC":5.0,"VCA":2.0,"ACA":30.0,"TCA":5.0,"VAA":2.0,"TAA":5.0,"VCM":2.0,"ACM":30.0,"TCM":5.0,"VAM":2.0,"AAM":30.0,"TAM":5.0}
      
        dedos[PULGAR].write(dedos[PULGAR].get_posicion_via_angulo(parametros["AAP"]),parametros["VAP"],false);
        delay(1000*(int)parametros["TAP"]);
        dedos[INDICE].write(dedos[INDICE].get_posicion_via_angulo(parametros["AAI"]),parametros["VAI"],false);
        delay(1000*(int)parametros["TAI"]);
        dedos[CORAZON].write(dedos[CORAZON].get_posicion_via_angulo(parametros["AAC"]),parametros["VAC"],false);
        delay(1000*(int)parametros["TAC"]);
        dedos[ANULAR].write(dedos[ANULAR].get_posicion_via_angulo(parametros["AAA"]),parametros["VAA"],false);
        delay(1000*(int)parametros["TAA"]);
        dedos[MENIQUE].write(dedos[MENIQUE].get_posicion_via_angulo(parametros["AAM"]),parametros["VAM"],false);
        delay(1000*(int)parametros["TAM"]);
        dedos[PULGAR].write(dedos[PULGAR].get_posicion_via_angulo(parametros["ACP"]),parametros["VCP"],false);
        delay(1000*(int)parametros["TCP"]);
        dedos[INDICE].write(dedos[INDICE].get_posicion_via_angulo(parametros["ACI"]),parametros["VCI"],false);
        delay(1000*(int)parametros["TCI"]);
        dedos[CORAZON].write(dedos[CORAZON].get_posicion_via_angulo(parametros["ACC"]),parametros["VCC"],false);
        delay(1000*(int)parametros["TCC"]);
        dedos[ANULAR].write(dedos[ANULAR].get_posicion_via_angulo(parametros["ACA"]),parametros["VCA"],false);
        delay(1000*(int)parametros["TCA"]);
        dedos[MENIQUE].write(dedos[MENIQUE].get_posicion_via_angulo(parametros["ACM"]),parametros["VCM"],true);
        delay(1000*(int)parametros["TCM"]);
        break;
      }
      case 2: //PinzaFina
      {
        //{"tipo":2.0,"R":3.0,"5":0.0,"VCPinza":2.0,"TCPinza":5.0,"VAPinza":1.0,"TAPinza":5.0}
        //Falta detallar del modelo cinematico una ecuacion que relacione la distancia entre dedos con los angulos de giro.
        break;
      }
      case 3: //PinzaGruesa
      {
        //{"tipo":3.0,"R":3.0,"5":0.0,"VCPinza":2.0,"TCPinza":5.0,"VAPinza":1.0,"TAPinza":5.0}
        
        //Falta detallar del modelo cinematico una ecuacion que relacione la distancia entre dedos con los angulos de giro.
        
        break;
      }
    }
    Serial.println("NEXT");
  }
}

