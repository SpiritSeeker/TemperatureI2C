#include <Wire.h>
#define SLAVE_ADDRESS 0x40

byte response[2];
volatile short reading;
void setup() {
  Serial.begin(9600);
  analogReference(INTERNAL);
  Wire.begin(SLAVE_ADDRESS);
  Wire.onRequest(sendData);
  pinMode(A0,INPUT);
}

void loop() {
  //Wire.write(reading);
  delay(1000);
}

void sendData(){
  reading = analogRead(A0);
  reading = map(reading,0,1023,0,255);
  response[0]=(byte)reading;
  Wire.write(response,2);
}  


