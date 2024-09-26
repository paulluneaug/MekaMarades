#include <SoftwareSerial.h>
#include <SerialCommand.h>

SerialCommand sCmd;

const int BUFFER_SIZE = 32;

int buffer[BUFFER_SIZE];

void setup() {
  Serial.begin(9600);
  sCmd.addCommand("PING", pingHandler);
  Serial.println("Connected");
}
// }

void loop()
{
  if (Serial.available()) 
  {
    int readBytes = Serial.readBytes(buffer, BUFFER_SIZE)

  }
}

void pingHandler(const char *command) {
  Serial.println("PONG");
}