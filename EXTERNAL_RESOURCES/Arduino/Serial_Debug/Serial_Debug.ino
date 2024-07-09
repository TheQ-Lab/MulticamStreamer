void setup() {
  Serial.begin(115200);
  Serial.println("Waiting for serial char: r>");
  pinMode(LED_BUILTIN, OUTPUT);
}

bool led = false;
void loop() {
  if(Serial.available() > 0){
    byte serialInByte = Serial.read();
    if(char(serialInByte) == 'r'){
      led = !led;
      digitalWrite(LED_BUILTIN, led);
    }
  }
}
