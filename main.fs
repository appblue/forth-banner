\ C=64 fonts banner in FORTH
empty

( configuraiton )
variable c64set

( definitions )

\ convert u-string to counted string
\ ( c-str u -- c-str satus )
: >cstr   pad rot rot pad 2dup c! 1+ swap move ;
: >num    >cstr number ;

\ read eight hex values from input stream and put to dict
\ example> c8: $0A $0B $0C $0D $0A $0B $0C $0D
\ : c8:    8 0 do bl parse evaluate loop ;
: c8:    8 0 do bl parse >num 0= if c, else abort then loop ;

: bitmap 8 0 do dup $80 and 0= if 
           [char] . else 
           [char] # then 
         emit 1 lshift loop drop ;

: cod2addr   8 * c64set @ + ;
: c64chr     cod2addr 8 0 do dup c@ bitmap cr 1+ loop ;

( terminal control )
: clrscr ESC[ .\" 2J" ;
: home   ESC[ .\" H" ;
: 9up    ESC[ .\" 9A" ;
: 1up    ESC[ .\" 1A" ;

\ C=64 character set
create c64 c64 c64set !
c8: 0 1 2 3 4 5 6 7
c8: $ff 0 $ff 0 $ff 0 $ff 0
c8: 0 1 2 3 4 5 6 7
c8: 0 1 2 3 4 5 6 7
c8: 0 1 2 3 4 5 6 7

: anim   0 do i c64chr s" frame: " type i . cr 45 ms 1up 9up loop ;

( main loop )

page
100 anim

