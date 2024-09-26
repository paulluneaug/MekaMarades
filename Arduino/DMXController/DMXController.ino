
#include <DMXSerial.h>
#include <SoftwareSerial.h>
#include <SerialCommand.h>

#include "Ultrasonic.h"
Ultrasonic ultrasonic(2, 3); // Trig et Echo
 
////////////////
// QUEUE
////////////////
const int MAX_SIZE = 64;

typedef struct {
    byte items[MAX_SIZE];
    int front;
    int rear;
    int count;
} Queue;

// Function to initialize the queue
void InitializeQueue(Queue* q)
{
    q->front = -1;
    q->rear = 0;
    q->count = 0;
}

// Function to check if the queue is empty
bool IsEmpty(Queue* q) 
{ 
  int frontMod = (q->front + MAX_SIZE) % MAX_SIZE;
  int rearMod = (q->rear + MAX_SIZE) % MAX_SIZE;
  return frontMod + 1 == rearMod; 
}

// Function to check if the queue is full
bool IsFull(Queue* q) 
{
  int frontMod = (q->front + MAX_SIZE) % MAX_SIZE;
  int rearMod = (q->rear + MAX_SIZE) % MAX_SIZE;
  return frontMod == rearMod + 1; 
}

// Function to add an element to the queue (Enqueue
// operation)
void Enqueue(Queue* q, byte value)
{
    if (IsFull(q)) {
        return;
    }
    q->items[q->rear] = value;
    q->rear = (q->rear + 1) % MAX_SIZE;
    q->count++;
}

// Function to remove an element from the queue (Dequeue
// operation)
void Dequeue(Queue* q)
{
    if (IsEmpty(q)) {
        return;
    }
    q->front = (q->front + 1) % MAX_SIZE;
    q->count--;
}

// Function to get the element at the front of the queue
// (Peek operation)
byte Peek(Queue* q)
{
    if (IsEmpty(q)) {
        return 0; // return some default value or handle
                  // error differently
    }
    return q->items[(q->front + 1) % MAX_SIZE];
}
////////////////
// END QUEUE
////////////////


// Communication
SerialCommand sCmd;

const byte ULTRASONIC_DATA_HEADER = 10;
const byte DMX_COMMAND_HEADER = 20;
const byte ERROR_HEADER = 245;

const int BUFFER_SIZE = 32;

byte m_readBuffer[BUFFER_SIZE];
byte m_writeBuffer[BUFFER_SIZE];

Queue m_recieveQueue;

// DMX Control
const int DMXPin = 1; 



void setup() 
{
  Serial.begin(9600);

  InitializeQueue(&m_recieveQueue);

  InitDMX();
  InitUltrasonic();

}

void loop() 
{
  if (Serial.available()) 
  {
    int readBytes = Serial.readBytes(m_readBuffer, BUFFER_SIZE);
    //SendError(readBytes);

    DispatchRecievedMessage(readBytes);

    Serial.flush();
  }

  UpdateUltrasonic();

  delay(10);
}

// Communication

void DispatchRecievedMessage(int readBytes)
{
  for (int i = 0; i < readBytes; i++)
  {
      Enqueue(&m_recieveQueue, m_readBuffer[i]);
  }

  bool recievedEnoughDatas = true;
  while (m_recieveQueue.count > 0 && recievedEnoughDatas)
  {
      byte queueHead = Peek(&m_recieveQueue);
      switch (queueHead)
      {
          case DMX_COMMAND_HEADER:
              recievedEnoughDatas &= TryProcessDMXDatas(&m_recieveQueue);
              break;

          default: // The header is discarded if unknown
              Dequeue(&m_recieveQueue);
              SendError(queueHead);
              break;
      }
  }





//  byte headerByte = m_readBuffer[0];
//  switch (headerByte)
//  {
//    case DMX_COMMAND_HEADER:
//      if (readBytes != 3)
//      {
//        SendError(10);
//        break;
//      }
//      SendError(0); // All Good
//      SendDMXCommand(m_readBuffer[1], m_readBuffer[2]);
//      break;
//    
//    default:
//      SendError(255);
//      break;

//  }
}

bool TryProcessDMXDatas(Queue* recievedDatas)
{
    if (recievedDatas->count < 3)
    {
        return false; // Should wait for more datas to arrive
    }

    Dequeue(recievedDatas); // Dequeue the header

    byte channel = Peek(recievedDatas);
    Dequeue(recievedDatas); // Dequeue the channel

    byte value = Peek(recievedDatas);
    Dequeue(recievedDatas); // Dequeue the value

    SendError(0); // All Good
    SendDMXCommand(channel, value);

    return true; // The command was processed and removed from the queue
}

void SendError(byte errorCode)
{
  int wroteBytes;
  m_writeBuffer[wroteBytes++] = ERROR_HEADER;
  m_writeBuffer[wroteBytes++] = errorCode;
  Serial.write(m_writeBuffer, wroteBytes);
}

// Ultrasonic
void InitUltrasonic()
{
  pinMode(12, OUTPUT); // LED
}

void UpdateUltrasonic()
{
  int dist = ultrasonic.read();

  dist = Clamp(dist, 0, 255);

  byte wroteBytes = 0;

  m_writeBuffer[wroteBytes++] = ULTRASONIC_DATA_HEADER;
  m_writeBuffer[wroteBytes++] = (byte)dist;
  Serial.write(m_writeBuffer, wroteBytes);
}


// DMX
void InitDMX()
{
  DMXSerial.init(DMXController);

  pinMode(DMXPin, OUTPUT);
}

void SendDMXCommand(byte channel, byte value)
{
    DMXSerial.write(channel, value);
    analogWrite(DMXPin, value);
}

// utils

int Clamp(int value, int min, int max){
  if (value < min){ return min; }
  if (value > max){ return max; }
  return value;
}
