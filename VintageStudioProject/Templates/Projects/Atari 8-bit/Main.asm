                        * = $0600
ICCOM = $0342
ICBA = $0344
ICBL = $0348
CIOV = $E456
                        ldx #$00            ; Since it's IOCB0  
                        lda #$09            ; For put record  
                        sta ICCOM,x         ; Command byte  
                        lda #<msg           ; Low byte of MSG  
                        sta ICBA,x          ;  into ICBAL  
                        lda #>msg           ; High byte of MSG  
                        sta ICBA + 1,x      ;  into ICBAH  
                        lda #$00            ; Length of MSG  
                        sta ICBL + 1,x      ;  high byte  
                        lda #$ff            ; Length of MSG  
                        sta ICBL,x          ; See discussion   
                        jsr CIOV

                        jmp *


msg                     .text "Hello World!",$9b