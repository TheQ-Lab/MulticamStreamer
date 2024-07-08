#include <FastLED.h>
//marker
#define NUM_LEDS 38
#define NUM_LEDS_OUTER 26
// ALL: 38 | Outer RING: 26 max

#define EXIT_HIGHLIGHT_POSA 10
#define EXIT_HIGHLIGHT_POSB 19


#define DATA_PIN A4
#define BUTTON_PIN 2

#define BRIGHTNESS 130
#define FRAMES_PER_SECOND 120

CRGB stripA[NUM_LEDS];
//CRGB stripA2[12];
CRGB stripB[NUM_LEDS];
CRGB stripC[NUM_LEDS];
CRGB stripD[NUM_LEDS];
CRGB stripE[NUM_LEDS];

int phase = 2;
unsigned long phaseTimer=0;
int serialInByte;
int exitHighlightPos = EXIT_HIGHLIGHT_POSA;

unsigned long lastUpdate = 0;
unsigned long cycleLength = 2500;
unsigned long cycleProgress; //0-1000, permillage in this cycle

int remainingPhase = 0;
int phaseLength = 3500;
int randomHue = 0;
int blink = 255;
int blinkDelta = -10;

//int hue = 0;

void setup() {
  // put your setup code here, to run once:
  //FastLED.addLeds<WS2812B, DATA_PIN, GRB>(stripA1,-12, NUM_LEDS+12);  // GRB ordering is typical
  //FastLED.addLeds<WS2812B, DATA_PIN, GRB>(stripA2, -NUM_LEDS, NUM_LEDS+12);
  FastLED.addLeds<WS2812B, A5, GRB>(stripA, 0, 38);  // GRB ordering is typical
  FastLED.addLeds<WS2812B, A4, GRB>(stripB, NUM_LEDS); 
  FastLED.addLeds<WS2812B, A3, GRB>(stripC, NUM_LEDS); 
  FastLED.addLeds<WS2812B, A2, GRB>(stripD, NUM_LEDS); 
  FastLED.addLeds<WS2812B, 7, GRB>(stripE, NUM_LEDS); 
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  FastLED.setMaxPowerInVoltsAndMilliamps(5,2500); 
  FastLED.setBrightness(BRIGHTNESS);
  
  for (int i=0; i<NUM_LEDS; i++) {
      stripA[i].setHSV(0, 255, 0);
  }
  FastLED.show(); 


  Serial.begin(115200);
  //EVERY_N_SECONDS()
}

void loop() {
  // put your main code here, to run repeatedly:
  Serial.println("state " + String(phase));
  //Serial.println(FastLED.getFPS());
  if(false){
    for (int i=0; i<NUM_LEDS; i++) {
      stripA[i].setColorCode(CRGB::YellowGreen);
    }
  }
  else if(phase == 1) {

    flash(stripE, 0, NUM_LEDS);
    if(millis() > phaseTimer)
      phase++;
  }
  else if(phase == 2) {
    cycleProgress = mapF((lastUpdate % cycleLength), 0, cycleLength, 0, 1000);
    setCycleColors(stripA, cycleProgress);

    exitHighlight(stripE, 0, NUM_LEDS_OUTER, 0, EXIT_HIGHLIGHT_POSA);
  }
  else if (phase == 3) {
    //sinelon(stripA1, NUM_LEDS);
    lightplanke(stripA, 0, NUM_LEDS_OUTER, false);
    lightplanke(stripB, 0, NUM_LEDS_OUTER, false);
    lightplanke(stripC, 0, NUM_LEDS_OUTER, false);
    lightplanke(stripD, 0, NUM_LEDS_OUTER, false);

    flash(stripE, 0, NUM_LEDS);
    if(millis() > phaseTimer)
      phase++;
  }
  else if (phase == 4) {
    lightplanke(stripA, 0, NUM_LEDS_OUTER, true);
    lightplanke(stripB, 0, NUM_LEDS_OUTER, true);
    lightplanke(stripC, 0, NUM_LEDS_OUTER, true);
    lightplanke(stripD, 0, NUM_LEDS_OUTER, true);

    exitHighlight(stripE, 0, NUM_LEDS_OUTER, 0, EXIT_HIGHLIGHT_POSB);
  }
  else if (phase == 5) {
    flash(stripA, 0, NUM_LEDS);
    lightschranke(stripB, 0, NUM_LEDS_OUTER, false);
    breathe(stripC, 0, NUM_LEDS);
    exitHighlight(stripD, 0, NUM_LEDS_OUTER, 0, EXIT_HIGHLIGHT_POSA);
  }

  if(digitalRead(BUTTON_PIN) == 0 && remainingPhase<=0 ){
    phase = 2;
    remainingPhase = phaseLength;
    randomHue = random(360);
  }

  if(Serial.available() > 0){
    serialInByte = Serial.read();
    if(char(serialInByte) == 'r'){
      phase++;
      if (phase%2 == 0) // if phase is even you're changing during transition-animation, that would just advance phase to the target phase of the previous command
        phase++;        // so we increment directly to the next transition
      if(phase > 4)
        phase = 1;
      //exitHighlightPos = EXIT_HIGHLIGHT_POSB;
      phaseTimer = millis() +2000;
    }
  }
  

  //cycleProgress = mapF((lastUpdate % cycleLength), 0, cycleLength, 0, 1000);
  //setCycleColors(stripE, cycleProgress);

  FastLED.show();  
  lastUpdate = millis();
  //FastLED.delay(5);
  FastLED.delay(1000/FRAMES_PER_SECOND); // better than vanilla delay

  //EVERY_N_SECONDS(8) { incrementState(5,5); }
}

void incrementState(short firstState, short lastState){
  phase++;
  if(phase > lastState){
    phase = firstState;
    for (int i=0; i<38; i++) {
      stripA[i].setHSV(0, 255, 0);
    }
  }
}

void setCycleColors(CRGB* strip, long cycleProgress) {
  float pos = mapF(cycleProgress, 0, 1000, 0, NUM_LEDS);
  for(int i=0; i<NUM_LEDS; i++){
    float dist = (i+NUM_LEDS)-pos; //dist to Start point
    if(dist >= NUM_LEDS)
      dist -= NUM_LEDS;
    //dist = int(dist) % NUM_LEDS;
    float hue = mapF(dist, 0, NUM_LEDS, 0, 256);
    strip[i].setHSV(hue, 255, 255);
  }
}

void flash(CRGB* strip, int startLed, int stopLed) {
  blink += blinkDelta;
  if(blink <= 40 || blink >= 255){
    blinkDelta *= -1; 
    blink = constrain(blink, 40, 255);
  }
  for(int i=startLed; i<stopLed; i++){
    strip[i].setHSV(0, 255, blink);
  }
}

void juggle() {
  // eight colored dots, weaving in and out of sync with each other
  fadeToBlackBy( stripA, NUM_LEDS, 20);
  uint8_t dothue = 0;
  for( int i = 0; i < 8; i++) {
    stripA[beatsin16( i+7, 0, NUM_LEDS-1 )] |= CHSV(dothue, 200, 255);
    dothue += 32;
  }
}

void sinelon(CRGB* strip, int size)
{
  // a colored dot sweeping back and forth, with fading trails
  fadeToBlackBy( strip, size, 40);  // this one applies to entire strip
  int pos = beatsin16( 20, 0, size-1 );
  strip[pos] += CHSV( 0, 255, 192);
}

void exitHighlight(CRGB* strip, int start, int length, byte hue, int position)
{
  byte localBrightness = 160;
  unsigned int revolutionTime = 650;

  fadeToBlackBy( strip, NUM_LEDS-start, 60);  // this one applies to entire strip

  int cycleProgress = millis() % revolutionTime;
  cycleProgress = map(cycleProgress, 0, revolutionTime, 0, 1000);

  int tailStart = round(position - (0.3 * length));
  //Serial.println(tailStart);
  int pilotLed = map(cycleProgress, 0, 1000, tailStart, position+1);
  pilotLed = (pilotLed+ length) %length; // rectified Location
  //Serial.println(pilotLed);
  strip[pilotLed] = CHSV( hue, 255, localBrightness);

  tailStart = round(position + (0.3 * length));
  pilotLed = map(cycleProgress, 0, 1000, tailStart, position-1);
  pilotLed = (pilotLed + 0) %length; // rectified Location
  //Serial.println(pilotLed);
  strip[pilotLed] = CHSV( hue, 255, localBrightness);
  
  CHSV highlight = CHSV( hue, 60, 0.40*localBrightness);
  strip[position] = highlight;
  strip[position+1] = highlight;
  strip[position-1] = highlight;

}

void lightplanke(CRGB* strip, int start, int length, bool counterclockwise)
{
  byte localBrightness = 190;
  unsigned int revolutionTime = 2500;

  fadeToBlackBy( strip, NUM_LEDS-start, 14);  // this one applies to entire strip
  int cycleProgress = millis() % revolutionTime;
  if(!counterclockwise)
    cycleProgress = map(cycleProgress, 0, revolutionTime, 0, 1000);
  else
    cycleProgress = map(cycleProgress, revolutionTime, 0, 0, 1000);
  // Serial.println(cycleProgress);

  int pilotLed = map(cycleProgress, 0, 1000, start, length);
  //Serial.println(pilotLed);

  strip[pilotLed] += CHSV( 0, 255, localBrightness);
  cycleProgress = (cycleProgress + 333) % 1000;
  pilotLed = map(cycleProgress, 0, 1000, start, length);
  strip[pilotLed] += CHSV( 0, 255, localBrightness);
  cycleProgress = (cycleProgress + 333) % 1000;
  pilotLed = map(cycleProgress, 0, 1000, start, length);
  strip[pilotLed] += CHSV( 0, 255, localBrightness);
}

void lightschranke(CRGB* strip, int start, int length, bool counterclockwise)
{
  byte localBrightness = 130;
  unsigned int revolutionTime = 3500;

  if(!counterclockwise)
    fadeToBlackBy( strip, NUM_LEDS-start, 35);  // this one applies to entire strip
  int cycleProgress = millis() % revolutionTime;
  if(!counterclockwise)
    cycleProgress = map(cycleProgress, 0, revolutionTime, 0, 1000);
  else
    cycleProgress = map(cycleProgress, revolutionTime, 0, 0, 1000);
  // Serial.println(cycleProgress);

  int pilotLed = map(cycleProgress, 0, 1000, start, length);
  //Serial.println(pilotLed);

  strip[pilotLed] += CHSV( 0, 255, localBrightness);
  cycleProgress = (cycleProgress + 333) % 1000;
  pilotLed = map(cycleProgress, 0, 1000, start, length);
  strip[pilotLed] += CHSV( 0, 255, localBrightness);
  cycleProgress = (cycleProgress + 333) % 1000;
  pilotLed = map(cycleProgress, 0, 1000, start, length);
  strip[pilotLed] += CHSV( 0, 255, localBrightness);

  if (!counterclockwise)
    lightschranke(strip, start, length, true);
}

int breatheProgress = 255;
int breatheDelta = -3;
void breathe(CRGB* strip, int start, int length)
{
  byte localBrightness = 220;
  byte localBrightnessMin = 80;

  breatheProgress += breatheDelta;
  //Serial.println(breatheProgress);
  if(breatheProgress <= localBrightnessMin || breatheProgress >= localBrightness){
    breatheDelta *= -1; 
    breatheProgress = constrain(breatheProgress, localBrightnessMin, localBrightness);
  }
  for(int i=start; i<start+length; i++){
    strip[i].setHSV(0, 255, breatheProgress);
  }
}

// ------------------------------
double mapF(double value, double start1, double stop1, double start2, double stop2)
{
    return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
}


