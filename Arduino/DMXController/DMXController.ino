
#include <DMXSerial.h>
#include <SoftwareSerial.h>
#include <SerialCommand.h>

#include "Ultrasonic.h"
Ultrasonic ultrasonic(2, 3); // Trig et Echo
 


// Communication
SerialCommand sCmd;

const int BUFFER_SIZE = 32;

byte m_readBuffer[BUFFER_SIZE];
byte m_sendBuffer[BUFFER_SIZE];

// DMX Control
const int DMXPin = 1; 

const int PAN_CHANNEL = 1;
const int TILT_CHANNEL = 3;
const int COLOR_CHANNEL = 5;
const int GOBO_CHANNEL = 6;
const int STROBE_CHANNEL = 7;
const int DIMMER_CHANNEL = 8;

const int WIND_CHANNEL = 13;



void setup() 
{
  Serial.begin(9600);
  InitDMX();
  InitUltrasonic();

}

void loop() 
{
  if (Serial.available()) 
  {
    int readBytes = Serial.readBytes(m_readBuffer, BUFFER_SIZE);
    Serial.write(m_readBuffer, readBytes);
    SendDMXValue(COLOR_CHANNEL, random(255));

    SendDMXValue(WIND_CHANNEL, random(255));
  }

  UpdateUltrasonic();

  delay(20);
}

// Communication

void DispatchRecievedMessage(int readBytes)
{
  byte typeByte = m_readBuffer[0];
  switch (typeByte)
  {
    case 0: // Requests a data
      break;

    case 1: // Sends a command
      break;
  }
}

// Ultrasonic
void InitUltrasonic()
{
  pinMode(12, OUTPUT); // LED
}

void UpdateUltrasonic()
{
  byte dist = ultrasonic.read();
  m_sendBuffer[0] = dist;
  Serial.write(m_sendBuffer, 1);
}


// DMX
void InitDMX()
{
  DMXSerial.init(DMXController);

  pinMode(DMXPin, OUTPUT);
  
  SendDMXValue(DIMMER_CHANNEL, 10);
  SendDMXValue(PAN_CHANNEL, 128);
  SendDMXValue(TILT_CHANNEL, 128);
}

void SendDMXValue(int channel, int value)
{
    DMXSerial.write(channel, value);
    analogWrite(DMXPin, value);
}
