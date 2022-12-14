#include <TimedAction.h>//https://playground.arduino.cc/Code/TimedAction/
#include <Wire.h>
#include <ADXL345.h>//https://github.com/Seeed-Studio/Accelerometer_ADXL345
#include <math.h>
// fatal error: WProgram.h: No such file or directory が出たらライブラリフォルダのTimedAction.hの#include "WProgram.h"を#include "Arduino.h"に

ADXL345 adxl; //variable adxl is an instance of the ADXL345 library
void setup()
{
  Serial.begin(9600);
  adxl.powerOn();

  //set activity/ inactivity thresholds (0-255)
  adxl.setActivityThreshold(75); //62.5mg per increment
  adxl.setInactivityThreshold(75); //62.5mg per increment
  adxl.setTimeInactivity(10); // how many seconds of no activity is inactive?

  //look of activity movement on this axes - 1 == on; 0 == off
  adxl.setActivityX(1);
  adxl.setActivityY(1);
  adxl.setActivityZ(1);

  //look of inactivity movement on this axes - 1 == on; 0 == off
  adxl.setInactivityX(1);
  adxl.setInactivityY(1);
  adxl.setInactivityZ(1);

  //look of tap movement on this axes - 1 == on; 0 == off
  adxl.setTapDetectionOnX(0);
  adxl.setTapDetectionOnY(0);
  adxl.setTapDetectionOnZ(1);

  //set values for what is a tap, and what is a double tap (0-255)
  adxl.setTapThreshold(50); //62.5mg per increment
  adxl.setTapDuration(15); //625us per increment
  adxl.setDoubleTapLatency(80); //1.25ms per increment
  adxl.setDoubleTapWindow(200); //1.25ms per increment

  //set values for what is considered freefall (0-255)
  adxl.setFreeFallThreshold(7); //(5 - 9) recommended - 62.5mg per increment
  adxl.setFreeFallDuration(45); //(20 - 70) recommended - 5ms per increment

  //setting all interrupts to take place on int pin 1
  //I had issues with int pin 2, was unable to reset it
  adxl.setInterruptMapping(ADXL345_INT_SINGLE_TAP_BIT,   ADXL345_INT1_PIN);
  adxl.setInterruptMapping(ADXL345_INT_DOUBLE_TAP_BIT,   ADXL345_INT1_PIN);
  adxl.setInterruptMapping(ADXL345_INT_FREE_FALL_BIT,    ADXL345_INT1_PIN);
  adxl.setInterruptMapping(ADXL345_INT_ACTIVITY_BIT,     ADXL345_INT1_PIN);
  adxl.setInterruptMapping(ADXL345_INT_INACTIVITY_BIT,   ADXL345_INT1_PIN);

  //register interrupt actions - 1 == on; 0 == off
  adxl.setInterrupt(ADXL345_INT_SINGLE_TAP_BIT, 1);
  adxl.setInterrupt(ADXL345_INT_DOUBLE_TAP_BIT, 1);
  adxl.setInterrupt(ADXL345_INT_FREE_FALL_BIT,  1);
  adxl.setInterrupt(ADXL345_INT_ACTIVITY_BIT,   1);
  adxl.setInterrupt(ADXL345_INT_INACTIVITY_BIT, 1);
}

double OffsetX, OffsetY, OffsetZ = 0;
bool First = true;

void get()
{
  int x, y, z;
  double X, Y, Z, A;
  adxl.readXYZ(&x, &y, &z);
  if (First)
  {
    OffsetX = x * 3.937;
    OffsetY = y * 3.937;
    OffsetZ = z * 3.937;
    First = false;
  }
  X = x * 3.937 - OffsetX;
  Y = y * 3.937 - OffsetY;
  Z = z * 3.937 - OffsetZ;
  OffsetX = x * 3.937;//無理やり加速度の変化を求めているので正しくない？(ずれたとき用)
  OffsetY = y * 3.937;
  OffsetZ = z * 3.937;

  A = sqrt(X * X + Y * Y + Z * Z);//合成加速度

  Serial.print(X);
  Serial.print(",");
  Serial.print(Y);
  Serial.print(",");
  Serial.print(Z);
  Serial.print(",");
  Serial.println(A);
}
TimedAction getwrite = TimedAction(100,get);

void loop()
{
 getwrite.check();
}
