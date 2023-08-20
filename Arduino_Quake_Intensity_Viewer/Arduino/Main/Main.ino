#include <TimedAction.h>//https://playground.arduino.cc/Code/TimedAction/
#include <Wire.h>
//#include <SD.h>
#include <ADXL345.h>//https://github.com/Seeed-Studio/Accelerometer_ADXL345
#include <math.h>
#include <TimeLib.h> //https://playground.arduino.cc/Code/Time/
// fatal error: WProgram.h: No such file or directory が出たらライブラリフォルダのTimedAction.hの#include "WProgram.h"を#include "Arduino.h"に

ADXL345 adxl; //variable adxl is an instance of the ADXL345 library
//File file;
void setup()
{
  Serial.begin(115200);
  //Serial.println("setup start...");
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
  //Serial.println("setup finish.");
  OffsetSetting();
}

double OffsetX, OffsetY, OffsetZ;
void(*resetFunc)(void) = 0;

void OffsetSetting()
{
  //Serial.println("Offset settig start...");
  double X = 0; //分けないと値がおかしくなる?
  double Y = 0;
  double Z = 0;
  for (int i = 0; i < 30; i++)//平均を求める
  {
    int x, y, z;
    adxl.readXYZ(&x, &y, &z);
    X += x;
    Y += y;
    Z += z;
    /*//異常確認用
      Serial.print(x);
      Serial.print(",");
      Serial.print(y);
      Serial.print(",");
      Serial.println(z);
      Serial.print(X);
      Serial.print(",");
      Serial.print(Y);
      Serial.print(",");
      Serial.println(Z);
    */
    delay(10);
  }
  OffsetX = X / 30 * 3.937;
  OffsetY = Y / 30 * 3.937;
  OffsetZ = Z / 30 * 3.937;
  /*
  Serial.print("Offset Setted:");
  Serial.print(OffsetX);
  Serial.print(",");
  Serial.print(OffsetY);
  Serial.print(",");
  Serial.println(OffsetZ);
  Serial.println("---------data start----------");
  */
}

String Text = "";
double Max = 0;
double latestA = 0;
double maxDifA = 0;
byte latestSec;
int c = 0;
double xMax = 0;
double xMin = 0;
double yMax = 0;
double yMin = 0;
double zMax = 0;
double zMin = 0;

void save()
{

}

void get()
{
  if (latestSec != second())
  {
    Serial.println("----------second change. save.");
    latestSec = second();
    get();
    Serial.print(Text);
    Serial.print("*");
    Serial.print(Max);
    Serial.print("*");
    Serial.print(maxDifA * 0.02);//等加速度直線運動v=atと仮定
    Serial.print("*");
    Serial.print(hour());
    Serial.print(":");
    Serial.print(minute());
    Serial.print(":");
    Serial.println(second());
/*
    //保存
    String path1 = "/";
    path1 += String(year());
    path1 += "-";
    path1 += String(month());
    String path2 = path1 + "/" + String(day());
    String path3 = path2 + "/" + String(hour());
    String path4 = path3 + "/" + String(minute());
    /*
    Serial.print("----------path:");
    Serial.println(path1);*//*
    if(!SD.exists(path1))
    {
      SD.mkdir(path1);
    }
    if(!SD.exists(path2))
    {
      SD.mkdir(path2);
    }
    if(!SD.exists(path3))
    {
      SD.mkdir(path3);
    }
    if(!SD.exists(path4))
    {
      SD.mkdir(path4);
    }
    file = SD.open(path4 + "/" + String(second()) + ".txt");
    file.print(Text);
    file.close();
    Serial.println("----------saved");*/
    
    Text = "";
    Max = 0;
    maxDifA = 0;
    c = 0;
    if (xMax <= 0 || xMin >= 0 || yMax <= 0 || yMin >= 0 || zMax <= 0 || zMin >= 0)
    {
      OffsetSetting();
    }
    xMax = 0;
    xMin = 0;
    yMax = 0;
    yMin = 0;
    zMax = 0;
    zMin = 0;
  }
  if (Serial.available() > 0)//データ受信時
  {
    Serial.println("----------data recieved");
    char input[20];
    byte times[7];//yearは-2000する
    times[6] = 0;
    Serial.readBytesUntil('\n', input, 32);
    char* cs = strtok(input, ",");
    for (byte i = 0; i < 7; i++)
    {
      times[i] = atoi(cs);
      cs = strtok(NULL, ",");
    }
    if (times[6] == 1)
    {
      setTime(times[3], times[4], times[5] - 1, times[2], times[1], times[0] + 2000);//1秒前のがくるから-1s
      Serial.print("------------");
      Serial.print(times[3]);
      Serial.print(":");
      Serial.print(times[4]);
      Serial.print(":");
      Serial.println(times[5]);
    }
    else
    {
      Serial.println("----------reseting...");
      resetFunc();//再起動
    }
    latestSec = second();
  }



  
  if (c >= 50)
  {
    return;
  }
  int x, y, z;
  double X, Y, Z, A;
  adxl.readXYZ(&x, &y, &z);
  X = x * 3.937 - OffsetX;
  Y = y * 3.937 - OffsetY;
  Z = z * 3.937 - OffsetZ;
  A = sqrt(X * X + Y * Y + Z * Z);//合成加速度
  Text += String(X);
  Text += ",";
  Text += String(Y);
  Text += ",";
  Text += String(Z);
  Text += ",";
  Text += String(A);
  Text += "/";
  Max = max(Max, A);
  maxDifA = max(maxDifA, abs(latestA - A));
  latestA = A;
  xMax = max(xMax, X);
  xMin = min(xMin, X);
  yMax = max(yMax, Y);
  yMin = min(yMin, Y);
  zMax = max(zMax, Z);
  zMin = min(zMin, Z);

  c++;
}
//1000/x Hz
TimedAction getAct = TimedAction(17, get); //なんか50いかないから調整 20未満だとRAM超えるかも(1個22.5byteくらい)
//TimedAction sendAct = TimedAction(17, save);
void loop()
{
  //sendAct.check();
  getAct.check();
}
