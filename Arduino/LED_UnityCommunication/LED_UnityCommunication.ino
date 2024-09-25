void setup()
{
  pinMode(2, OUTPUT);
  digitalWrite(2, HIGH);
}
void loop()
{
}

void lightUp()
{
  digitalWrite(2, LOW);
}