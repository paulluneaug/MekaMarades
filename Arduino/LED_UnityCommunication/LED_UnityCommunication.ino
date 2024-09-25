void setup()
{
  pinMode(2, OUTPUT);
  digitalWrite(2, HIGH);
}
void loop()
{
  delay(1000);
  lightUp();
}

void lightUp()
{
  digitalWrite(2, LOW);
}