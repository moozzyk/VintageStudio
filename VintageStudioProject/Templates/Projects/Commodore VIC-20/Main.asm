                        * = $1100
__start
                        ; add your code here    

                        ldx #$00
loop                    lda msg,x 
                        beq end
                        jsr $ffd2
                        inx 
                        jmp loop
end                     rts 

msg                     .null 147, "HELLO WORLD!"
                         
                        .include Launcher.asm   
