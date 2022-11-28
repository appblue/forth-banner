\ C=64 fonts banner in FORTH
empty
decimal

( configuraiton )
variable c64set
variable fd-in

variable 'buf
variable 'text
variable #text
variable 'pos

81 value line-size
8  value #lines

( definitions )

\ convert u-string to counted string
\ ( c-str u -- c-str satus )
: >cstr      pad rot rot pad 2dup c! 1+ swap move ;
: >num       >cstr number ;

\ read eight hex values from input stream and put to dict
\ example> c8: $0A $0B $0C $0D $0A $0B $0C $0D
\ : c8:    8 0 do bl parse evaluate loop ;
: c8:        8 0 do bl parse >num 0= if c, else abort then loop ;

: bit2chr    dup 0= if drop 1 else 1 swap 0 do 2 * loop then and 0= if bl else [char] # then ; 
: bitmap     0 8 do dup i bit2chr emit 1 -loop drop ;

\ only upper letters, numbers and some special chars are allowed/mapped
\ from the C64 characterset
: ascii2pet  dup #65 >= over #90 <= and if #64 - exit then  
             dup #32 >= over #58 <= and if exit then 
             drop #32 ;
: ascii2addr ascii2pet 8 * c64set @ + ;
: c64chr     ascii2addr 8 0 do dup c@ bitmap cr 1+ loop drop ;

: lineaddr   ( n - addr ) line-size * 'buf @ + ;
: lineaddr$  ( n - addr ) lineaddr line-size 2 - + ;
: initcre    ( - )        here 'buf ! line-size #lines * allot ;
: initline   ( addr -   ) line-size 1- 0 do dup i + bl swap c! loop line-size 1- + 10 swap c! ;
: fillbuf    #lines 0 do i lineaddr initline loop ;
: initbuf    initcre fillbuf ;

: printbuf   'buf @ #lines line-size * type ;
: scrollbuf  #lines 0 do i lineaddr dup 1+ swap line-size 2 - cmove loop ;

0 value #bit

: updbuf     #lines 0 do  
               dup ascii2addr i + c@ 7 #bit - bit2chr
               i lineaddr$ c! 
             loop drop ;

( terminal control )
: clrscr     ESC[ .\" 2J" ;
: home       ESC[ .\" H" ;
: 8up        ESC[ .\" 8A" ;
: 8down      ESC[ .\" 8B" ;

: letter@    8 / + c@ ;
: _scroll    8 * 0 do printbuf scrollbuf i 8 mod to #bit dup i 8 / + c@ updbuf 100 ms 8up loop ;
: scroll     cr _scroll drop 8down ;

: banbuf     pad 10 0 do bl over i + c! loop swap 10 min cmove pad ; 
: banner     cr banbuf 10 8 * 0 do scrollbuf i 8 mod to #bit dup i 8 / + c@ updbuf loop drop printbuf ;

\ C=64 character set
create c64 c64 c64set !
c8: $3C $66 $6E $6E $60 $62 $3C $00 
c8: $18 $3C $66 $7E $66 $66 $66 $00 
c8: $7C $66 $66 $7C $66 $66 $7C $00 
c8: $3C $66 $60 $60 $60 $66 $3C $00 
c8: $78 $6C $66 $66 $66 $6C $78 $00 
c8: $7E $60 $60 $78 $60 $60 $7E $00 
c8: $7E $60 $60 $78 $60 $60 $60 $00 
c8: $3C $66 $60 $6E $66 $66 $3C $00 
c8: $66 $66 $66 $7E $66 $66 $66 $00 
c8: $3C $18 $18 $18 $18 $18 $3C $00 
c8: $1E $0C $0C $0C $0C $6C $38 $00 
c8: $66 $6C $78 $70 $78 $6C $66 $00 
c8: $60 $60 $60 $60 $60 $60 $7E $00 
c8: $63 $77 $7F $6B $63 $63 $63 $00 
c8: $66 $76 $7E $7E $6E $66 $66 $00 
c8: $3C $66 $66 $66 $66 $66 $3C $00 
c8: $7C $66 $66 $7C $60 $60 $60 $00 
c8: $3C $66 $66 $66 $66 $3C $0E $00 
c8: $7C $66 $66 $7C $78 $6C $66 $00 
c8: $3C $66 $60 $3C $06 $66 $3C $00 
c8: $7E $18 $18 $18 $18 $18 $18 $00 
c8: $66 $66 $66 $66 $66 $66 $3C $00 
c8: $66 $66 $66 $66 $66 $3C $18 $00 
c8: $63 $63 $63 $6B $7F $77 $63 $00 
c8: $66 $66 $3C $18 $3C $66 $66 $00 
c8: $66 $66 $66 $3C $18 $18 $18 $00 
c8: $7E $06 $0C $18 $30 $60 $7E $00 
c8: $3C $30 $30 $30 $30 $30 $3C $00 
c8: $0C $12 $30 $7C $30 $62 $FC $00 
c8: $3C $0C $0C $0C $0C $0C $3C $00 
c8: $00 $18 $3C $7E $18 $18 $18 $18 
c8: $00 $10 $30 $7F $7F $30 $10 $00 
c8: $00 $00 $00 $00 $00 $00 $00 $00 
c8: $18 $18 $18 $18 $00 $00 $18 $00 
c8: $66 $66 $66 $00 $00 $00 $00 $00 
c8: $66 $66 $FF $66 $FF $66 $66 $00 
c8: $18 $3E $60 $3C $06 $7C $18 $00 
c8: $62 $66 $0C $18 $30 $66 $46 $00 
c8: $3C $66 $3C $38 $67 $66 $3F $00 
c8: $06 $0C $18 $00 $00 $00 $00 $00 
c8: $0C $18 $30 $30 $30 $18 $0C $00 
c8: $30 $18 $0C $0C $0C $18 $30 $00 
c8: $00 $66 $3C $FF $3C $66 $00 $00 
c8: $00 $18 $18 $7E $18 $18 $00 $00 
c8: $00 $00 $00 $00 $00 $18 $18 $30 
c8: $00 $00 $00 $7E $00 $00 $00 $00 
c8: $00 $00 $00 $00 $00 $18 $18 $00 
c8: $00 $03 $06 $0C $18 $30 $60 $00 
c8: $3C $66 $6E $76 $66 $66 $3C $00 
c8: $18 $18 $38 $18 $18 $18 $7E $00 
c8: $3C $66 $06 $0C $30 $60 $7E $00 
c8: $3C $66 $06 $1C $06 $66 $3C $00 
c8: $06 $0E $1E $66 $7F $06 $06 $00 
c8: $7E $60 $7C $06 $06 $66 $3C $00 
c8: $3C $66 $60 $7C $66 $66 $3C $00 
c8: $7E $66 $0C $18 $18 $18 $18 $00 
c8: $3C $66 $66 $3C $66 $66 $3C $00 
c8: $3C $66 $66 $3E $06 $66 $3C $00 
c8: $00 $00 $18 $00 $00 $18 $00 $00 
c8: $00 $00 $18 $00 $00 $18 $18 $30 
c8: $0E $18 $30 $60 $30 $18 $0E $00 
c8: $00 $00 $7E $00 $7E $00 $00 $00 
c8: $70 $18 $0C $06 $0C $18 $70 $00 
c8: $3C $66 $06 $0C $18 $00 $18 $00 
c8: $00 $00 $00 $FF $FF $00 $00 $00 
c8: $08 $1C $3E $7F $7F $1C $3E $00 
c8: $18 $18 $18 $18 $18 $18 $18 $18 
c8: $00 $00 $00 $FF $FF $00 $00 $00 
c8: $00 $00 $FF $FF $00 $00 $00 $00 
c8: $00 $FF $FF $00 $00 $00 $00 $00 
c8: $00 $00 $00 $00 $FF $FF $00 $00 
c8: $30 $30 $30 $30 $30 $30 $30 $30 
c8: $0C $0C $0C $0C $0C $0C $0C $0C 
c8: $00 $00 $00 $E0 $F0 $38 $18 $18 
c8: $18 $18 $1C $0F $07 $00 $00 $00 
c8: $18 $18 $38 $F0 $E0 $00 $00 $00 
c8: $C0 $C0 $C0 $C0 $C0 $C0 $FF $FF 
c8: $C0 $E0 $70 $38 $1C $0E $07 $03 
c8: $03 $07 $0E $1C $38 $70 $E0 $C0 
c8: $FF $FF $C0 $C0 $C0 $C0 $C0 $C0 
c8: $FF $FF $03 $03 $03 $03 $03 $03 
c8: $00 $3C $7E $7E $7E $7E $3C $00 
c8: $00 $00 $00 $00 $00 $FF $FF $00 
c8: $36 $7F $7F $7F $3E $1C $08 $00 
c8: $60 $60 $60 $60 $60 $60 $60 $60 
c8: $00 $00 $00 $07 $0F $1C $18 $18 
c8: $C3 $E7 $7E $3C $3C $7E $E7 $C3 
c8: $00 $3C $7E $66 $66 $7E $3C $00 
c8: $18 $18 $66 $66 $18 $18 $3C $00 
c8: $06 $06 $06 $06 $06 $06 $06 $06 
c8: $08 $1C $3E $7F $3E $1C $08 $00 
c8: $18 $18 $18 $FF $FF $18 $18 $18 
c8: $C0 $C0 $30 $30 $C0 $C0 $30 $30 
c8: $18 $18 $18 $18 $18 $18 $18 $18 
c8: $00 $00 $03 $3E $76 $36 $36 $00 
c8: $FF $7F $3F $1F $0F $07 $03 $01 
c8: $00 $00 $00 $00 $00 $00 $00 $00 
c8: $F0 $F0 $F0 $F0 $F0 $F0 $F0 $F0 
c8: $00 $00 $00 $00 $FF $FF $FF $FF 
c8: $FF $00 $00 $00 $00 $00 $00 $00 
c8: $00 $00 $00 $00 $00 $00 $00 $FF 
c8: $C0 $C0 $C0 $C0 $C0 $C0 $C0 $C0 
c8: $CC $CC $33 $33 $CC $CC $33 $33 
c8: $03 $03 $03 $03 $03 $03 $03 $03 
c8: $00 $00 $00 $00 $CC $CC $33 $33 
c8: $FF $FE $FC $F8 $F0 $E0 $C0 $80 
c8: $03 $03 $03 $03 $03 $03 $03 $03 
c8: $18 $18 $18 $1F $1F $18 $18 $18 
c8: $00 $00 $00 $00 $0F $0F $0F $0F 
c8: $18 $18 $18 $1F $1F $00 $00 $00 
c8: $00 $00 $00 $F8 $F8 $18 $18 $18 
c8: $00 $00 $00 $00 $00 $00 $FF $FF 
c8: $00 $00 $00 $1F $1F $18 $18 $18 
c8: $18 $18 $18 $FF $FF $00 $00 $00 
c8: $00 $00 $00 $FF $FF $18 $18 $18 
c8: $18 $18 $18 $F8 $F8 $18 $18 $18 
c8: $C0 $C0 $C0 $C0 $C0 $C0 $C0 $C0 
c8: $E0 $E0 $E0 $E0 $E0 $E0 $E0 $E0 
c8: $07 $07 $07 $07 $07 $07 $07 $07 
c8: $FF $FF $00 $00 $00 $00 $00 $00 
c8: $FF $FF $FF $00 $00 $00 $00 $00 
c8: $00 $00 $00 $00 $00 $FF $FF $FF 
c8: $03 $03 $03 $03 $03 $03 $FF $FF 
c8: $00 $00 $00 $00 $F0 $F0 $F0 $F0 
c8: $0F $0F $0F $0F $00 $00 $00 $00 
c8: $18 $18 $18 $F8 $F8 $00 $00 $00 
c8: $F0 $F0 $F0 $F0 $00 $00 $00 $00 
c8: $F0 $F0 $F0 $F0 $0F $0F $0F $0F 

( main loop )

page
initbuf
s" BANNER" banner
