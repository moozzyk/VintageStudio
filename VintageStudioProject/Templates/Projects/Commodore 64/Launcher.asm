; makes the program runnable from BASIC

                  	*=$0801
					.word __nextline            ; pointer to the next BASIC line 
					.word 10					; BASIC line number
					.null $9e,^__start			; sys <__start>
					.byte 0						; end of line marker
__nextline			.word 0						; end of basic program marker
