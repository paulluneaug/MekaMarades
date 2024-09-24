#include "Ultrasonic.h"
Ultrasonic ultrasonic(2, 3); // Trig et Echo
 
void setup() {
  Serial.begin(9600);
  pinMode(12, OUTPUT);
}

void loop () {
  int dist = ultrasonic.read();
  if(dist > 10)
  {
    digitalWrite(12, HIGH);
  }
  else
  {
    digitalWrite(12, LOW);
  }

  delay(10);
}