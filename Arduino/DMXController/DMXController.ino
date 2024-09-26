
#include <DMXSerial.h>
#include <SoftwareSerial.h>
#include <SerialCommand.h>


// Communication
SerialCommand sCmd;

const int BUFFER_SIZE = 32;

byte m_buffer[BUFFER_SIZE];

// DMX Control
const int DMXPin = 1; 

const int PAN_CHANNEL = 1;
const int TILT_CHANNEL = 3;
const int COLOR_CHANNEL = 5;
const int GOBO_CHANNEL = 6;
const int STROBE_CHANNEL = 6;
const int DIMMER_CHANNEL = 8;

const int WIND_CHANNEL = 13;



void setup() 
{
  Serial.begin(9600);
  InitDMX();
}

void loop() 
{
  if (Serial.available()) 
  {
    int readBytes = Serial.readBytes(m_buffer, BUFFER_SIZE);
    Serial.write(m_buffer, readBytes);
    SendDMXValue(COLOR_CHANNEL, random(255));

    SendDMXValue(WIND_CHANNEL, random(255));
  }

  delay(10);
}

// Communication

void DispatchRecievedMessage(int readBytes)
{
  byte typeByte = m_buffer[0];
  switch (typeByte)
  {
    case 0: // Requests a data
      break;

    case 1: // Sends a command
      break;
  }
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
